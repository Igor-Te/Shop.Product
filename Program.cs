using Shop.ProductTestWork;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shop.ProductTestWork.Core.Class;
using Shop.ProductTestWork.Core.Interface;
using Shop.ProductTestWork.Core.Interface.InterfaceClasses;
using System.Collections.Generic;

namespace Shop.WebApi
{
    public class Program
{
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ShopDataDb>(options =>
            {
                options.UseSqlite("Data Source= Shop.Data.db"); ///UseSqlite("Data Source=Shop.Data.db");
            });
           
            builder.Services.AddScoped <IShopDataRepository, ShopDataRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ShopDataDb>();
                db.Database.EnsureCreated();
               
            }


            //Поиск по имени 
            app.MapGet("/productByName/{Name}", async (string name, IShopDataRepository repository) =>
            await repository.GetInterfaceProductsNameAsync(name) is List<InterfaceClassProduct> product
            ? Results.Ok(product)
            : Results.NotFound())
            .Produces<Product>(StatusCodes.Status200OK)
            .WithName("GetProductByName")
            .WithTags("ProductPage");

            app.MapGet("/productTypeByName/{Name}", async (string name, IShopDataRepository repository) =>
            await repository.GetInterfaceProductsTypeNameAsync(name) is List<InterfaceClassProductType> product
            ? Results.Ok(product)
            : Results.NotFound())
                .Produces<Product>(StatusCodes.Status200OK)
                .WithName("GetProductTypeByName")
                .WithTags("ProductTypePage");


            //Создание по имени
            
           app.MapPost("/productByName", async ([FromBody] string product, IShopDataRepository repository) =>
                        {
                            // NewMethod(product);
                            await repository.InsertProductByNameAsync(product);
                            await repository.SaveAsync();
                            return Results.Created($"/productByName/{product}", product);
                        })
                .Accepts<Product>("application/json")
                .Produces<Product>(StatusCodes.Status201Created)
                .WithName("CreateProductByName")
                .WithTags("CreatorsByName");

           app.MapPost("/productTypeByName", async ([FromBody] string product, IShopDataRepository repository) =>
                        {
                            // NewMethod(product);
                            await repository.InsertProductTypeByNameAsync(product);
                            await repository.SaveAsync();
                            return Results.Created($"/productTypeByName/{product}", product);
                        })
                .Accepts<Product>("application/json")
                .Produces<Product>(StatusCodes.Status201Created)
                .WithName("CreateProductTypeByName")
                .WithTags("CreatorsByName");

           app.MapPost("/productTypeDataOptionByName/{type}", async (string type,[FromBody] string product, IShopDataRepository repository) =>
                 {
                   await repository.InsertProductTypeDataOptionByNameAsync(type,product);
                   await repository.SaveAsync();
                   return Results.Created($"/productTypeDataOptionByName/{product}", product);
                })
              .Accepts<Product>("application/json")
              .Produces<Product>(StatusCodes.Status201Created)
              .WithName("CreateProductTypeDataOptionByName")
              .WithTags("CreatorsByName");


            app.MapPost("/productTypeDataOptionByName/{product}", async (string product, [FromBody] string type, IShopDataRepository repository) =>
            {
                await repository.InsertProductUseProductTypeByNameAsync(product,type);
                await repository.SaveAsync();
                return Results.Created($"/productUseProductTypeByNameAsync/{product}", product);
            })
               .Accepts<Product>("application/json")
               .Produces<Product>(StatusCodes.Status201Created)
               .WithName("CreateProductUseProductTypeByName")
               .WithTags("CreatorsByName");



