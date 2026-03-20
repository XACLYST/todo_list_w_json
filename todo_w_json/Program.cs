using System;
using System.Text;
using System.Text.Json;

namespace TodoList
{
    class Program
    {
        class Todo
        {
            public string id { get; set; }
            public string description { get; set; }
            public bool isCompleted { get; set; }
            public DateTime date { get; set; }

            public Todo() { }

            public Todo(string Description, DateTime Date)
            {
                id = Guid.NewGuid().ToString();
                description = Description;
                isCompleted = false;
                date = Date;
            }
        }

        static void Main(string[] args)
        {
            //Absolute path to the "data.json" file
            const string path = "D:/CSprojects/todo_w_json/todo_w_json/data.json";
            List<Todo> list = GetTasks(path);

            Console.WriteLine("Hello, please, choose the option what you would like to do: ");
        Prog:
            //Adding a 1 sec timer just to separate the info on the screen
            Thread.Sleep(1000);

            Console.WriteLine("1 - Create new task;\r\n2 - Show all tasks;\r\n3 - Mark as done\r\n4 - Delete task;\r\n5 - Exit");
            string option = Console.ReadLine().Trim();

            //There we operate over the user's input
            switch (option.Trim())
            {
                case "1":
                    Console.WriteLine("Please, write down the description of a task: ");
                    string description = Console.ReadLine();

                    if (!string.IsNullOrEmpty(description))
                    {
                        Todo task = new Todo(description, DateTime.Now);
                        list.Add(task);
                    }
                    else
                    {
                        Console.WriteLine("Description cannot be empty!");
                    }

                    RewriteFile(list, path);

                    goto Prog;

                case "2":
                    ShowAllTasks(list);
                    goto Prog;

                case "3":
                    ShowAllTasks(list);
                    Console.WriteLine("Please, copy the id of a task you want to mark as \"completed\" and just paste it here");
                    string id = Console.ReadLine().Trim();

                    if (!string.IsNullOrEmpty(id))
                    {
                        MarkAsComplete(list, id);
                        RewriteFile(list, path);
                    }
                    else
                    {
                        Console.WriteLine("You must've written the id here");
                    }

                    goto Prog;

                case "4":
                    ShowAllTasks(list);
                    Console.WriteLine("Please, copy the id of a task you want to delete and just paste it here");
                    string id_to_delete = Console.ReadLine().Trim();

                    if (!String.IsNullOrEmpty(id_to_delete))
                    {
                        DeleteTask(list, id_to_delete);
                        RewriteFile(list, path);
                    }
                    else
                    {
                        Console.WriteLine("You must've written the id here");
                    }

                    goto Prog;

                case "5":
                    break;

                default:
                    Console.WriteLine("You must've written numbers from 1 to 5 to choose one of the options!");
                    goto Prog;
            }
        }

        //This method must return a List<Todo> of all the data in JSON file
        static List<Todo> GetTasks(string path)
        {
            string contents = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(contents))
            {
                return JsonSerializer.Deserialize<List<Todo>>(contents) ?? new List<Todo>();
            }
            else
            {
                return new List<Todo>();
            }
        }

        //This method adds info in the data file
        static void RewriteFile(List<Todo> tasks, string path)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(path, jsonString);
            }
            catch
            {
                Console.WriteLine("Something went wrong while writing in the file");
            }
        }

        //This method shows all the data to the user
        static void ShowAllTasks(List<Todo> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Todo task = tasks[i];
                string completion = "";
                if (task.isCompleted)
                {
                    completion = "completed";
                }
                else
                {
                    completion = "not completed yet";
                }

                Console.WriteLine("======================");
                Console.WriteLine("id: " + task.id + "");
                Console.WriteLine("description: " + task.description + "");
                Console.WriteLine("completion: " + completion + "");
                Console.WriteLine("date/time: " + task.date.ToString() + "");
                Console.WriteLine("======================\r\n");
            }
        }

        //This method changes the fiels "isCompleted" ONLY in the !!!LIST OF TASKS!!!
        //To change data in the file you will need to use the RewriteFile() method once more
        static void MarkAsComplete(List<Todo> tasks, string Id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Todo task = tasks[i];
                if (task.id == Id)
                {
                    task.isCompleted = true;
                }
                else
                {
                    continue;
                }
            }
        }

        //This method deletes the fields with the id the user chooses
        //It changes !!!ONLY THE LIST OF TASKS!!!
        //To change data in the file use the RewriteFile() method
        static void DeleteTask(List<Todo> tasks, string Id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Todo task = tasks[i];
                if (task.id == Id)
                {
                    tasks.RemoveAt(i);
                }
                else
                {
                    continue;
                }
            }
        }
    }
}