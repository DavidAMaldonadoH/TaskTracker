using System.Text.Json;

internal class Program
{
    private static List<MyTask> tasks = new List<MyTask>();
    private static void Main(string[] args)
    {
        Console.WriteLine("Welcome to your Task Tracker App");
        Console.WriteLine("To save any changes input the 'end' command");
        LoadTasks();

        while (true)
        {
            try
            {
                var input = Console.ReadLine();

                if (input == null)
                {
                    continue;
                }

                if (input.Trim().Equals("end", StringComparison.OrdinalIgnoreCase))
                {
                    using (StreamWriter outputFile = new StreamWriter("tasks.json"))
                    {
                        outputFile.Write(JsonSerializer.Serialize(tasks));
                    }
                    Console.WriteLine("See you later!");
                    break;
                }

                var arguments = input.Trim().Split(" ");

                var action = arguments[0];

                switch (action)
                {
                    case "add":
                        var id = tasks.Count == 0 ? 0 : tasks.Last().Id + 1;
                        var newTask = new MyTask(id, arguments[1]);
                        tasks.Add(newTask);
                        Console.WriteLine("Task added successfully!");
                        break;
                    case "update":
                        var taskToUpdate = GetTask(arguments[1]);
                        taskToUpdate.SetDescription(arguments[2]);
                        Console.WriteLine("Task updated successfully!");
                        break;
                    case "delete":
                        var taskToDelete = GetTask(arguments[1]);
                        tasks.Remove(taskToDelete);
                        Console.WriteLine("Task deleted successfully!");
                        break;
                    case "mark-in-progress":
                        var taskToInProgress = GetTask(arguments[1]);
                        taskToInProgress.SetStatus(MyTaskStatus.IN_PROGRESS);
                        Console.WriteLine("Task marked as in progress successfully!");
                        break;
                    case "mark-done":
                        var taskToDone = GetTask(arguments[1]);
                        taskToDone.SetStatus(MyTaskStatus.DONE);
                        Console.WriteLine("Task marked as done successfully!");
                        break;
                    case "list":
                        var filterString = arguments.Length == 2 ? arguments[1] : "";
                        MyTaskStatus filterStatus = MyTaskStatus.TODO;
                        if (!String.IsNullOrEmpty(filterString))
                        {
                            if (filterString.Trim().Equals("done", StringComparison.OrdinalIgnoreCase))
                            {
                                filterStatus = MyTaskStatus.DONE;
                            }
                            else if (filterString.Trim().Equals("in-progress", StringComparison.OrdinalIgnoreCase))
                            {
                                filterStatus = MyTaskStatus.IN_PROGRESS;
                            }
                            else if (filterString.Trim().Equals("todo", StringComparison.OrdinalIgnoreCase))
                            {
                                filterStatus = MyTaskStatus.TODO;
                            }
                            else
                            {
                                throw new Exception($"The tasks can't be in the given status: {filterString}");
                            }
                        }
                        var filteredTasks = !String.IsNullOrEmpty(filterString) ? tasks.Where(task => task.Status == filterStatus).ToList() : tasks;

                        foreach (var task in filteredTasks)
                        {
                            Console.WriteLine(task);
                        }
                        break;
                    default:
                        Console.WriteLine("The input command is not a valid command!");
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message == null ? ex.Message : "Oops an error has happened!");
            }
        }
    }

    private static MyTask GetTask(string id)
    {
        if (int.TryParse(id, out int taskId))
        {
            var task = tasks.Find(task => task.Id == taskId) ?? throw new Exception($"There is no task with the given input id: {taskId}");
            return task;
        }
        else
        {
            throw new Exception($"The first argument in the command is not a number: {id}");
        }
    }

    private static void LoadTasks()
    {
        try
        {
            if (File.Exists("tasks.json"))
            {
                using (StreamReader inputFile = new StreamReader("tasks.json"))
                {
                    var jsonContent = inputFile.ReadToEnd();
                    tasks = JsonSerializer.Deserialize<List<MyTask>>(jsonContent) ?? new List<MyTask>();
                    Console.WriteLine("Tasks loaded successfully!");
                }
            }
        }
        catch (System.Exception)
        {
            Console.WriteLine("An error occurred while loading your saved tasks!");
        }
    }
}