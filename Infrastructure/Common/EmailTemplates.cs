namespace Infrastructure.Services;

public static class EmailTemplates
{
    public static string GetEmailConfirmationTemplate(string confirmationLink, string username)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body>
    <div class='container'>
        <h2>Welcome to Learn Buddy!</h2>
        <p>Thank you for registering. Please confirm your email address by clicking the button below:</p>
        <a href='{confirmationLink}' class='button'>Confirm Email</a>
        <p>If you didn't create an account, you can safely ignore this email.</p>
        <p>Best regards,<br>The Learn Buddy Team</p>
    </div>
</body>
</html>";
    }
    
    public static string GetPasswordResetTemplate(string resetLink, string username)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body>
    <div class='container'>
        <h2>Password Reset Request</h2>
        <p>Hello {username},</p>
        <p>We received a request to reset your password for your Learn Buddy account. Click the button below to set a new password:</p>
        <a href='{resetLink}' class='button'>Reset Password</a>
        <p>If you didn't request a password reset, you can safely ignore this email. Your password will remain unchanged.</p>
        <p>This link will expire in 24 hours for security reasons.</p>
        <p>Best regards,<br>The Learn Buddy Team</p>
    </div>
</body>
</html>";
    }
} 