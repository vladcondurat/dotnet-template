using Application.DTOs;
using Application.Services;
using GeminiSharp.Client;

namespace Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly GeminiClient _client;
        private const string Model = "gemini-2.0-flash-thinking-exp";

        public GeminiService()
            : this(Environment.GetEnvironmentVariable("GEMINI_API_KEY"))
        {
        }

        public GeminiService(string? apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Gemini API key is not set in the environment variable 'GEMINI_API_KEY'.");

            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(60)
            };
            _client = new GeminiClient(httpClient, apiKey);
        }

        public async Task<string> GenerateSummaryAsync(string content, int size)
        {
            var minWords = (int)(0.75 * size);
            var maxWords = (int)(1.25 * size);

            var prompt = $@"
You are an educational AI. Summarize the following content into a single HTML <section>…</section> optimized for learning. Be strict: all variable names outside full formulas must be plain and simple; full formulas remain in LaTeX.
- Use <h1 style=""color:#1E88E5;text-decoration:underline;""> for the main title.
- Use <h2 style=""color:#D32F2F;""> for major section headings and <h3 style=""color:#388E3C;""> for subsections.
- Under each heading, present key points as bullet lists (<ul><li>…</li></ul>).
- Emphasize important terms by bolding (<strong>) or coloring (<span style=""background-color:#FFFF8D;"">…</span>).
- Wrap full mathematical formulas in LaTeX between $$…$$ and render as:
  <div data-type=""math"" class=""math-block"" data-equation=""RAW_LATEX"">KATEX_HTML</div>
- Strict rule for inline variables (no LaTeX):
    • F<sub>12</sub> (force exerted by body 1 on body 2), not \\vec{{F}}_{{12}}  
    • r<sub>12</sub> (distance between bodies), not $r = |\\vec{{r}}_{{12}}|$  
    • r̂<sub>21</sub> (unit vector from body 2 to body 1), not \\hat{{r}}_{{21}}  
- If you need to show an algebraic operation or magnitude, wrap the entire expression in a LaTeX block; otherwise keep it in words or plain sub/sup.
- Encourage judicious use of color to group or emphasize meaning.
- Do not include markdown fences or backticks.
- Allowed HTML tags: <section>, <h1>, <h2>, <h3>, <p>, <span>, <div style=""text-align:…;"">, <ul>, <ol>, <li>, <blockquote>, <pre><code>, <sub>, <sup>, <table>, <tr>, <td>.
- Maintain a friendly, approachable tone; chunk content into logical sections.
- The summary must contain between {minWords} and {maxWords} visible words (ignore HTML/Katex tags). Do not pad or truncate just to meet this range.
- Output exactly one <section>…</section> with no extra text.

Content:
{content}

Output:
";

            var response = await _client.GenerateContentAsync(Model, prompt);
            var html = response.Candidates?[0].Content?.Parts?[0].Text
                       ?? throw new InvalidOperationException("No summary generated.");

            return html.Trim();
        }

        public async Task<IEnumerable<QuestionGenerationResult>> GenerateQuestionsAsync(
            string topic,
            int numberOfQuestions,
            string difficulty,
            string summaryText)
        {
            var prompt = $@"
You are an educational AI. Create {numberOfQuestions} multiple-choice questions on the topic ""{topic}"" with difficulty ""{difficulty}"": 
- Base each question only on the summary below; do not reference the word summary or source in the question.
- Format each as:
  Question: [text]
  Options:
    A. [option A]
    B. [option B]
    C. [option C]
    D. [option D]
  Correct Answer: [letter]
- Use plausible distractors and a clear, helpful tone.

Summary:
{summaryText}

Output:
";

            var response = await _client.GenerateContentAsync(Model, prompt);
            var text = response.Candidates?[0].Content?.Parts?[0].Text ?? "";

            return ParseQuestions(text);
        }

        public async Task<IEnumerable<FlashcardGenerationResult>> GenerateFlashcardsAsync(
            string summaryText,
            int numberOfFlashcards,
            string difficulty,
            string? focusArea)
        {
            var prompt = $@"
You are an educational AI. Generate {numberOfFlashcards} flashcards from the following summary:
- Do not reference the original content.
- Format each card as:
    Q: [question]
    A: [answer]
- Focus on single concepts per card.
- Present all flashcards as a bullet list (<ul><li>…</li></ul>), no backticks.

Summary:
{summaryText}

Output:
";

            var response = await _client.GenerateContentAsync(Model, prompt);
            var text = response.Candidates?[0].Content?.Parts?[0].Text ?? "";

            return ParseFlashcards(text);
        }

        private static IEnumerable<QuestionGenerationResult> ParseQuestions(string text)
        {
            var questions = new List<QuestionGenerationResult>();
            var blocks = text.Split("Question:", StringSplitOptions.RemoveEmptyEntries);

            foreach (var block in blocks)
            {
                var lines = block.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(l => l.Trim())
                                 .ToList();

                // Parse question text
                var questionLine = lines.FirstOrDefault(l => !l.StartsWith("Options:") && !l.StartsWith("A.") && !l.StartsWith("B.") && !l.StartsWith("C.") && !l.StartsWith("D.") && !l.StartsWith("Correct Answer:"));
                if (string.IsNullOrEmpty(questionLine))
                    continue;

                var questionText = questionLine;

                // Parse options into dictionary
                var options = new Dictionary<string, string>();
                foreach (var optLine in lines.Where(l => l.StartsWith("A.") || l.StartsWith("B.") || l.StartsWith("C.") || l.StartsWith("D.")))
                {
                    var key = optLine.Substring(0, 1);
                    var value = optLine.Substring(2).Trim();
                    options[key] = value;
                }

                // Get correct letter and text
                var correctLetter = lines.FirstOrDefault(l => l.StartsWith("Correct Answer:"))
                                         ?.Replace("Correct Answer:", "").Trim();
                if (string.IsNullOrEmpty(correctLetter) || !options.ContainsKey(correctLetter))
                    continue;

                var correctText = options[correctLetter];

                var incorrect = options
                    .Where(kv => kv.Key != correctLetter)
                    .Select(kv => kv.Value)
                    .ToList();

                if (incorrect.Count >= 3)
                {
                    questions.Add(new QuestionGenerationResult
                    {
                        Question = questionText,
                        CorrectAnswer = correctText,
                        IncorrectAnswers = incorrect.Take(3).ToList()
                    });
                }
            }

            return questions;
        }

        private static IEnumerable<FlashcardGenerationResult> ParseFlashcards(string text)
        {
            var flashcards = new List<FlashcardGenerationResult>();
            var pairs = text.Split("Q:", StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var lines = pair.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length < 2) continue;

                var question = lines[0].Trim();
                var answerLine = lines.FirstOrDefault(l => l.StartsWith("A:"));
                if (answerLine == null) continue;

                var answer = answerLine.Replace("A:", "").Trim();
                flashcards.Add(new FlashcardGenerationResult { Question = question, Answer = answer });
            }

            return flashcards;
        }
    }
}