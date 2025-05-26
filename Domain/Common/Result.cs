using Domain.Exceptions;

namespace Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new ArgumentException("Invalid error configuration", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
        {
            return new Result(true, Error.None);
        }

        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }

        public static Result EntityNotFound(Guid entityId, Type entityType)
        {
            return Failure(Error.EntityNotFound(entityId, entityType));
        }

        public static Result Unauthorized(string message = "Authorization has been denied for this request.")
        {
            return Failure(Error.Unauthorized(message));
        }

        public static Result Conflict(string message)
        {
            return Failure(Error.Conflict(message));
        }

        public static Result ValidationError(IEnumerable<string> validationErrors)
        {
            return Failure(Error.ValidationError(validationErrors));
        }
        
        public static implicit operator Result(Error error)
        {
            return Failure(error);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(bool isSuccess, Error error, T value)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, Error.None, value);
        }

        public new static Result<T> Failure(Error error)
        {
            return new Result<T>(false, error, default!);
        }

        public static Result<T> ValidationError(IEnumerable<string> validationErrors)
        {
            return Failure(Error.ValidationError(validationErrors));
        }
        public new static Result<T> EntityNotFound(Guid entityId, Type entityType)
        {
            return Failure(Error.EntityNotFound(entityId, entityType));
        }

        public new static Result<T> Unauthorized(string message = "Authorization has been denied for this request.")
        {
            return Failure(Error.Unauthorized(message));
        }

        public new static Result<T> Conflict(string message)
        {
            return Failure(Error.Conflict(message));
        }
        
        public static implicit operator Result<T>(Error error)
        {
            return Failure(error);
        }
    }
}
