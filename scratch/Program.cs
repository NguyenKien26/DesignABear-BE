using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Models;
using System.IO;

Console.WriteLine("Starting Duplicate Slug Fixer...");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(@"C:\Users\triNguyxn\Documents\GitHub\cp-sp26-toys-ecommerce\DesignABear.WebAPI\appsettings.json")
    .Build();

var services = new ServiceCollection();
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

using var serviceProvider = services.BuildServiceProvider();
using var context = serviceProvider.GetRequiredService<AppDbContext>();

Console.WriteLine("Connected to Database. Checking for duplicate slugs...");

var allProducts = await context.Products.ToListAsync();
var duplicateSlugs = allProducts
    .GroupBy(p => p.Slug)
    .Where(g => g.Count() > 1)
    .Select(g => new { Slug = g.Key, Count = g.Count(), ids = g.Select(p => p.ProductId).ToList() })
    .ToList();

if (!duplicateSlugs.Any())
{
    Console.WriteLine("No duplicate slugs found.");
}
else
{
    foreach (var dup in duplicateSlugs)
    {
        Console.WriteLine($"Found {dup.Count} products with slug: {dup.Slug}");
        var products = allProducts.Where(p => dup.ids.Contains(p.ProductId)).OrderBy(p => p.CreatedAt).ToList();
        
        // Keep the first one, update others
        for (int i = 1; i < products.Count; i++)
        {
            var p = products[i];
            var newSlug = $"{p.Slug}-{Guid.NewGuid().ToString().Substring(0, 4)}";
            Console.WriteLine($"Updating Product {p.ProductId} slug from {p.Slug} to {newSlug}");
            p.Slug = newSlug;
        }
    }
    
    await context.SaveChangesAsync();
    Console.WriteLine("Duplicate slugs fixed successfully.");
}
