// Importações necessárias
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Json;
using System.Collections.Generic;
using System.Linq;

// Definição da classe principal do programa
class Program
{
    // Método principal que inicia a aplicação
    static void Main(string[] args)
    {

        // Inicialização do objeto Product para teste
        var produto = new Produto { Id = 1, Nome = "Exemplo", Descricao = "Descrição", Preco = 10.99m, Quantidade = 5 };

        // Cria uma lista em memória para armazenar os produtos
        var produtos = new List<Produto>();

        // Cria um construtor para a aplicação web
        var construtor = WebApplication.CreateBuilder(args);

        // Adiciona os serviços necessários para a aplicação
        construtor.Services.AddEndpointsApiExplorer();
        construtor.Services.AddSwaggerGen();

        // Cria a aplicação
        var app = construtor.Build();

        // Define as rotas e a lógica para cada rota
        app.MapGet("/produtos", () =>
        {
            // Retorna todos os produtos como JSON
            return Results.Ok(produtos);
        });

        app.MapGet("/produtos/{id}", (int id) =>
        {
            // Busca o produto pelo ID
            var produto = produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
            {
                // Se não encontrar, retorna 404 Not Found
                return Results.NotFound();
            }

            // Retorna o produto encontrado
            return Results.Ok(produto);
        });

        app.MapPost("/produtos", (Produto produto) =>
        {
            // Gera um novo ID para o produto
            var novoId = produtos.Count > 0 ? produtos.Max(p => p.Id) + 1 : 1;
            produto = produto with { Id = novoId }; // Atribui o novo ID ao produto
            // Adiciona o produto à lista
            produtos.Add(produto);
            // Retorna 201 Created com o produto adicionado
            return Results.Created($"/produtos/{produto.Id}", produto);
        });

        app.MapPut("/produtos/{id}", (int id, Produto produtoAtualizado) =>
        {
            // Busca o produto pelo ID
            var produtoExistente = produtos.FirstOrDefault(p => p.Id == id);
            if (produtoExistente == null)
            {
                // Se não encontrar, retorna 404 Not Found
                return Results.NotFound();
            }

            // Atualiza as propriedades do produto
            produtoExistente = produtoExistente with
            {
                Nome = produtoAtualizado.Nome,
                Descricao = produtoAtualizado.Descricao,
                Preco = produtoAtualizado.Preco,
                Quantidade = produtoAtualizado.Quantidade
            };


            // Retorna o produto atualizado
            return Results.Ok(produtoExistente);
        });

        app.MapDelete("/produtos/{id}", (int id) =>
        {
            // Busca o produto pelo ID
            var produto = produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
            {
                // Se não encontrar, retorna 404 Not Found
                return Results.NotFound();
            }

            // Remove o produto da lista
            produtos.Remove(produto);

            // Retorna 204 No Content
            return Results.NoContent();
        });

        // Adiciona o middleware para habilitar o Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        // Executa a aplicação
        app.Run();
    }
}

// Classe que representa um produto
record Produto
{
    public int Id { get; init; }
    public string? Nome { get; init; }
    public string? Descricao { get; init; }
    public decimal Preco { get; init; }
    public int Quantidade { get; init; }
}
