using Domain.Enums;
namespace Application.LogViewer
{
    public class LogViewerQuery : IPagedQuery<AuditLog>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public LogLevelType? LogLevel { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
