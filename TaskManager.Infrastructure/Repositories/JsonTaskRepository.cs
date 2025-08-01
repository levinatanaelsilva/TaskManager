using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.Infrastructure.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;
        private List<TaskItem> _tasks;

        public JsonTaskRepository(string filePath)
        {
            _filePath = filePath;
            _tasks = LoadTasks();
        }

        private List<TaskItem> LoadTasks()
        {
            if (!File.Exists(_filePath))
                return new List<TaskItem>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        private void SaveTasks()
        {
            var json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<TaskItem> GetAll() => _tasks;

        public TaskItem GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

        public void Add(TaskItem task)
        {
            _tasks.Add(task);
            SaveTasks();
        }

        public void Update(TaskItem task)
        {
            var index = _tasks.FindIndex(t => t.Id == task.Id);
            if (index >= 0)
            {
                _tasks[index] = task;
                SaveTasks();
            }
        }

        public void Delete(int id)
        {
            var task = GetById(id);
            if (task != null)
            {
                _tasks.Remove(task);
                SaveTasks();
            }
        }
    }
}