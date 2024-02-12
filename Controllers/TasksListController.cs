using Microsoft.AspNetCore.Mvc;
using Task = myTodoList.Models.Task;
using myTodoList.Service;
using myTodoList.Interface;
using myTodoList.Models;

namespace myTodoList.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksListController : ControllerBase
{
    ITasksListService TasksListService;
    public int UserId{get;set;}
    public TasksListController(ITasksListService TasksListService,IHttpContextAccessor httpContextAccessor)
    {
        this.TasksListService = TasksListService;
        this.UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value);
    }
    [HttpGet]
    public ActionResult<List<Task>> Get()
    {
        return TasksListService.GetAll(this.UserId);
    }

    [HttpGet("{id}")]
    public ActionResult<Task> Get(int id)
    {
        var task = TasksListService.GetById(id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public ActionResult Post(Task newTask)
    {
        var newId = TasksListService.Add(newTask);

        return CreatedAtAction("Post",
            new { id = newId }, TasksListService.GetById(newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Task newTask)
    {
        var result = TasksListService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var result = TasksListService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }



}
