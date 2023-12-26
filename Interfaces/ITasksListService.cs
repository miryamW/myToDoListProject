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
    public  interface ITasksListService
    {
       List<myToDoList.Models.Task> GetAll();
     
       myToDoList.Models.Task GetById(int id);
     
       int Add(myToDoList.Models.Task newTask);

       bool Update(int id, myToDoList.Models.Task newTask);
   
       bool Delete(int id);
      
    }
}