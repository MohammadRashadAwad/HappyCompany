namespace Application.Common.Shared
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

}

