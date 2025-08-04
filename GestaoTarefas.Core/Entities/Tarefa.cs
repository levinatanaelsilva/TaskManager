using System;
using GestaoTarefas.Core.Enums;

namespace GestaoTarefas.Core.Entities
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public TarefaPrioridade Prioridade { get; set; }
        public TarefaStatus Status { get; set; }
        public DateTime CriadaEm { get; set; }
        public DateTime? FinalizadaEm { get; set; }

        public Tarefa() { }

        public Tarefa(string titulo, string descricao, TarefaPrioridade prioridade)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia.");

            Titulo = titulo;
            Descricao = descricao;
            Prioridade = prioridade;
            Status = TarefaStatus.Pendente;
            CriadaEm = DateTime.Now;
        }

        public void Atualizar(string titulo, string descricao, TarefaPrioridade prioridade, TarefaStatus status)
        {
            Titulo = titulo;
            Descricao = descricao;
            Prioridade = prioridade;
            Status = status;

            if (status == TarefaStatus.Finalizada)
                FinalizadaEm = DateTime.Now;
        }

        public void MarcarComoFinalizada()
        {
            Status = TarefaStatus.Finalizada;
            FinalizadaEm = DateTime.Now;
        }
    }
}
