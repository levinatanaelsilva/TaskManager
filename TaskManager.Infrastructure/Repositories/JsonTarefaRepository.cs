using GestaoTarefas.Core.Entities;
using GestaoTarefas.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GestaoTarefas.Infra.Repositories
{
    public class JsonTarefaRepository : ITarefaRepository
    {
        private readonly string _caminhoArquivo;
        private List<Tarefa> _tarefas;
        private static readonly object _bloqueado = new object();

        public JsonTarefaRepository(string caminhoArquivo)
        {
            _caminhoArquivo = caminhoArquivo;
            _tarefas = CarregarTarefas();
        }

        private List<Tarefa> CarregarTarefas()
        {
            if (!File.Exists(_caminhoArquivo)) return new List<Tarefa>();

            try
            {
                var json = File.ReadAllText(_caminhoArquivo);
                return JsonConvert.DeserializeObject<List<Tarefa>>(json) ?? new List<Tarefa>();
            }
            catch
            {
                return new List<Tarefa>();
            }
        }

        private void SalvarTarefas()
        {
            lock (_bloqueado)
            {
                var json = JsonConvert.SerializeObject(_tarefas, Formatting.Indented);
                File.WriteAllText(_caminhoArquivo, json);
            }
        }

        public IEnumerable<Tarefa> ListarTodos() => _tarefas;

        public Tarefa ConsultarPorId(int id) => _tarefas.FirstOrDefault(t => t.Id == id);

        public void Adicionar(Tarefa task)
        {
            _tarefas.Add(task);
            SalvarTarefas();
        }

        public void Atualizar(Tarefa task)
        {
            var index = _tarefas.FindIndex(t => t.Id == task.Id);
            if (index >= 0)
            {
                _tarefas[index] = task;
                SalvarTarefas();
            }
        }

        public void Excluir(int id)
        {
            var task = ConsultarPorId(id);
            if (task != null)
            {
                _tarefas.Remove(task);
                SalvarTarefas();
            }
        }

        public int GerarProximoId() => _tarefas.Any() ? _tarefas.Max(t => t.Id) + 1 : 1;
    }
}
