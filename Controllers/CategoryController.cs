using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("v1/categories")]
public class CategoryController : ControllerBase
{

    //endpoint => url
    //produção sempre https://
    //https://localhost:5001
    //http://localhost:5000
    //azure: https://meuapp.azurewebsites.net/
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] //se colocar o .AddResponseCaching() na Startup e não
    //quiser que um método seja "cacheado" é necessário incluir a linha acima para não "cachear" o método em questão
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
        try
        {
            //AsNoTracking é pra desativar todo o proxy que o EFCore carrega junto quando faz uma busca
            //Se for preciso realizar um filtro(where), deverá ser feito antes do ToList, pois ele vai ao banco
            //E se o filtro estiver após ele irá primeiro no banco trazer todas as categorias e depois fará
            //O filtro em memória o que irá causar um gargalo
            var category = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(category);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpGet]
    [Route("{id:int}")] //o :int restringe a rota apenas inteiros
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
    {
        try
        {
            //AsNoTracking é pra desativar todo o proxy que o EFCore carrega junto quando faz uma busca
            //Se for preciso realizar um filtro(where), deverá ser feito antes do ToList, pois ele vai ao banco
            //E se o filtro estiver após ele irá primeiro no banco trazer todas as categorias e depois fará
            //O filtro em memória o que irá causar um gargalo
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Post([FromBody] Category model, [FromServices] DataContext context)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Categories.Add(model);

            //await: aguardar até que as mudanças sejam salvas
            await context.SaveChangesAsync();

            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível criar a categoria." });
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
    {
        try
        {
            //Verifica se o ID informado é o mesmo do modelo
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });

            // //Verifica se os dados são válidos
            // if (model.Id == id)
            //     return Ok(model);

            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Este registro já foi atualizado." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível atualizar a categoria." });
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada." });

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(new { message = "Categoria removida com sucesso." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível remover a categoria." });
        }

    }
}