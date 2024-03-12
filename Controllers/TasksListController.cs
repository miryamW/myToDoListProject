using Microsoft.AspNetCore.Mvc;
using Task = myTodoList.Models.Task;
using myTodoList.Service;
using myTodoList.Interface;
using myTodoList.Models;
using Microsoft.AspNetCore.Authorization;

namespace myTodoList.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksListController : ControllerBase
{
    ITasksListService TasksListService;
    public int UserId {get;set;}
    public TasksListController(ITasksListService TasksListService,IHttpContextAccessor httpContextAccessor)
    {
        this.TasksListService = TasksListService;
        this.UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value);
    }

    [Authorize(Policy ="User")]
    [HttpGet]
    public ActionResult<List<Task>> Get()
    {
        return TasksListService.GetAll(this.UserId);
    }

    [Authorize(Policy ="User")]
    [HttpGet("{id}")]
    public ActionResult<Task> Get(int id)
    {
        var task = TasksListService.GetById(this.UserId,id);
        if (task == null)
            return NotFound();
        return task;
    }

    [Authorize(Policy ="User")]
    [HttpPost]
    public ActionResult Post(Task newTask)
    {
        newTask.UserId = this.UserId;
        var newId = TasksListService.Add(newTask);

        return CreatedAtAction("Post",
            new { id = newId }, TasksListService.GetById(this.UserId,newId));
    }

    [Authorize(Policy ="User")]
    [HttpPut("{id}")]
    public ActionResult Put(int id, Task newTask)
    {
        newTask.UserId = this.UserId;
        var result = TasksListService.Update(this.UserId,id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
   
    [Authorize(Policy ="User")]
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var result = TasksListService.Delete(this.UserId,id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }



}
