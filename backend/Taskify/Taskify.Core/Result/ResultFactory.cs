using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Result
{
    public static class ResultFactory
    {
        public static Result<T> Success<T>(T data)
        {
            return new Result<T>(true, data, null);
        }

        public static Result<T> Success<T>(Result<T> result)
        {
            return new Result<T>(result.IsSuccess, result.Data, result.Errors);
        }

        public static Result<T> Failure<T>(Result<T> result)
        {
            return new Result<T>(result.IsSuccess, result.Data, result.Errors);
        }

        public static Result<T> Failure<T>(List<string> errors)
        {
            return new Result<T>(false, default(T), errors);
        }

        public static Result<T> Failure<T>(string error)
        {
            return new Result<T>(false, default(T), new List<string> { error });
        }
    }
}
