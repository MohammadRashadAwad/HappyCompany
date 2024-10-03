namespace Application.Common.Shared
{
    public interface IPagedQuery<TResponse> : IRequest<PagedResult<TResponse>>
    {

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
