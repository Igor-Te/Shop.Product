using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ProductTestWork.Core.Class;
using Shop.ProductTestWork.Core.Interface.InterfaceClasses;

namespace Shop.ProductTestWork.Core.Interface
{
    public interface IShopDataRepository :IDisposable
    {
        //GetAll

     
        Task UploadFileAsync(Guid productId, IFormFile file);
       

        Task<List<InterfaceClassProduct>> GetAllInterfaceProductsAsync();

        Task<List<InterfaceClassProductType>> GetAllInterfaceProductsTypesAsync();

        Task<List<ProductImage>> GetAllProductsImage();

        Task<List<Product>> GetAllProducts();

        Task<List<ProductType>> GetAllProductsTypes();

        Task<List<ProductUseProductType>> GetAllProductsUseProductsTypes();

        Task<List<ProductTypeDataOption>> GetAllProductsTypesDataOption();

        Task<List<ProductDataForOption>> GetAllProductsDataForOptions();

        //GetId

        Task<List<InterfaceClassProduct>> GetInterfaceProductsNameAsync(string name);

        Task<List<InterfaceClassProductType>> GetInterfaceProductsTypeNameAsync(string name);


        Task<Product> GetProductAsync(Guid Id);

        Task<ProductType> GetProductTypeAsync(Guid Id);

        Task<ProductTypeDataOption> GetProductTypeDataOptionAsync(Guid Id);

        Task<ProductDataForOption> GetProductDataForOptionAsync(Guid Id);

       Task<ProductUseProductType> GetProductUseProductTypeAsync(Guid Id);

        //Insert

        Task InsertAsync(Product product);

        Task InsertAsync(ProductType product);

        Task InsertAsync(ProductTypeDataOption product);

        Task InsertAsync(ProductDataForOption product);

        Task InsertAsync(ProductUseProductType product);

        Task InsertProductByNameAsync(string product);

        Task InsertProductTypeByNameAsync(string product);

        Task InsertProductUseProductTypeByNameAsync(string productName, string typeName);



        Task InsertProductTypeDataOptionByNameAsync(string typeName, string caption);

        //Update ID
        Task UpdateAsync(Product product);

        Task UpdateAsync(ProductType product);

        Task UpdateAsync(ProductTypeDataOption product);

        Task UpdateAsync(ProductDataForOption product);

        Task UpdateAsync(ProductUseProductType product);



        Task DeleteProductAsync(Guid productId);

        Task DeleteProductTypeAsync(Guid productId);

        Task DeleteProductsUseProductTypesAsync(Guid productId);

        Task DeleteProductTypeDataOptionAsync(Guid productId);

        Task DeleteProductDataForOptionsAsync(Guid productId);

        Task SaveAsync();
    }
}
