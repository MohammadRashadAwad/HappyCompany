using Domain.Enums;
namespace Application.LogViewer
{
    public class AuditLog
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

    }
}
