using Microsoft.AspNetCore.Mvc;
using myTodoList.Models;
using myTodoList.Service;
using myTodoList.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace myTodoList.Controllers;
[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    IUsersService usersService;
    public int UserId{get;set;}

    public UsersController(IUsersService usersService,IHttpContextAccessor httpContextAccessor)
    {
        this.usersService = usersService;
        this.UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value);
    }

    [Authorize(Policy = "Admin")]
    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return usersService.GetAll();
    }
  
    [Authorize(Policy = "User")]
    [Route("[action]")]
    [HttpGet]
    public ActionResult<User> GetUser()
    {
        return usersService.GetById(this.UserId);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = usersService.Add(newUser);

        return CreatedAtAction("Post",
            new { id = newId }, usersService.GetById(newId));
    }

    [Authorize(Policy ="User")]
    [HttpPut("{id}")]
    public ActionResult Put(int id, User newUser)
    {
        var result = usersService.Update(id, newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var result = usersService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

}

