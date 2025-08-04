using System;
using System.Collections.Generic;
using System.Linq;
using GestaoTarefas.Core.Entities;
using GestaoTarefas.Core.Enums;
using GestaoTarefas.Core.Interfaces;

namespace GestaoTarefas.Core.Services
{
    public class TarefaService
    {
        private readonly ITarefaRepository _repository;

        public TarefaService(ITarefaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Tarefa> ListarTodos() => _repository.ListarTodos();

        public Tarefa ConsultarPorId(int id) => _repository.ConsultarPorId(id);

        public Tarefa Adicionar(string titulo, string descricao, TarefaPrioridade prioridade)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título da tarefa é obrigatório.");
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A descrição da tarefa é obrigatória.");

            var nextId = _repository.GerarProximoId();
            var task = new Tarefa(titulo, descricao, prioridade)
            {
                Id = nextId
            };

            _repository.Adicionar(task);
            return task;
        }

        public Tarefa Atualizar(int id, string titulo, string descricao, TarefaPrioridade prioridade, TarefaStatus status)
        {
            var task = _repository.ConsultarPorId(id);
            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            task.Atualizar(titulo, descricao, prioridade, status);
            _repository.Atualizar(task);
            return task;
        }

        public void Excluir(int id)
        {
            var task = _repository.ConsultarPorId(id);
            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            _repository.Excluir(id);
        }

        public IEnumerable<Tarefa> FiltrarPorStatus(TarefaStatus status)
            => _repository.ListarTodos().Where(t => t.Status == status);

        public IEnumerable<Tarefa> FiltrarPorPrioridade(TarefaPrioridade prioridade)
            => _repository.ListarTodos().Where(t => t.Prioridade == prioridade);

        public Tarefa MarcarComoFinalizada(int id)
        {
            var task = _repository.ConsultarPorId(id);
            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            task.MarcarComoFinalizada();
            _repository.Atualizar(task);
            return task;
        }
    }
}
