using GestaoTarefas.CLI;
using GestaoTarefas.Core.Services;
using GestaoTarefas.Infra.Repositories;

namespace GestaoTarefas
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new JsonTarefaRepository("tasks.json");
            var service = new TarefaService(repository);
            var cli = new TarefaCLI(service);
            cli.Executar();
        }
    }
}