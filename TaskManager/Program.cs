using System;
using System.Linq;
using TaskManager.Core.Enums;
using TaskManager.Core.Services;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new JsonTaskRepository("tasks.json");
            var service = new TaskService(repository);

            Console.WriteLine("Task Manager CLI");
            var first = true;
            while (true)
            {
                if (!first)
                {
                    Console.WriteLine("\n\n");
                }
                first = false;

                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("Comandos:");
                Console.WriteLine("1 - Nova");
                Console.WriteLine("2 - Listar");
                Console.WriteLine("3 - Atualizar");
                Console.WriteLine("4 - Excluir");
                Console.WriteLine("5 - Filtrar");
                Console.WriteLine("6 - Sair");
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                try
                {
                    switch (input.Trim())
                    {
                        case "6":
                            Console.WriteLine("Encerrando o programa...");
                            System.Threading.Thread.Sleep(1000);
                            return;

                        case "1":
                            Console.Write("Título: ");
                            var title = Console.ReadLine();
                            Console.Write("Descrição: ");
                            var desc = Console.ReadLine();
                            Console.Write("Prioridade (1-Baixa, 2-Média, 3-Alta): ");
                            var priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), Console.ReadLine(), true);

                            service.Create(title, desc, priority);
                            Console.WriteLine("Tarefa adicionada.");
                            break;

                        case "2":
                            var listAll = service.GetAll();
                            if (listAll.Any())
                            {
                                Console.WriteLine("ID\t\t\t\t\tTítulo\tStatus\tPrioridade");
                                foreach (var task in listAll)
                                {
                                    Console.WriteLine($"[{task.Id}]\t{task.Title}\t{task.Status}\t{task.Priority}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("=============== Nenhuma tarefa cadastrada ===============");
                            }
                            break;

                        case "3":
                            Console.Write("ID da tarefa: ");
                            var idUpdate = Guid.Parse(Console.ReadLine());
                            Console.Write("Novo título: ");
                            var newTitle = Console.ReadLine();
                            Console.Write("Nova descrição: ");
                            var newDesc = Console.ReadLine();
                            Console.Write("Prioridade (1-Baixa, 2-Média, 3-Alta): ");
                            var newPriority = (TaskPriority)Enum.Parse(typeof(TaskPriority), Console.ReadLine(), true);
                            Console.Write("Status (1-Pendente, 2-Iniciada, 3-Finalizada): ");
                            var newStatus = (TaskStatus)Enum.Parse(typeof(TaskStatus), Console.ReadLine(), true);

                            service.Update(idUpdate, newTitle, newDesc, newPriority, newStatus);
                            Console.WriteLine("Tarefa atualizada.");
                            break;

                        case "4":
                            Console.Write("ID da tarefa: ");
                            var idDelete = Guid.Parse(Console.ReadLine());
                            service.Delete(idDelete);
                            Console.WriteLine("Tarefa removida.");
                            break;

                        case "5":
                            Console.Write("Filtrar por (1-Status / 2-Prioridade): ");
                            var type = Console.ReadLine();
                            if (type == "1")
                            {
                                Console.Write("Status (Pendente, Iniciada, Finalizada): ");
                                var status = (TaskStatus)Enum.Parse(typeof(TaskStatus), Console.ReadLine(), true);
                                foreach (var task in service.FilterByStatus(status))
                                    Console.WriteLine($"[{task.Id}] {task.Title} - {task.Status} - {task.Priority}");
                            }
                            else if (type == "2")
                            {
                                Console.Write("Prioridade (Baixa, Media, Alta): ");
                                var priorityFilter = (TaskPriority)Enum.Parse(typeof(TaskPriority), Console.ReadLine(), true);
                                foreach (var task in service.FilterByPriority(priorityFilter))
                                    Console.WriteLine($"[{task.Id}] {task.Title} - {task.Status} - {task.Priority}");
                            }
                            else
                            {
                                Console.WriteLine("Comando não reconhecido.");
                            }
                            break;

                        default:
                            Console.WriteLine("Comando não reconhecido.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }
    }
}