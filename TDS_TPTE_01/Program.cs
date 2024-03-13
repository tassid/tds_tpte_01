using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

var produtos = new List<Produto>
{
    new Produto { Id = 1, Nome = "Produto 1", Descricao = "Descrição do Produto 1", Preco = 19.99m, Quantidade = 10 },
    new Produto { Id = 2, Nome = "Produto 2", Descricao = "Descrição do Produto 2", Preco = 29.99m, Quantidade = 20 },
    new Produto { Id = 3, Nome = "Produto 3", Descricao = "Descrição do Produto 3", Preco = 39.99m, Quantidade = 30 }
};

var app = WebApplication.Create(args);

// GET /produtos
app.MapGet("/produtos", () => Results.Ok(produtos));

// GET /produtos/{id}
app.MapGet("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto == null)
        return Results.NotFound("Produto não encontrado.");

    return Results.Ok(produto);
});

// POST /produtos
app.MapPost("/produtos", (Produto produto) =>
{
    produto.Id = produtos.Count > 0 ? produtos.Max(p => p.Id) + 1 : 1;
    produtos.Add(produto);
    return Results.Created($"/produtos/{produto.Id}", produto);
});

// PUT /produtos/{id}
app.MapPut("/produtos/{id}", (int id, Produto produtoAtualizado) =>
{
    var produtoExistente = produtos.FirstOrDefault(p => p.Id == id);
    if (produtoExistente == null)
        return Results.NotFound("Produto não encontrado.");

    produtoExistente.Nome = produtoAtualizado.Nome;
    produtoExistente.Descricao = produtoAtualizado.Descricao;
    produtoExistente.Preco = produtoAtualizado.Preco;
    produtoExistente.Quantidade = produtoAtualizado.Quantidade;

    return Results.Ok(produtoExistente);
});

// DELETE /produtos/{id}
app.MapDelete("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto == null)
        return Results.NotFound("Produto não encontrado.");

    produtos.Remove(produto);
    return Results.NoContent();
});

// Executa a aplicação
app.Run();

// Classe que representa um produto
public record Produto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}
