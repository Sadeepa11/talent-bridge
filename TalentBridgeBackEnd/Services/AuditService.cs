using System;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Data;

namespace TalentBridgeBackEnd.Services
{
    public class AuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAuditAsync(int actorId, string entityType, long entityId, string action, string? before, string? after, string ip)
        {
            var auditLog = new AuditLog
            {
                ActorUserId = actorId,
                EntityType = entityType,
                EntityId = entityId.ToString(),
                Action = action,
                BeforeJson = before,
                AfterJson = after,
                IpAddress = ip,
                OccurredAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