            //Загрузка Рисунка
            app.MapGet("/productImage", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllProductsImage()))
              .Produces<List<ProductImage>>(StatusCodes.Status200OK)
              .WithName("GetAllProductImage")
              .WithTags("Getters");

            

            //GetAll
            app.MapGet("/productText", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllInterfaceProductsAsync()))
                      .Produces<List<InterfaceClassProduct>>(StatusCodes.Status200OK)
                      .WithName("GetAllProductsText")
                      .WithTags("ProductPage");

            app.MapGet("/productTypeText", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllInterfaceProductsTypesAsync()))
                      .Produces<List<InterfaceClassProduct>>(StatusCodes.Status200OK)
                      .WithName("GetAllProductsTypeText")
                      .WithTags("ProductTypePage");




            app.MapGet("/product", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllProducts()))
                    .Produces<List<Product>>(StatusCodes.Status200OK)
                    .WithName("GetAllProducts")
                    .WithTags("Getters");

            app.MapGet("/productType", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllProductsTypes()))
                   .Produces<List<ProductType>>(StatusCodes.Status200OK)
                   .WithName("GetAllProductsTypes")
                   .WithTags("Getters");

            app.MapGet("/productUseProductType>", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllProductsUseProductsTypes()))
                   .Produces<List<ProductUseProductType>>(StatusCodes.Status200OK)
                   .WithName("GetAllProductsUseProductsType")
                   .WithTags("Getters");

            app.MapGet("/productTypeDataOption>", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllProductsTypesDataOption()))
                  .Produces<List<ProductTypeDataOption>>(StatusCodes.Status200OK)
                  .WithName("GetAllProductsTypesDataOption")
                  .WithTags("Getters");

            app.MapGet("/productUseTypeDataOption>", async (IShopDataRepository repository) => Results.Ok(await repository.GetAllProductsDataForOptions()))
                    .Produces<List<ProductDataForOption>>(StatusCodes.Status200OK)
                    .WithName("GetAllProductDataForOptions")
                    .WithTags("Getters");





            //GetId
            app.MapGet("/product/{id}", async (Guid id, IShopDataRepository repository) =>
            await repository.GetProductAsync(id) is  Product product
            ? Results.Ok(product)
            : Results.NotFound())
             .Produces<Product>(StatusCodes.Status200OK)
             .WithName("GetProduct")
             .WithTags("Getters");

            app.MapGet("/productType/{id}", async (Guid id, IShopDataRepository repository) =>
            await repository.GetProductTypeAsync(id) is ProductType productT
             ? Results.Ok(productT)
             : Results.NotFound())
             .Produces<Product>(StatusCodes.Status200OK)
            .WithName("GetProductType")
            .WithTags("Getters");


            // Добавление прямое в базу

            app.MapPost("/product", async ([FromBody] Product product, IShopDataRepository repository) =>
            {
                // NewMethod(product);
                await repository.InsertAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/product/{product.Id}", product);
            })
            .Accepts<Product>("application/json")
            .Produces<Product>(StatusCodes.Status201Created)
            .WithName("CreateProduct")
            .WithTags("Creators");

            app.MapPost("/productType", async ([FromBody] ProductType product, IShopDataRepository repository) =>
            {
                
                await repository.InsertAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productType/{product.Id}", product);
            })
            .Accepts<ProductType>("application/json")
            .Produces<ProductType>(StatusCodes.Status201Created)
            .WithName("CreateProductType")
            .WithTags("Creators");


            app.MapPost("/productsUseProductsTypes", async ([FromBody] ProductUseProductType product, IShopDataRepository repository) =>
            {
                
                await repository.InsertAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productsUseProductsTypes/{product.Id}", product);
            })
                .Accepts<ProductUseProductType>("application/json")
                .Produces<ProductUseProductType>(StatusCodes.Status201Created)
                .WithName("CreateProductUseProductsTypes")
                .WithTags("Creators");


            app.MapPost("/productTypeDataOption", async ([FromBody] ProductTypeDataOption product, IShopDataRepository repository) =>
            {
                
                await repository.InsertAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productTypeDataOption/{product.Id}", product);
            })
             .Accepts<ProductTypeDataOption>("application/json")
             .Produces<ProductTypeDataOption>(StatusCodes.Status201Created)
             .WithName("CreateProductTypeDataOption")
             .WithTags("Creators");

            app.MapPost("productDataForOption", async ([FromBody] ProductDataForOption product, IShopDataRepository repository) =>
            {
                
                await repository.InsertAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productDataForOption/{product.Id}", product);
            })
                .Accepts<ProductDataForOption>("application/json")
                .Produces<ProductDataForOption>(StatusCodes.Status201Created)
                .WithName("CreateProductDataForOption")
                .WithTags("Creators");
            
            //Изменение
            app.MapPut("/product", async ([FromBody] Product product, IShopDataRepository repository) => 
            {
                await repository.UpdateAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/product/{product.Id}", product);
            })
                .Accepts<Product>("application/json")
                .WithName("UpdateProduct")
                .WithTags("Updaters"); 

            app.MapPut("/productType", async ([FromBody] ProductType product, IShopDataRepository repository) =>
            {
                await repository.UpdateAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productType/{product.Id}", product);
            })
                .Accepts<ProductType>("application/json")
                .WithName("UpdateProductType")
                .WithTags("Updaters"); 

            app.MapPut("/productUseProductType", async ([FromBody] ProductUseProductType product, IShopDataRepository repository) =>
            {
                await repository.UpdateAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productUseProductType/{product.Id}", product);
            })
              .Accepts< ProductUseProductType>("application/json")
              .WithName("UpdateProductUseProductType")
              .WithTags("Updaters"); 

            app.MapPut("/productTypeDataOption", async ([FromBody] ProductTypeDataOption product, IShopDataRepository repository) =>
            {
                await repository.UpdateAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productTypeDataOption/{product.Id}", product);
            })
              .Accepts<ProductTypeDataOption>("application/json")
              .WithName("UpdateProductTypeDataOption")
              .WithTags("Updaters");

            app.MapPut("/productDataForOption", async ([FromBody] ProductDataForOption product, IShopDataRepository repository) =>
            {
                await repository.UpdateAsync(product);
                await repository.SaveAsync();
                return Results.Created($"/productDataForOption/{product.Id}", product);
            })
             .Accepts<ProductDataForOption>("application/json")
             .WithName("UpdateProductDataForOption")
             .WithTags("Updaters");

            //Удаление 

            app.MapDelete("/product/{id}", async (Guid id, IShopDataRepository repository) => {
                
                await repository.DeleteProductAsync(id);
                await repository.SaveAsync();
                return Results.NoContent();
            })
                .WithName("DeleteProduct")
                .WithTags("Deleters");

            app.MapDelete("/productType/{id}", async (Guid id, IShopDataRepository repository) => {
                
                await repository.DeleteProductTypeAsync(id);
                await repository.SaveAsync();
                return Results.NoContent();
            })
               .WithName("DeleteProductType")
               .WithTags("Deleters");

            app.MapDelete("/productsUseProductsTypes/{id}", async (Guid id, IShopDataRepository repository) => {
                
                await repository.DeleteProductTypeAsync(id);
                await repository.SaveAsync();
                return Results.NoContent();
            })
                .WithName("DeleteProductsUseProductsTypes")
                .WithTags("Deleters");

            app.MapDelete("/productTypeDataOption/{id}", async (Guid id, IShopDataRepository repository) => {
             
                await repository.DeleteProductTypeDataOptionAsync(id);
                await repository.SaveAsync();
                return Results.NoContent();
            })
                .WithName("DeleteProductTypeDataOption")
                .WithTags("Deleters");

            app.MapPut("/Image/{id}", async (Guid id, IFormFile File, IShopDataRepository repository) => {
               
                await repository.UploadFileAsync(id, File);
                await repository.SaveAsync();
                return Results.NoContent();
            })
                .WithName("Image")
                .WithTags("Download");


            app.MapDelete("/productDataForOptions/{id}", async (Guid id, IShopDataRepository repository) => {
               
                await repository.DeleteProductDataForOptionsAsync(id);
                await repository.SaveAsync();
                return Results.NoContent();
            })
               .WithName("DeleteProductDataForOptions")
               .WithTags("Deleters");

            app.UseHttpsRedirection();

            app.Run();

            
        }




    }
}
