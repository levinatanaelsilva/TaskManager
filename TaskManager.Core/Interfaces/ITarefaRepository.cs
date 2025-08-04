using System;
using System.Collections.Generic;
using GestaoTarefas.Core.Entities;

namespace GestaoTarefas.Core.Interfaces
{
    public interface ITarefaRepository
    {
        IEnumerable<Tarefa> ListarTodos();
        Tarefa ConsultarPorId(int id);
        void Adicionar(Tarefa task);
        void Atualizar(Tarefa task);
        void Excluir(int id);
        int GerarProximoId();
    }
}