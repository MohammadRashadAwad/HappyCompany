﻿namespace Application.Common.Shared
{
    public interface ICommand : IRequest<Result> { }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

}


