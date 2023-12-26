using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myToDoList.Models;
using myToDoList.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace myToDoList.Services
{
    public class TasksListService:ITasksListService
    {
       List<myToDoList.Models.Task> tasksList{ get; }
       private string fileName = "Tasks.json";

        public TasksListService(IWebHostEnvinronment webHost)
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
                // tasksList = new List<myToDoList.Models.Task>
                // {
                //     new myToDoList.Models.Task{Id = 1, Description = "H.W", IsDone = false}
                // };
            
        }
    
        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Task));
        }
        public List<myToDoList.Models.Task> GetAll()=> tasksList;
        
        public  myToDoList.Models.Task GetById(int id)
        {
            return tasksList.FirstOrDefault(t=>t.Id==id);
        }
        public int Add(myToDoList.Models.Task newTask)
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
        public bool Update(int id, myToDoList.Models.Task newTask)
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