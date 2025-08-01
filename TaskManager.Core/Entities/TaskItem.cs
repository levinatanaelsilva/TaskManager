using System;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = TaskPriority.Media;
        public TaskStatus Status { get; set; } = TaskStatus.Pendente;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
    }
}