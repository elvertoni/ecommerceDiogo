﻿using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("api/produto")]
public class ProdutoController : ControllerBase
{
    private readonly AppDataContext _ctx;
    public ProdutoController(AppDataContext ctx)
    {
        _ctx = ctx;
    }

    //GET: api/produto/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Produto> produtos =
                _ctx.Produtos
                .Include(x => x.Categoria)
                .ToList();
            return produtos.Count == 0 ? NotFound() : Ok(produtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Produto produto)
    {
        try
        {
            Categoria? categoria =
                _ctx.Categorias.Find(produto.CategoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            produto.Categoria = categoria;
            _ctx.Produtos.Add(produto);
            _ctx.SaveChanges();
            return Created("", produto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("buscar/{id}")]
    public IActionResult Buscar([FromRoute] int id)
    {
        try
        {
            Produto? produtoCadastrado =
                _ctx.Produtos
                .Include(x => x.Categoria)
                .FirstOrDefault(x => x.ProdutoId == id);
            if (produtoCadastrado != null)
            {
                return Ok(produtoCadastrado);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("deletar/{id}")]
    public IActionResult Deletar([FromRoute] int id)
    {
        try
        {
            Produto? produtoCadastrado = _ctx.Produtos.Find(id);
            if (produtoCadastrado != null)
            {
                _ctx.Produtos.Remove(produtoCadastrado);
                _ctx.SaveChanges();
                return Ok(_ctx.Produtos
                .Include(x => x.Categoria)
                .ToList());
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id,
        [FromBody] Produto produto)
    {
        try
        {
            //Expressões lambda
            Produto? produtoCadastrado =
                _ctx.Produtos.FirstOrDefault(x => x.ProdutoId == id);

            if (produtoCadastrado != null)
            {

                Categoria? categoria =
                    _ctx.Categorias.Find(produto.CategoriaId);
                if (categoria == null)
                {
                    return NotFound();
                }
                produtoCadastrado.Categoria = categoria;
                produtoCadastrado.Nome = produto.Nome;
                produtoCadastrado.Descricao = produto.Descricao;
                produtoCadastrado.Quantidade = produto.Quantidade;
                produtoCadastrado.Preco = produto.Preco;
                _ctx.Produtos.Update(produtoCadastrado);
                _ctx.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}