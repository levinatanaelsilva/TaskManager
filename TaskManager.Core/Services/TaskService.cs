using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;

namespace TaskManager.Core.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TaskItem> GetAll() => _repository.GetAll();

        public TaskItem GetById(int id) => _repository.GetById(id);

        public void Create(string title, string description, TaskPriority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título da tarefa é obrigatório.");

            var tasks = _repository.GetAll();
            var nextId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;

            var task = new TaskItem
            {
                Id = nextId,
                Title = title,
                Description = description,
                Priority = priority,
                Status = TaskStatus.Pendente,
                CreatedAt = DateTime.Now
            };

            _repository.Add(task);
        }

        public void Update(int id, string title, string description, TaskPriority priority, TaskStatus status)
        {
            var task = _repository.GetById(id);
            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            task.Title = title;
            task.Description = description;
            task.Priority = priority;
            task.Status = status;

            if (status == TaskStatus.Finalizada && task.CompletedAt == null)
                task.CompletedAt = DateTime.Now;

            _repository.Update(task);
        }

        public void Delete(int id)
        {
            var task = _repository.GetById(id);
            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            _repository.Delete(id);
        }

        public IEnumerable<TaskItem> FilterByStatus(TaskStatus status)
        {
            return _repository.GetAll().Where(t => t.Status == status);
        }

        public IEnumerable<TaskItem> FilterByPriority(TaskPriority priority)
        {
            return _repository.GetAll().Where(t => t.Priority == priority);
        }
    }
}
