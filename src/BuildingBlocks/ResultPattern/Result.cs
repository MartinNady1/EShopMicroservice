using System;
using System.Collections.Generic;
using System.Text;

namespace ResultPattern
{
    public class Result<T>
    {
        public T? Value { get; }
        public Error? Error { get; }
        public bool IsSuccess => Error == null;
        private Result(T? value)

        {
            Value = value;
            Error = null;

        }

        private Result(Error error)
        {
            Error = error;
            Value = default;

        }
        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(Error error) => new Result<T>(error);
        public TResult Map<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
        {
            return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
        }


    }
}
