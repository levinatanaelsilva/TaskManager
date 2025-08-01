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
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");                    
                }
                first = false;

                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("Comandos: nova, listar, atualizar, excluir, filtrar, sair");
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var command = input.Trim().ToLower();

                try
                {
                    if (command == "sair") break;
                    else if (command == "nova")
                    {
                        Console.Write("Título: ");
                        var title = Console.ReadLine();
                        Console.Write("Descrição: ");
                        var desc = Console.ReadLine();
                        Console.Write("Prioridade (1-Baixa, 2-Média, 3-Alta): ");
                        var priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), Console.ReadLine(), true);

                        service.Create(title, desc, priority);
                        Console.WriteLine("Tarefa adicionada.");
                    }
                    else if (command == "listar")
                    {
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
                    }
                    else if (command == "atualizar")
                    {
                        Console.Write("ID da tarefa: ");
                        var id = Guid.Parse(Console.ReadLine());
                        Console.Write("Novo título: ");
                        var title = Console.ReadLine();
                        Console.Write("Nova descrição: ");
                        var desc = Console.ReadLine();
                        Console.Write("Prioridade (1-Baixa, 2-Media, 3-Alta): ");
                        var priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), Console.ReadLine(), true);
                        Console.Write("Status (1-Pendente, 2-Iniciada, 3-Finalizada): ");
                        var status = (TaskStatus)Enum.Parse(typeof(TaskStatus), Console.ReadLine(), true);

                        service.Update(id, title, desc, priority, status);
                        Console.WriteLine("Tarefa atualizada.");
                    }
                    else if (command == "excluir")
                    {
                        Console.Write("ID da tarefa: ");
                        var id = Guid.Parse(Console.ReadLine());
                        service.Delete(id);
                        Console.WriteLine("Tarefa removida.");
                    }
                    else if (command == "filtrar")
                    {
                        Console.Write("Filtrar por (1-Status/2-Prioridade): ");
                        var type = Console.ReadLine();
                        if (type == "1")
                        {
                            Console.Write("Status (Pending, InProgress, Completed): ");
                            var status = (TaskStatus)Enum.Parse(typeof(TaskStatus), Console.ReadLine(), true);
                            foreach (var task in service.FilterByStatus(status))
                                Console.WriteLine($"[{task.Id}] {task.Title} - {task.Status} - {task.Priority}");
                        }
                        else if (type == "2")
                        {
                            Console.Write("Prioridade (Low, Medium, High): ");
                            var priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), Console.ReadLine(), true);
                            foreach (var task in service.FilterByPriority(priority))
                                Console.WriteLine($"[{task.Id}] {task.Title} - {task.Status} - {task.Priority}");
                        }
                        else
                        {
                            Console.WriteLine("Comando não reconhecido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Comando não reconhecido.");
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