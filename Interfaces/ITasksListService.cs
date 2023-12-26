using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myToDoList.Models;
using myToDoList.Services;

namespace myToDoList.Interfaces
{
    public static class ITasksListService
    {
     public static List<myToDoList.Models.Task> GetAll();
     
     public static myToDoList.Models.Task GetById(int id);
     
     public static int Add(myToDoList.Models.Task newTask);

     public static bool Update(int id, myToDoList.Models.Task newTask);
   
     public static bool Delete(int id);
    }
}