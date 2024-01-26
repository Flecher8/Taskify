using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Result
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public List<string> Errors { get; private set; }

        public Result(bool isSuccess, T data, List<string> errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Errors = errors ?? new List<string>();
        }
    }
}
