using System;

namespace MvcTemplate.Core
{
    public interface IResult
    {
        Exception Exception { get; set; }
        Guid ID { get; }
        System.Collections.Generic.List<IResult> InnerResults { get; }
        string Message { get; set; }
        bool Success { get; set; }
    }
}