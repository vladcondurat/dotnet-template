namespace Domain.Exceptions
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }
        public Guid? EntityId { get; }
        public Type? EntityType { get; }
        public IEnumerable<string>? ValidationErrors { get; }

        public Error(string code, string message, Guid? entityId = null, Type? entityType = null, IEnumerable<string>? validationErrors = null)
        {
            Code = code;
            Message = message;
            EntityId = entityId;
            EntityType = entityType;
            ValidationErrors = validationErrors;
        }

        public static readonly Error None = new Error(ErrorCodes.ServerError, string.Empty);

        public static Error EntityNotFound(Guid entityId, Type entityType)
        {
            return new Error(
                ErrorCodes.EntityNotFound,
                $"Entity '{entityType.Name}' not found for ID {entityId}.",
                entityId,
                entityType
            );
        }
        
        public static Error Unauthorized(string message = "Authorization has been denied for this request.")
        {
            return new Error(ErrorCodes.Unauthorized, message);
        }

        public static Error ValidationError(IEnumerable<string> validationErrors)
        {
            return new Error(
                ErrorCodes.ValidationError,
                "One or more validation errors occurred.",
                validationErrors: validationErrors
            );
        }

        public static Error ValidationError(string validationError)
        {
            return ValidationError(new[] { validationError });
        }
        public static Error Conflict(string message)
        {
            return new Error(ErrorCodes.Conflict, message);
        }

        public static Error ServerError(string message = "An unexpected server error occurred.")
        {
            return new Error(ErrorCodes.ServerError, message);
        }
        
        public static Error ExternalServiceError(string message)
        {
            return new Error(ErrorCodes.ExternalServiceError, message);
        }

        public static Error Forbidden(string message = "You do not have permission to access this resource.")
        {
            return new Error(ErrorCodes.Forbidden, message);
        }

        public override string ToString() => $"{Code}: {Message}";
    }
}
