
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Task =myToDoList.Models.Task;
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
        public ActionResult<List<Task>> Get()
        {
           return TasksListService.GetAll();
        }
        [HttpGet("{id}")]
         public ActionResult<Task> Get(int id)
        {
            var task = TasksListService.GetById(id);
            if(task==null)
              return NotFound();
            return task;   
        }
        [HttpPost]
          public ActionResult<int> Post(Task task)
        {
            int newId =  TasksListService.Add(task);
            return CreatedAtAction("Post", 
            new {id = newId}, TasksListService.GetById(newId)); 
        }
        [HttpPut]
        public ActionResult<Task> Put(int id, Task task)
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
