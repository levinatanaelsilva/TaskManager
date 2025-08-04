using System;
using System.Collections.Generic;
using System.Linq;
using GestaoTarefas.Core.Enums;
using GestaoTarefas.Core.Services;

namespace GestaoTarefas.CLI
{
    public class TarefaCLI
    {
        private readonly TarefaService _service;

        public TarefaCLI(TarefaService service)
        {
            _service = service;
        }

        public void Executar()
        {
            Console.WriteLine("Gestão de Tarefas");
            while (true)
            {
                ExibirMenu();
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                try
                {
                    Console.Clear();
                    switch (input.Trim())
                    {
                        case "1": CriarTarefa(); break;
                        case "2": ListarTarefas(); break;
                        case "3": AtualizarTarefa(); break;
                        case "4": ExcluirTarefa(); break;
                        case "5": FiltrarTarefas(); break;
                        case "6": MarcarTarefaComoFinalizada(); break;
                        case "7": Sair(); return;
                        default: Console.WriteLine("Comando inválido."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }

        private static void Sair()
        {
            Console.WriteLine("Encerrando o programa...");
            System.Threading.Thread.Sleep(1000);
        }

        private void ExibirMenu()
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1 - Nova tarefa");
            Console.WriteLine("2 - Listar tarefas");
            Console.WriteLine("3 - Atualizar tarefa");
            Console.WriteLine("4 - Excluir tarefa");
            Console.WriteLine("5 - Filtrar tarefas");
            Console.WriteLine("6 - Marcar tarefa como finalizada");
            Console.WriteLine("7 - Sair");
            Console.Write("> ");
        }

        private void CriarTarefa()
        {
            Console.WriteLine("INFORME OS DADOS PARA NOVA TAREFA:");
            Console.Write("Título: ");
            var titulo = Console.ReadLine();
            Console.Write("Descrição: ");
            var desc = Console.ReadLine();
            Console.Write("Prioridade (1-Baixa, 2-Média, 3-Alta): ");
            TarefaPrioridade prioridade;
            if (!Enum.TryParse<TarefaPrioridade>(Console.ReadLine(), out prioridade))
            {
                Console.WriteLine("Prioridade inválida.");
                return;
            }

            _service.Adicionar(titulo, desc, prioridade);
            Console.WriteLine("Tarefa adicionada.");
        }

        private void ListarTarefas()
        {
            var tasks = _service.ListarTodos();
            ExibirListaTarefas(tasks);
        }

        private static void ExibirListaTarefas(IEnumerable<Core.Entities.Tarefa> tasks)
        {
            Console.WriteLine($"LISTA DE TAREFAS CADASTRADAS - Total de {tasks.Count()} registro(s)");
            Console.WriteLine(" ");

            if (!tasks.Any())
            {
                Console.WriteLine("Nenhuma tarefa cadastrada.");
                return;
            }

            Console.WriteLine("ID | Título | Status | Prioridade");
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            foreach (var task in tasks)
            {
                Console.WriteLine("{0} | {1} | {2} | {3}", task.Id, task.Titulo, task.Status, task.Prioridade);
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
            }
        }

        private void AtualizarTarefa()
        {
            Console.WriteLine("ALTERAÇÃO DE TAREFAS");
            Console.Write("ID da tarefa: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            Console.Write("Novo título: ");
            var titulo = Console.ReadLine();
            Console.Write("Nova descrição: ");
            var desc = Console.ReadLine();
            Console.Write("Prioridade (1-Baixa, 2-Média, 3-Alta): ");
            if (!Enum.TryParse(Console.ReadLine(), out TarefaPrioridade prioridade))
            {
                Console.WriteLine("Prioridade inválida.");
                return;
            }

            Console.Write("Status (1-Pendente, 2-Finalizada): ");
            TarefaStatus status;
            if (!Enum.TryParse<TarefaStatus>(Console.ReadLine(), out status))
            {
                Console.WriteLine("Status inválido.");
                return;
            }

            _service.Atualizar(id, titulo, desc, prioridade, status);
            Console.WriteLine("Tarefa atualizada.");
        }

        private void ExcluirTarefa()
        {
            Console.WriteLine("EXCLUIR TAREFA");
            Console.Write("ID da tarefa: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            _service.Excluir(id);
            Console.WriteLine("Tarefa removida.");
        }

        private void FiltrarTarefas()
        {
            Console.WriteLine("DIGITE OS CRITÉRIOS DA PESQUISA");
            Console.WriteLine("Filtrar por: 1 - Status | 2 - Prioridade");
            Console.Write("> ");
            var option = Console.ReadLine();
            if (option == "1")
            {
                Console.Write("Status (1-Pendente, 2-Finalizada): ");
                if (!Enum.TryParse(Console.ReadLine(), out TarefaStatus status))
                {
                    Console.WriteLine("Status inválido.");
                    return;
                }

                var tasks = _service.FiltrarPorStatus(status);
                ExibirListaTarefas(tasks);
            }
            else if (option == "2")
            {
                Console.Write("Prioridade (1-Baixa, 2-Média, 3-Alta): ");
                if (!Enum.TryParse(Console.ReadLine(), out TarefaPrioridade prioridade))
                {
                    Console.WriteLine("Prioridade inválida.");
                    return;
                }

                var tasks = _service.FiltrarPorPrioridade(prioridade);
                ExibirListaTarefas(tasks);
            }
            else
            {
                Console.WriteLine("Opção inválida.");
            }
        }


        private void MarcarTarefaComoFinalizada()
        {
            Console.WriteLine("MARCAR TAREFA COMO FINALIZADA");
            Console.Write("ID da tarefa: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            _service.MarcarComoFinalizada(id);
            Console.WriteLine("Tarefa atualizada para finalizada.");
        }
    }
}
