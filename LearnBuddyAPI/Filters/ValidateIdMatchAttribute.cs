using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateIdMatchAttribute : ActionFilterAttribute
    {
        private readonly string _routeKey;

        public ValidateIdMatchAttribute(string routeKey = "id")
            => _routeKey = routeKey;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.RouteData.Values.TryGetValue(_routeKey, out var routeObj)
                || !Guid.TryParse(routeObj?.ToString(), out var routeId))
            {
                base.OnActionExecuting(context);
                return;
            }

            var body = context.ActionArguments
                .Values
                .FirstOrDefault(v => v?.GetType().GetProperty("Id") != null);
            if (body == null)
            {
                base.OnActionExecuting(context);
                return;
            }

            var bodyId = body.GetType()
                .GetProperty("Id")!
                .GetValue(body) as Guid?;

            if (bodyId.HasValue && bodyId.Value != routeId)
            {
                var pd = new ProblemDetails
                {
                    Title  = "ID in route does not match ID in body.",
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(pd)
                {
                    DeclaredType = typeof(ProblemDetails)
                };
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}