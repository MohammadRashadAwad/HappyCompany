namespace Application.Common.Shared
{
    public interface IPagedQueryHandler<in TQuery, TResponse> :
        IRequestHandler<TQuery, PagedResult<TResponse>>
   where TQuery : IPagedQuery<TResponse>
    { }
}
