using System.Collections.Generic;
using System.Linq;

namespace CRM.Domain.Core
{
    public class Result
    {
        public bool IsFailure => Messages.Any();
        public bool IsSuccess => !IsFailure;
        
        public List<string> Messages { get; } = new List<string>();

        private Result(IEnumerable<string> messages)
        {
            Messages.AddRange(messages);
        }

        public static Result Successful() => new Result(new string[] { });
        public static Result Failure(IEnumerable<string> messages) => new Result(messages);
        public static Result Failure(string message) => new Result(new string[] { message });
    }
}