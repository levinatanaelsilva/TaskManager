using System;
using System.Collections.Generic;
using Moq;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Services;
using Xunit;

namespace TaskManager.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepo;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _mockRepo = new Mock<ITaskRepository>();
            _service = new TaskService(_mockRepo.Object);
        }

        [Fact]
        public void Create_ShouldAddTask_WhenValidData()
        {
            // Arrange
            var title = "Nova tarefa";
            var description = "Descrição da tarefa";
            var priority = TaskPriority.Alta;

            // Act
            _service.Create(title, description, priority);

            // Assert
            _mockRepo.Verify(r => r.Add(It.Is<TaskItem>(t =>
                t.Title == title &&
                t.Description == description &&
                t.Priority == priority &&
                t.Status == TaskStatus.Pendente
            )), Times.Once);
        }

        [Fact]
        public void Create_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange
            var title = "";
            var description = "Descrição";
            var priority = TaskPriority.Baixa;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.Create(title, description, priority));
        }

        [Fact]
        public void Update_ShouldModifyTask_WhenTaskExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingTask = new TaskItem
            {
                Id = id,
                Title = "Antigo",
                Description = "Desc",
                Priority = TaskPriority.Media,
                Status = TaskStatus.Pendente
            };

            _mockRepo.Setup(r => r.GetById(id)).Returns(existingTask);

            // Act
            _service.Update(id, "Novo", "Nova desc", TaskPriority.Alta, TaskStatus.Finalizada);

            // Assert
            _mockRepo.Verify(r => r.Update(It.Is<TaskItem>(t =>
                t.Id == id &&
                t.Title == "Novo" &&
                t.Description == "Nova desc" &&
                t.Priority == TaskPriority.Alta &&
                t.Status == TaskStatus.Finalizada &&
                t.CompletedAt.HasValue
            )), Times.Once);
        }

        [Fact]
        public void Delete_ShouldRemoveTask_WhenTaskExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskItem { Id = id };
            _mockRepo.Setup(r => r.GetById(id)).Returns(task);

            // Act
            _service.Delete(id);

            // Assert
            _mockRepo.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public void FilterByStatus_ShouldReturnMatchingTasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Status = TaskStatus.Pendente },
                new TaskItem { Status = TaskStatus.Finalizada }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            var result = _service.FilterByStatus(TaskStatus.Pendente);

            // Assert
            Assert.Single(result);
        }
    }
}
