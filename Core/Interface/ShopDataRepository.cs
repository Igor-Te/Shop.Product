using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ProductTestWork.Core.Class;
using Shop.ProductTestWork.Core.Interface.InterfaceClasses;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Xml.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace Shop.ProductTestWork.Core.Interface
{
    public class ShopDataRepository : IShopDataRepository
    {

        private readonly ShopDataDb _context;

        public async Task UploadFileAsync(Guid productId, IFormFile file)
        {

            IFormFile uploadedImage = file;
            var productFromDb = await _context.Products.FindAsync(new object[] { productId });
            if (productFromDb == null) return;


            var Temp = new ProductImage();
            Temp.ProductId = productId;
            //Temp.file = file;
            Temp.SetFile(file);

            if (uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                //using var memoryStream = new MemoryStream();
                // file.CopyToAsync(memoryStream);


                // Temp.ImageByte = memoryStream.ToArray();
                using (BinaryReader br = new BinaryReader(uploadedImage.OpenReadStream()))
                {
                    Temp.ImageByte = br.ReadBytes((int)uploadedImage.OpenReadStream().Length);
                }
            }
            var t = Convert.ToBase64String(Temp.ImageByte);
            await _context.ProductImage.AddAsync(Temp);
        }

        public ShopDataRepository(ShopDataDb context)
        {
            _context = context;

        }

        //Interface
        public async Task<List<InterfaceClassProduct>> GetInterfaceProductsNameAsync(string name)
        {
            List<InterfaceClassProduct> result = new List<InterfaceClassProduct>();

            var productFromDb = await _context.Products.Where(h => h.Title.Contains(name)).ToListAsync();
            if (productFromDb == null) return result;
            foreach (var product in productFromDb)
            {
                result.Add(await AddInterfaceProductsIDAsync(product));

            }

            return result;
        }

        public async Task<List<InterfaceClassProductType>> GetInterfaceProductsTypeNameAsync(string name)
        {
            List<InterfaceClassProductType> result = new List<InterfaceClassProductType>();

            var productFromDb = await _context.ProductTypes.Where(h => h.Caption.Contains(name)).ToListAsync();
            if (productFromDb == null) return result;
            foreach (var product in productFromDb)
            {
                result.Add(await AddInterfaceProductTypeIDAsync(product));

            }

            return result;
        }


        public async Task<InterfaceClassProduct> AddInterfaceProductsIDAsync(Product product)
        {
            var result = new InterfaceClassProduct(product.Title, product.Description, product.Price);
            var productFromDbUT = await ListPUBTByIDProduct(product.Id);
            if (productFromDbUT != null)
            {
                foreach (var productDataForOption in productFromDbUT)
                {
                    string productTypeName = "";
                    var productTypeDB = await GetProductTypeAsync(productDataForOption.IdProductType);
                    if (productTypeDB != null)
                    {
                        productTypeName = productTypeDB.Caption;
                    }
                    result.AddTagsProductType(productTypeName);
                    var productFromDbPDFO = await ListByIDPUPT(productDataForOption.Id);
                    if (productFromDbPDFO != null)
                    {
                        string Temp = productTypeName + "|";
                        foreach (var productPDFO in productFromDbPDFO)
                        {
                            Temp = Temp + productPDFO.Text + "|";
                        }
                        result.AddCaptionForOption(Temp);
                    }
                }
            }

            var productFromDbUTIm = await ListPIByID(product.Id);
            if (productFromDbUTIm != null)
            {
                foreach (var productImage in productFromDbUTIm)
                {
                    result.file = productImage.GetFile();
                }
            }
            return result;
        }

        public async Task<List<InterfaceClassProduct>> GetAllInterfaceProductsAsync()
        {

            List<InterfaceClassProduct> result = new List<InterfaceClassProduct>();

            var productFromDb = await GetAllProducts();
            if (productFromDb == null) return result;
            //Собираем данные по всем таблицам, если необходимо ускорить, возможно добавление текстов в таблицы, или формирование одного запроса
            foreach (var product in productFromDb)
            {
                result.Add(await AddInterfaceProductsIDAsync(product));

            }
            return result;
        }

        public async Task<InterfaceClassProductType> AddInterfaceProductTypeIDAsync(ProductType productType)
        {
            var result = new InterfaceClassProductType(productType.Caption);
            var productFromDbDO = await ListPTDOByIDPT(productType.Id);
            if (productFromDbDO != null)
            {

                string Temp = productType.Caption + "|";
                foreach (var productTypeDataOption in productFromDbDO)
                {

                    result.AddTagsProductType(productTypeDataOption.Caption);

                }
            }
            return result;
        }

        public async Task<List<InterfaceClassProductType>> GetAllInterfaceProductsTypesAsync()
        {

            List<InterfaceClassProductType> result = new List<InterfaceClassProductType>();

            var productFromDb = await GetAllProductsTypes();
            if (productFromDb == null) return result;
            //Собираем данные по всем таблицам, если необходимо ускорить, возможно добавление текстов в таблицы, или формирование одного запроса
            foreach (var productType in productFromDb)
            {
                result.Add(await AddInterfaceProductTypeIDAsync(productType));

            }
            return result;
        }


        //GetAll
        public Task<List<ProductImage>> GetAllProductsImage() => _context.ProductImage.ToListAsync();

        public Task<List<Product>> GetAllProducts() => _context.Products.ToListAsync();

        public Task<List<ProductType>> GetAllProductsTypes() => _context.ProductTypes.ToListAsync();


        public Task<List<ProductTypeDataOption>> GetAllProductsTypesDataOption() => _context.ProductTypeDataOptions.ToListAsync();

        public Task<List<ProductDataForOption>> GetAllProductsDataForOptions() => _context.ProductDataForOptions.ToListAsync();

        public Task<List<ProductUseProductType>> GetAllProductsUseProductsTypes() => _context.ProductUseProductTypes.ToListAsync();

        //Get Id
        public async Task<Product> GetProductAsync(Guid Id) => await _context.Products.FindAsync(new object[] { Id });

        public async Task<ProductType> GetProductTypeAsync(Guid Id) => await _context.ProductTypes.FindAsync(new object[] { Id });

        public async Task<ProductTypeDataOption> GetProductTypeDataOptionAsync(Guid Id) => await _context.ProductTypeDataOptions.FindAsync(new object[] { Id });

        public async Task<ProductDataForOption> GetProductDataForOptionAsync(Guid Id) => await _context.ProductDataForOptions.FindAsync(new object[] { Id });

        public async Task<ProductUseProductType> GetProductUseProductTypeAsync(Guid Id) => await _context.ProductUseProductTypes.FindAsync(new object[] { Id });


        //Insert
        public async Task InsertAsync(Product product) => await _context.Products.AddAsync(product);

        public async Task InsertAsync(ProductType product) => await _context.ProductTypes.AddAsync(product);


        public async Task InsertAsync(ProductTypeDataOption product)
        {
            var productFromDb = await _context.ProductTypes.FindAsync(new object[] { product.IdType });
            if (productFromDb == null) return;
            await _context.ProductTypeDataOptions.AddAsync(product);
        }

        public async Task InsertAsync(ProductUseProductType product)
        {
            var productFromDbPT = await _context.ProductTypes.FindAsync(new object[] { product.IdProductType });
            if (productFromDbPT == null) return;

            var productFromDb = await _context.Products.FindAsync(new object[] { product.IdProduct });
            if (productFromDb == null) return;

            await _context.ProductUseProductTypes.AddAsync(product);
        }

        public async Task InsertAsync(ProductDataForOption product)
        {

            var productFromDbUPT = await _context.ProductUseProductTypes.FindAsync(new object[] { product.IdProductUseProductType });
            if (productFromDbUPT == null) return;

            var productFromDb = await _context.ProductTypeDataOptions.FindAsync(new object[] { product.IdProductTypeDataOptions });
            if (productFromDb == null) return;

            if (productFromDb.IdType != productFromDbUPT.IdProductType) return;

            await _context.ProductDataForOptions.AddAsync(product);
        }


        public async Task InsertProductByNameAsync(string product)
        {

            var Temp = new Product();
            var splitArray = product.Split(',', 3);
            if (splitArray.Length != 3) return;
            Temp.Title = splitArray[0];
            Temp.Description = splitArray[1];
            try
            {
                Temp.Price = Convert.ToDouble(splitArray[2]);
            }
            catch
            {
                return;
            };

            Temp.Id = Guid.NewGuid();
            Temp.UserId = new Guid("12345678-1234-1234-1234-223456789123");
            await _context.Products.AddAsync(Temp);
        }

        public async Task InsertProductTypeByNameAsync(string product)
        {

            var Temp = new ProductType();

            Temp.Caption = product;


            Temp.Id = Guid.NewGuid();
            Temp.UserId = new Guid("12345678-1234-1234-1234-223456789123");
            await _context.ProductTypes.AddAsync(Temp);
        }

        public async Task InsertProductTypeDataOptionByNameAsync(string typeName,string caption)
        {

            var Temp1 = new ProductTypeDataOption();

            var productTypeFromDb = await _context.ProductTypes.Where(h => h.Caption.Contains(typeName)).ToListAsync();
            if (productTypeFromDb == null) return;

            foreach (var type in productTypeFromDb)
            {
                bool Use = false;
                var ListTemp =await ListPTDOByIDPT(type.Id);
                foreach (var productUse in ListTemp)
                {
                    if(productUse.Caption== caption)
                    {
                        Use= true;
                        break;
                    }

                }

                if(!Use)
                {
                    var Temp = new ProductTypeDataOption();
                    Temp.Caption = caption;
                    Temp.Id = Guid.NewGuid();
                    Temp.IdType = type.Id;
                    await _context.ProductTypeDataOptions.AddAsync(Temp);
                }
            }
           
        }
        public async Task InsertProductUseProductTypeByNameAsync(string productName, string typeName)
        {
            var productFromDb = await _context.Products.Where(h => h.Title.Contains(productName)).ToListAsync(); 
            if (productFromDb == null) return;

            var productTypeFromDb = await _context.ProductTypes.Where(h => h.Caption.Contains(typeName)).ToListAsync(); 
            if (productTypeFromDb == null) return;


            //Добавление всех комбинаций с проверкой на наличие в текущей базе
            foreach (var product in productFromDb)
            {
                var ListProduct = await ListByIDPUPT(product.Id);
                foreach (var type in productTypeFromDb)
                {
                    var ListType= await ListPUPTByIDPT(type.Id);
                    bool Use = false;
                    foreach (var p in ListProduct)
                    {
                        if (!Use)
                        {
                            break;
                        }
                        foreach (var t in ListType)
                        {
                            if (p.Id == t.Id)
                            {
                                Use = true;
                                break;
                            }
                        }

                    }

                    if (!Use)
                    {
                        ProductUseProductType Temp = new ProductUseProductType();
                        Temp.Id = Guid.NewGuid();
                        Temp.IdProduct = product.Id;
                        Temp.IdProductType = type.Id;

                        await _context.ProductUseProductTypes.AddAsync(Temp);
                    }
                }
            }

            return;
        }



        //Обновить данные
        public async Task UpdateAsync(Product product)
        {
            var productFromDb = await _context.Products.FindAsync(new object[] { product.Id });
            if (productFromDb == null) return;
            productFromDb.Title = product.Title;
            productFromDb.Description = product.Description;
            productFromDb.UserId = product.UserId; 
            productFromDb.Price = product.Price;
        }

        public async Task UpdateAsync(ProductType product)
        {
            var productFromDb = await _context.ProductTypes.FindAsync(new object[] { product.Id });
            if (productFromDb == null) return;
            productFromDb.UserId = product.UserId;
            productFromDb.Caption = product.Caption;
        }

        public async Task UpdateAsync(ProductTypeDataOption product)
        {
            var productFromDb = await _context.ProductTypeDataOptions.FindAsync(new object[] { product.Id });
            if (productFromDb == null) return;
            productFromDb.Caption = product.Caption;
        }

        public async Task UpdateAsync(ProductDataForOption product)
        {
            var productFromDb = await _context.ProductDataForOptions.FindAsync(new object[] { product.Id });
            if (productFromDb == null) return;
            productFromDb.Text = product.Text;
        }

        public async Task UpdateAsync(ProductUseProductType product)
        {
            var productFromDb = await _context.ProductUseProductTypes.FindAsync(new object[] { product.Id });
            if (productFromDb == null) return;
            //Запрет на изменение в данный момент
        }

        //Удалить

        public async Task DeleteProductAsync(Guid productId)
        {
            var productFromDb = await _context.Products.FindAsync(new object[] { productId });
            if (productFromDb == null) return;



            var ListFromDBP = await ListPUBTByIDProduct(productFromDb.Id);

            if (ListFromDBP == null) return;

            foreach (var productDataForOption in ListFromDBP)
            {
                await DeleteProductsUseProductTypesAsync(productDataForOption.Id);
            }

            _context.Products.Remove(productFromDb);
           
        }

        public async Task DeleteProductTypeAsync(Guid productId)
        {
            var productFromDb = await _context.ProductTypes.FindAsync(new object[] { productId });
            if (productFromDb == null) return;

            var ListFromDB = await ListPTDOByIDPT(productFromDb.Id);

            if (ListFromDB == null) return;

            foreach (var productDataForOption in ListFromDB)
            {
                await DeleteProductTypeDataOptionAsync(productDataForOption.Id);
            }


            var ListFromDBPT = await ListPUPTByIDPT(productFromDb.Id);

            if (ListFromDBPT == null) return;

            foreach (var productDataForOption in ListFromDB)
            {
                await DeleteProductsUseProductTypesAsync(productDataForOption.Id);
            }


            _context.ProductTypes.Remove(productFromDb);
        }


        public async Task DeleteProductsUseProductTypesAsync(Guid productId)
        {

            var productFromDb = await _context.ProductUseProductTypes.FindAsync(new object[] { productId });
            if (productFromDb == null) return;

            var ListFromDB = await ListByIDPUPT(productFromDb.Id);

            if (ListFromDB == null) return;

            foreach (var productDataForOption in ListFromDB)
            {
               await DeleteProductDataForOptionsAsync(productDataForOption.Id);
            }

            _context.ProductUseProductTypes.Remove(productFromDb);
        }



        public async Task DeleteProductTypeDataOptionAsync(Guid productId)
        {

            var productFromDb = await _context.ProductTypeDataOptions.FindAsync(new object[] { productId });
            if (productFromDb == null) return;

            var ListFromDB = await ListByIDPTDO(productFromDb.Id);

            if (ListFromDB == null) return;

            foreach (var productDataForOption in ListFromDB)
            {
               await DeleteProductDataForOptionsAsync(productDataForOption.Id);
            }

                _context.ProductTypeDataOptions.Remove(productFromDb);
        }


        public async Task DeleteProductDataForOptionsAsync(Guid productId)
        {
            var productFromDb = await _context.ProductDataForOptions.FindAsync(new object[] { productId });
            if (productFromDb == null) return;
            _context.ProductDataForOptions.Remove(productFromDb);
        }

        //Сохранение
        public async Task SaveAsync() => await _context.SaveChangesAsync();

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        //Различные выдачи по параметрам

        public Task<List<ProductDataForOption>> ListByIDPUPT(Guid productId)
        {
           return _context.ProductDataForOptions.Where(h => h.IdProductUseProductType== productId).ToListAsync();
        }

        public Task<List<ProductDataForOption>> ListByIDPTDO(Guid productId)
        {
            return _context.ProductDataForOptions.Where(h => h.IdProductTypeDataOptions == productId).ToListAsync();
        }

        public Task<List<ProductTypeDataOption>> ListPTDOByIDPT(Guid productId)
        {
            return _context.ProductTypeDataOptions.Where(h => h.IdType == productId).ToListAsync();
        }

        public Task<List<ProductUseProductType>> ListPUBTByIDProduct(Guid productId)
        {
            return _context.ProductUseProductTypes.Where(h => h.IdProduct == productId).ToListAsync();
        }

        public Task<List<ProductUseProductType>> ListPUPTByIDPT(Guid productId)
        {
            return _context.ProductUseProductTypes.Where(h => h.IdProductType == productId).ToListAsync();
        }

        public Task<List<ProductImage>> ListPIByID(Guid productId)
        {
            return _context.ProductImage.Where(h => h.ProductId == productId).ToListAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

     
    }
}
