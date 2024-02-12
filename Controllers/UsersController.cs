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
    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [Authorize(Policy = "Admin")]
    [HttpGet]


    public ActionResult<List<User>> Get()
    {
        return usersService.GetAll();
    }
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var task = usersService.GetById(id);
        if (task == null)
            return NotFound();
        return task;
    }
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = usersService.Add(newUser);

        return CreatedAtAction("Post",
            new { id = newId }, usersService.GetById(newId));
    }

    [Authorize(Policy = "Admin")]
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
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    IUsersService usersService;

    public LoginController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost]
    // [Route("[action]")]
    public ActionResult<String> Login(User User)
    {
        if (usersService.IsAdmin(User))
        {
            var claims = new List<Claim>
            {
                new Claim("type", "Admin"),
                new Claim("name",User.Name),
                new Claim("id",User.Id.ToString())
            };

            var token = UserTokenService.GetToken(claims);

            return new OkObjectResult(UserTokenService.WriteToken(token));
        }
        User current = usersService.GetAll().FirstOrDefault(u => u.Id == User.Id);
        if (current != null)
        {
            {
                var claims2 = new List<Claim>
            {
                new Claim("type", "User"),
                new Claim("name",User.Name),
                new Claim("id",User.Id.ToString())
            };

                var token2 = UserTokenService.GetToken(claims2);

                return new OkObjectResult(UserTokenService.WriteToken(token2));
            }
        }
        else 
            return BadRequest();
    }
}







