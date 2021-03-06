using System.Threading.Tasks;
using AuthJwt.Models;
using AuthJwt.Repositories;
using AuthJwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwt.Controllers
{   
    [Route("v1/Account")]
    public class HomeController : Controller
    {

      [HttpPost]
      [Route("login")]
      [AllowAnonymous]
      public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
      {
        var user = UserRepository.Get(model.Username, model.Password);

        if(user == null)
          return NotFound(new {message = "Usuário ou senha inválidos"});

        var token = TokenService.GenerateToken(user);
        user.Password = "";

        return new 
        {
          user = user,
          token = token
        };
      }

      [HttpGet]
      [Route("anonymous")]
      [AllowAnonymous]
      public string Anonymous() => "Anônimo";

      [HttpGet]
      [Route("authencated")]
      [Authorize]
      public string Authenticated() => string.Format("Autenticado - {0}", User.Identity.Name);

      [HttpGet]
      [Route("employee")]
      [Authorize(Roles = "employee, manager")]
      public string Employee() => "Funcionários";

      [HttpGet]
      [Route("manager")]
      [Authorize(Roles = "manager")]
      public string Manager() => "Gerentes";
    }
}