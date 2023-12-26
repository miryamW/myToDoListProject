using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myToDoList.Models;
using myToDoList.Interfaces;

namespace myToDoList.Services
{
    public class TasksListService:ITasksListService;
    {
       List<myToDoList.Models.Task> tasksList{ get; }
       private string fileName = "Tasks.json";

        public TasksListService()
        {
            public PizzaService(IWebHostEnvinronment webHost)
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
            }
        }
    
        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Task));
        }
        public  List<myToDoList.Models.Task> GetAll()=> tasksList;
        
        public  myToDoList.Models.Task GetById(int id)
        {
            return tasksList.FirstOrDefault(t=>t.Id==id);
        }
        public static int Add(myToDoList.Models.Task newTask)
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
        public static bool Update(int id, myToDoList.Models.Task newTask)
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
        public static bool Delete(int id)
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