using Microsoft.AspNetCore.Mvc;
using myTodoList.Models;
using myTodoList.Service;
using myTodoList.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using  System.IdentityModel.Tokens.Jwt;



namespace myTodoList.Controllers;

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
    public ActionResult<String> Login(User User)
    {
        if (usersService.IsAdmin(User))
        {       var claims = new List<Claim>
            {
                new Claim("type", "Admin"),
                new Claim("name",User.Name),
                new Claim("id",usersService.GetAdminId().ToString())
            };

            var token = UserTokenService.GetToken(claims);
            return new OkObjectResult(UserTokenService.WriteToken(token));
        }
        User current = usersService.GetAll().FirstOrDefault(u => u.Name == User.Name&&u.Password==User.Password);
        if (current != null)
        {
            {
                var claims2 = new List<Claim>
            {
                new Claim("type", "User"),
                new Claim("name",User.Name),
                new Claim("id",usersService.GetUser(User).Id.ToString())
            };

                var token2 = UserTokenService.GetToken(claims2);

                return new OkObjectResult(UserTokenService.WriteToken(token2));
            }
        }
        else 
            return BadRequest();
    }
  

}







