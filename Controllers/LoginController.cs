using Microsoft.AspNetCore.Mvc;
using myTodoList.Models;
using myTodoList.Service;
using myTodoList.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;


namespace myTodoList.Controllers;
[ApiController]
[Route("[controller]")]

public class LoginController : ControllerBase
{
    IUsersService usersService;

    public LoginController(IUsersService usersService, IHttpContextAccessor httpContextAccessor)
    {
        this.usersService = usersService;
    }
    [HttpPost]
    public ActionResult<String> Login(User user)
    {
        User current = usersService.GetUser(user);
        if(current == null)
            return BadRequest();
        if (current?.UserType == UserType.Manager)
        {
            var claims = new List<Claim>
            {
                new("type", "Admin"),
                new("name",current.Name!),
                new("id",current.Id.ToString())
            };

            var token = UserTokenService.GetToken(claims);
            return new OkObjectResult(UserTokenService.WriteToken(token));
        }
        if (current != null)
        {
            {
                var claims2 = new List<Claim>
            {
                new Claim("type", "User"),
                new Claim("name",current.Name!),
                new Claim("id",current.Id.ToString())
            };

                var token2 = UserTokenService.GetToken(claims2);

                return new OkObjectResult(UserTokenService.WriteToken(token2));
            }
        }
        else
            return BadRequest();
    }
 
}


   