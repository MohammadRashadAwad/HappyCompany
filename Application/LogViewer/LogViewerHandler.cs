using Application.Common.Data;

namespace Application.LogViewer
{
    public class LogViewerHandler : IPagedQueryHandler<LogViewerQuery, AuditLog>
    {
        private readonly IApplicationDbContext _context;
        public LogViewerHandler(IApplicationDbContext context) => _context = context;

        public async Task<PagedResult<AuditLog>> Handle(LogViewerQuery request, CancellationToken cancellationToken)
        {
            var query = _context.SystemLogs.AsQueryable();
            if (request.FromDate.HasValue)
                query = query.Where(x => x.Timestamp >= request.FromDate);

            if (request.ToDate.HasValue)
                query = query.Where(x => x.Timestamp <= request.ToDate);

            if (request.LogLevel.HasValue && request.LogLevel != LogLevelType.All)
                query = query.Where(p => p.Level.ToLower() == request.LogLevel.ToString()!.ToLower());


            var auditLog = query.Select(p => new AuditLog
            {
                Timestamp = p.Timestamp,
                Exception = p.Exception,
                Level = p.Level,
                Message = p.RenderedMessage
            });
            return await PagedResult<AuditLog>.CreateAsync(auditLog, request.PageIndex, request.PageSize);
        }
    }
}
