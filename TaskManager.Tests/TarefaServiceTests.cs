using System;
using System.Collections.Generic;
using Moq;
using GestaoTarefas.Core.Entities;
using GestaoTarefas.Core.Enums;
using GestaoTarefas.Core.Interfaces;
using GestaoTarefas.Core.Services;
using Xunit;

namespace GestaoTarefas.Tests
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _mockRepo;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _mockRepo = new Mock<ITarefaRepository>();
            _service = new TarefaService(_mockRepo.Object);
        }

        [Fact]
        public void Create_ShouldAddTask_WhenValidData()
        {
            var titulo = "Nova tarefa";
            var descricao = "Descrição da tarefa";
            var prioridade = TarefaPrioridade.Alta;

            _mockRepo.Setup(r => r.GerarProximoId()).Returns(1);

            var task = _service.Adicionar(titulo, descricao, prioridade);

            _mockRepo.Verify(r => r.Adicionar(It.Is<Tarefa>(t =>
                t.Titulo == titulo &&
                t.Descricao == descricao &&
                t.Prioridade == prioridade &&
                t.Status == TarefaStatus.Pendente
            )), Times.Once);
        }

        [Fact]
        public void Create_ShouldThrowException_WhenTitleIsEmpty()
        {
            var titulo = "";
            var descricao = "Descrição";
            var prioridade = TarefaPrioridade.Baixa;

            Assert.Throws<ArgumentException>(() => _service.Adicionar(titulo, descricao, prioridade));
        }

        [Fact]
        public void Update_ShouldModifyTask_WhenTaskExists()
        {
            var id = 1;
            var existingTask = new Tarefa("Antigo", "Desc", TarefaPrioridade.Media)
            {
                Id = id
            };

            _mockRepo.Setup(r => r.ConsultarPorId(id)).Returns(existingTask);

            _service.Atualizar(id, "Novo", "Nova desc", TarefaPrioridade.Alta, TarefaStatus.Finalizada);

            _mockRepo.Verify(r => r.Atualizar(It.Is<Tarefa>(t =>
                t.Id == id &&
                t.Titulo == "Novo" &&
                t.Descricao == "Nova desc" &&
                t.Prioridade == TarefaPrioridade.Alta &&
                t.Status == TarefaStatus.Finalizada &&
                t.FinalizadaEm.HasValue
            )), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrow_WhenTaskNotFound()
        {
            _mockRepo.Setup(r => r.ConsultarPorId(It.IsAny<int>())).Returns((Tarefa)null);

            Assert.Throws<KeyNotFoundException>(() =>
                _service.Atualizar(1, "Novo", "Desc", TarefaPrioridade.Alta, TarefaStatus.Pendente));
        }

        [Fact]
        public void Delete_ShouldRemoveTask_WhenTaskExists()
        {
            var id = 1;
            var task = new Tarefa("Tarefa", "Desc", TarefaPrioridade.Media) { Id = id };
            _mockRepo.Setup(r => r.ConsultarPorId(id)).Returns(task);

            _service.Excluir(id);

            _mockRepo.Verify(r => r.Excluir(id), Times.Once);
        }

        [Fact]
        public void FilterByStatus_ShouldReturnMatchingTasks()
        {
            var tasks = new List<Tarefa>
            {
                new Tarefa("T1", "D1", TarefaPrioridade.Baixa)
            };
            _mockRepo.Setup(r => r.ListarTodos()).Returns(tasks);

            var result = _service.FiltrarPorStatus(TarefaStatus.Pendente);

            Assert.Single(result);
        }

        [Fact]
        public void FilterByPriority_ShouldReturnMatchingTasks()
        {
            var tasks = new List<Tarefa>
            {
                new Tarefa("T1", "D1", TarefaPrioridade.Alta),
                new Tarefa("T2", "D2", TarefaPrioridade.Baixa)
            };
            _mockRepo.Setup(r => r.ListarTodos()).Returns(tasks);

            var result = _service.FiltrarPorPrioridade(TarefaPrioridade.Alta);

            Assert.Single(result);
        }
    }
}
