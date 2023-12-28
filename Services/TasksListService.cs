using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using myToDoList.Interfaces;
using System.IO;
using Task = myToDoList.Models.Task;
using Microsoft.AspNetCore.Hosting;


namespace myToDoList.Services
{
    public class TasksListService:ITasksListService
    {
       List<Task> tasksList{ get; }
       private string fileName = "Tasks.json";
        public TasksListService(IWebHostEnvironment webHost)
        {
                this.fileName = Path.Combine(webHost.ContentRootPath, "Data", "Tasks.json");
                using (var jsonFile = File.OpenText(fileName))
                {
                    tasksList = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                // tasksList = new List<Task>
                // {
                //     new myToDoList.Models.Task{Id = 1, Description = "H.W", IsDone = false}
                // };
            
        }
    
        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(tasksList));
        }
        public List<Task> GetAll()=> tasksList;
        
        public  Task GetById(int id)
        {
            return tasksList.FirstOrDefault(t=>t.Id==id);
        }
        public int Add(Task newTask)
        {
            if (tasksList.Count == 0)
            {
                newTask.Id = 1;
            }
            else
            {
                newTask.Id =  tasksList.Max(t=>t.Id) + 1;
            }
            tasksList.Add(newTask);
            saveToFile();
            return newTask.Id;
        }
        public bool Update(int id, Task newTask)
        {
            if (id != newTask.Id)
                return false;
            var existingTask = GetById(id);
            if (existingTask == null )
                return false;
            var index = tasksList.IndexOf(existingTask);
            if (index == -1 )
                return false;
            tasksList[index] = newTask;
            saveToFile();
            return true;
        }  
        public bool Delete(int id)
        {
            var existingTask = GetById(id);
            if (existingTask == null )
                return false;
            var index = tasksList.IndexOf(existingTask);
            if (index == -1 )
                return false;
            tasksList.RemoveAt(index);
            saveToFile();
            return true;
        }  
    }
    public static class TaskUtils
{
    public static void AddTask(this IServiceCollection services)
    {
        services.AddSingleton<ITasksListService, TasksListService>();
    }
}
}