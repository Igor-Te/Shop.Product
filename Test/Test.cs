using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shop.ProductTestWork.Core.Class;
using Shop.ProductTestWork.Core.Interface;

namespace Shop.ProductTestWork.Test
{
    public class Test
    {
        public Test() { }

        public void TestBase(ShopDataDb db)
        {
            var prodoct = db.Products.ToList();
            Console.WriteLine("Список объектов Products:");
            foreach (Product u in prodoct)
            {
                // Console.WriteLine($"{u.Id} - {u.UserId} - {u.Title} - {u.Description} - {u.Price}");
            }

            var type = db.ProductTypes.ToList();
            Console.WriteLine("Список объектов ProductTypes:");
            foreach (ProductType u in type)
            {
                Console.WriteLine($"{u.Id} - {u.UserId} - {u.Caption}");
            }

            var typeDataOptions = db.ProductTypeDataOptions.ToList();
            Console.WriteLine("Список объектов ProductTypeDataOption:");
            foreach (ProductTypeDataOption u in typeDataOptions)
            {
                Console.WriteLine($"{u.Id} - {u.IdType} - {u.Caption}");
            }

            var useProductTypes = db.ProductUseProductTypes.ToList();
            Console.WriteLine("Список объектов ProductUseProductTypes:");
            foreach (ProductUseProductType u in useProductTypes)
            {
                Console.WriteLine($"{u.IdProduct} - {u.IdProductType}");
            }

            var dataForOptions = db.ProductDataForOptions.ToList();
            Console.WriteLine("Список объектов ProductDataForOptions:");
            foreach (ProductDataForOption u in dataForOptions)
            {
                Console.WriteLine($"{u.Id} - {u.IdProductUseProductType}  - {u.IdProductTypeDataOptions} - {u.Text}");
            }

            Console.WriteLine("Проверка Базы закончена");

            //Остановка в случае необходимости
            ///Console.ReadKey();
        }
    }
}
