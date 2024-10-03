using Microsoft.EntityFrameworkCore;

namespace Application.Common.Shared
{
    public class PagedResult<TEntity>
    {
        public IReadOnlyList<TEntity> Items { get; set; }
        public int TotalRecord { get; set; }

        private PagedResult(List<TEntity> items, int totalRecord)
        {
            Items = items;
            TotalRecord = totalRecord;
        }

        public static async Task<PagedResult<TEntity>> CreateAsync(IQueryable<TEntity> query, int pageIndex, int pageSize)
        {
            var totalRecord = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new(items, totalRecord);
        }
    }
}
