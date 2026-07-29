using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Services
{
    public class FollowUpService
    {
        private readonly AppDbContext _context;

        public FollowUpService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FollowUpTask>> GetOpenFollowUps()
        {
            return await _context.FollowUpTasks
                .Where(t => t.Status == TalentBridgeBackEnd.Models.Enums.TaskStatus.Open || t.Status == TalentBridgeBackEnd.Models.Enums.TaskStatus.InProgress)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task UpdateFollowUp(int id, TalentBridgeBackEnd.Models.Enums.TaskStatus status, string? notes)
        {
            var task = await _context.FollowUpTasks.FindAsync(id);
            if (task == null) throw new Exception("Task not found");

            task.Status = status;
            task.ResolutionNotes = notes;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
