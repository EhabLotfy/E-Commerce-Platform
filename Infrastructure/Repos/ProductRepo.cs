using Core.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class ProductRepo : IProductRepo
    {
        private readonly DataContext _context;

        public ProductRepo(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// returns true if product instock and false if product Out of stock
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool IsAvailableInStock(int productId, int quantity)
        {
            if (quantity > 0)
            {
                var validateProductQuantity = _context.Products.Where(p => p.Id == productId && p.Stock >= quantity).FirstOrDefault();

                return validateProductQuantity != null ? true : false;
            }
            else
             return false; 
        }

        public bool UpdateProductQuantity(int productId, int quantity, bool IsAdd)
        {
            if( IsAvailableInStock(productId, quantity))
            {
                var product = _context.Products.FirstOrDefault(p=>p.Id ==  productId);
                if (product == null)
                {
                    return false;
                }
                if(IsAdd)
                    product.Stock += quantity;
                else
                product.Stock -= quantity;

                _context.Products.Update(product);
                _context.SaveChanges();
                return true;
            }

            return false;
            
        }
    }
}
