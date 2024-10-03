using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Application.Common.Shared
{
    public record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("General.Null", "Null value was provided");

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }

        public string Message { get; }

        public override string ToString() => JsonSerializer.Serialize(this);

    }
    public sealed record ValidationError : Error
    {
        public ValidationError(Error[] errors) : base("Validation.General", "One or more validation errors occurred")
        {
            Errors = errors;
        }
        public Error[] Errors { get; }
    }

    public sealed record IdentityErrors : Error
    {
        public IdentityErrors(Error[] errors) : base("Identity.Errors", "One or more Identity errors occurred")
        {
            Errors = errors;
        }
        public Error[] Errors { get; }

    }
}
