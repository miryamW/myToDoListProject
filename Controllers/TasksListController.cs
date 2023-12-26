using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myToDoList.Models;
using myToDoList.Interfaces;

namespace myToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskListController : ControllerBase
    {
        ITasksListService TasksListService;
          public TaskListController(ITasksListService TasksListService)
        {
            this.TasksListService = TasksListService;
        }
        [HttpGet]
        public ActionResult<List<myToDoList.Models.Task>> Get()
        {
           return TasksListService.GetAll();
        }
        [HttpGet("{id}")]
         public ActionResult<myToDoList.Models.Task> Get(int id)
        {
            var task = TasksListService.GetById(id);
            if(task==null)
              return NotFound();
            return task;   
        }
        [HttpPost]
          public ActionResult<int> Post(myToDoList.Models.Task task)
        {
            int newId =  TasksListService.Add(task);
            return CreatedAtAction("Post", 
            new {id = newId}, TasksListService.GetById(newId)); 
        }
        [HttpPut]
        public ActionResult<myToDoList.Models.Task> Put(int id,myToDoList.Models.Task task)
        {
             var result = TasksListService.Update(id, task);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent(); 
        }

    }
}
