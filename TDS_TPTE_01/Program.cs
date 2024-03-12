// Importações necessárias
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

// Definição da classe principal do programa
class Program
{
    // Método principal que inicia a aplicação
    static void Main(string[] args)
    {
        // Cria uma lista em memória para armazenar os produtos
        var products = new List<Product>();

        // Cria um construtor para a aplicação web
        var builder = WebApplication.CreateBuilder(args);

        // Adiciona os serviços necessários para a aplicação
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Cria a aplicação
        var app = builder.Build();

        // Define as rotas e a lógica para cada rota
        app.MapGet("/products", () =>
        {
            // Retorna todos os produtos como JSON
            return Results.Ok(products);
        });

        app.MapGet("/products/{id}", (int id) =>
        {
            // Busca o produto pelo ID
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                // Se não encontrar, retorna 404 Not Found
                return Results.NotFound();
            }

            // Retorna o produto encontrado
            return Results.Ok(product);
        });

        app.MapPost("/products", (Product product) =>
        {
            // Gera um novo ID para o produto
            product.Id = products.Count + 1;
            // Adiciona o produto à lista
            products.Add(product);
            // Retorna 201 Created com o produto adicionado
            return Results.Created($"/products/{product.Id}", product);
        });

        app.MapPut("/products/{id}", (int id, Product updatedProduct) =>
        {
            // Busca o produto pelo ID
            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                // Se não encontrar, retorna 404 Not Found
                return Results.NotFound();
            }

            // Atualiza as propriedades do produto
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Quantity = updatedProduct.Quantity;

            // Retorna o produto atualizado
            return Results.Ok(existingProduct);
        });

        app.MapDelete("/products/{id}", (int id) =>
        {
            // Busca o produto pelo ID
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                // Se não encontrar, retorna 404 Not Found
                return Results.NotFound();
            }

            // Remove o produto da lista
            products.Remove(product);

            // Retorna 204 No Content
            return Results.NoContent();
        });

        // Adiciona o middleware para habilitar Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        // Executa a aplicação
        app.Run();
    }
}

// Classe que representa um produto
record Product
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
}
