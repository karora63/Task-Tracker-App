using System;
using System.Collections.Generic;
using System.IO;

namespace TaskTracker
{
    class Program
    {
        static string filePath = "tasks.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Task Tracker!");

            // Load existing tasks from file
            List<Task> tasks = LoadTasks();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Task Tracker Menu ---");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View Tasks");
                Console.WriteLine("3. Mark Task as Complete");
                Console.WriteLine("4. Remove Task");
                Console.WriteLine("5. Exit");

                Console.Write("Select an option (1-5): ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddTask(tasks);
                        break;
                    case "2":
                        ViewTasks(tasks);
                        break;
                    case "3":
                        MarkTaskComplete(tasks);
                        break;
                    case "4":
                        RemoveTask(tasks);
                        break;
                    case "5":
                        running = false;
                        SaveTasks(tasks);
                        Console.WriteLine("Tasks saved. Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddTask(List<Task> tasks)
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            Console.Write("Enter deadline (yyyy-mm-dd): ");
            DateTime deadline;
            while (!DateTime.TryParse(Console.ReadLine(), out deadline))
            {
                Console.WriteLine("Invalid date. Please enter the deadline in the format yyyy-mm-dd.");
            }

            Task newTask = new Task(description, deadline);
            tasks.Add(newTask);
            Console.WriteLine("Task added successfully!");
        }

        static void ViewTasks(List<Task> tasks)
        {
            Console.WriteLine("\n--- Current Tasks ---");
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                }
            }
        }

        static void MarkTaskComplete(List<Task> tasks)
        {
            ViewTasks(tasks);
            if (tasks.Count == 0) return;

            Console.Write("Enter the task number to mark complete: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks[taskNumber - 1].IsComplete = true;
                Console.WriteLine("Task marked as complete!");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
        }

        static void RemoveTask(List<Task> tasks)
        {
            ViewTasks(tasks);
            if (tasks.Count == 0) return;

            Console.Write("Enter the task number to remove: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks.RemoveAt(taskNumber - 1);
                Console.WriteLine("Task removed successfully!");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
        }

        static List<Task> LoadTasks()
        {
            List<Task> tasks = new List<Task>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 3 && DateTime.TryParse(parts[1], out DateTime deadline))
                    {
                        Task task = new Task(parts[0], deadline, bool.Parse(parts[2]));
                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        static void SaveTasks(List<Task> tasks)
        {
            List<string> lines = new List<string>();

            foreach (Task task in tasks)
            {
                lines.Add($"{task.Description}|{task.Deadline}|{task.IsComplete}");
            }

            File.WriteAllLines(filePath, lines);
        }
    }

    class Task
    {
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsComplete { get; set; }

        public Task(string description, DateTime deadline, bool isComplete = false)
        {
            Description = description;
            Deadline = deadline;
            IsComplete = isComplete;
        }

        public override string ToString()
        {
            return $"{Description} | Deadline: {Deadline.ToShortDateString()} | Complete: {(IsComplete ? "Yes" : "No")}";
        }
    }
}
