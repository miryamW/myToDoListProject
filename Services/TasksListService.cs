using Task =myTodoList.Models.Task;
using myTodoList.Interface;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;

namespace myTodoList.Service;


public class TasksListService: ITasksListService
{
    // private List<Task> tasks;

    // public TasksListService()
    // {
    //     tasks = new List<Task>{
    //         new Task {Id=1 , Name="aaa" , IsDo=true},
    //         new Task {Id=2 , Name="bbb" , IsDo=false},
    //         new Task {Id=3 , Name="ccc" , IsDo=true}
    //     };
    // }

        private List<Task> tasks;
        private string fileName = "Task.json";
        public TasksListService()
        {
            this.fileName = Path.Combine(/*webHost.ContentRootPath,*/ "Data", "Task.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(tasks));
        }

    public List<Task> GetAll() => tasks;

    public Task GetById(int id) 
    {
        return tasks.FirstOrDefault(p => p.Id == id);
    }

    public int Add(Task newTask)
    {
        if (tasks.Count == 0)

            {
                newTask.Id = 1;
            }
            else
            {
        newTask.Id =  tasks.Max(p => p.Id) + 1;

            }

        tasks.Add(newTask);
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

        var index = tasks.IndexOf(existingTask);
        if (index == -1 )
            return false;

        tasks[index] = newTask;
        saveToFile();
        return true;
    }  

      
    public bool Delete(int id)
    {
        var existingTask = GetById(id);
        if (existingTask == null )
            return false;

        var index = tasks.IndexOf(existingTask);
        if (index == -1 )
            return false;

        tasks.RemoveAt(index);
        saveToFile();
        return true;
    }  

    

}

public static class TaskSListUtil
    {
        public static void AddTask(this IServiceCollection services)
        {
            services.AddSingleton<ITasksListService, TasksListService>();
        }
    }
