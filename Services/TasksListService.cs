using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myToDoList.Models;
using myToDoList.Services;

namespace myToDoList.Services
{
    public static class TasksListService
    {
     private static List<myToDoList.Models.Task> tasksList ;
     static TasksListService()
    {
        tasksList = new List<myToDoList.Models.Task>
        {
            new myToDoList.Models.Task { Id = 1, Description = "Wash the dishes", IsDone = false},
            new myToDoList.Models.Task { Id = 2, Description = "Do home work", IsDone = false},
            new myToDoList.Models.Task { Id = 3, Description = "Do Angular project", IsDone = true}
        };
    }
     public static List<myToDoList.Models.Task> GetAll()=> tasksList;
     
     public static myToDoList.Models.Task GetById(int id)
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
        return true;
    }  
     

    }
}