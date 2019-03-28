using System;

namespace RepoitoryPattern.AppCode
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