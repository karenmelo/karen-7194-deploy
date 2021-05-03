
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context, [FromBody] User model)
        {
            var users = await context
                                    .Users
                                    .AsNoTracking()
                                    .ToListAsync();


            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        //[Authorize(Roles="manager")] -> comentado pois se não tiver nenhum usuário cadastrado, logo não haverá manager para acessar e cadastrar.
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            //verifica se os dados são válidos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //Força o usuário a ser sempre "funcionário"
                model.Role = "employee";

                context.Users.Add(model);
                await context.SaveChangesAsync();

                //deixando de exibir a senha quando retornar para a tela
                model.Password = "";
                return model;

            }
            catch (Exception)
            {

                return BadRequest(new { message = "Não foi possível criar o usuário." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put([FromServices] DataContext context, int id, [FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado." });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível alterar o usuário." });
            }

        }



        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
                                    .AsNoTracking()
                                    .Where(x => x.Username == model.Username && x.Password == model.Password)
                                    .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos." });

            var token = TokenSevice.GenerateToken(user);


            //deixando de exibir a senha quando retornar para a tela
            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }



        //exemplo pra testes
        // [HttpGet]
        // [Route("anonimo")]
        // public string Anonimo() => "Anonimo";

        // [HttpGet]
        // [Route("autenticado")]
        // public string Autenticado() => "Autenticado";

        // [HttpGet]
        // [Route("funcionario")]
        // public string Funcionario() => "Funcionario";

        // [HttpGet]
        // [Route("gerente")]
        // public string Gerente() => "Gerente";

    }
}