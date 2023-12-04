namespace API.Controllers;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Data;

[ApiController]
[Route("api/categoria")]
public class CategoriaController : ControllerBase
{
    private readonly AppDataContext _context;

    public CategoriaController(AppDataContext context)
    {
        _context = context;
    }

    // GET: api/categoria/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Categoria> categorias = _context.Categorias.ToList();
            return Ok(categorias);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/categoria/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Categoria categoria)
    {
        try
        {
            _context.Add(categoria);
            _context.SaveChanges();
            return Created("", categoria);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

