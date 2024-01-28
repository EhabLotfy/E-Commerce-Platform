namespace Infrastructure.Seeder;

public class DataSeed
{
    public static async Task SaveData(DataContext context)
    {
        // Adding Products seed
        if (!context.Products.Any())
        {
            List<ProductSet> products = new List<ProductSet>()
            {
               new ProductSet
               {
                Name ="سكتشرز جو ووك 6",
                Description = "حذاء رياضي للمشي عالي الأداء جو ووك 6 بأستك مرن سهل الارتداء للرجال من سكيتشرز",
                Price = 2390.5,
                Stock = 12
               },
                  new ProductSet
               {
                Name ="Adidas Run Falcon for kids",
                Description = "حذاء رياضي رن فالكون 3.0 للأطفال من الجنسين من اديداس",
                Price = 1250,
                Stock = 6
               }
            };

            await context.Products.AddRangeAsync(products);
        }
        // Adding Customers seed
        if (!context.Customers.Any())
        {
           List<CustomerSet> customers = new List<CustomerSet>()
            {
               new CustomerSet
               {
                Name ="Mohamed Ahmed",
                Email = "Mohamed@domain.com",
                Address = "1 El Nozha ST, Misr El Gadida, Cairo" 
               },
                  new CustomerSet
               {
                Name ="Ahmed Lotfy",
                Email = "Ahmed@gmail.com",
                Address = "1 Makram Ebied ST, Nasr city, Cairo"
               }
            };

            await context.Customers.AddRangeAsync(customers);
        }
        // Adding Orders with items seed
        if (!context.Orders.Any()) 
        {
            List<OrderSet> orders = new List<OrderSet>()
           {
               new OrderSet
               {
                   CustomerId = 1,
                   OrderDate = DateTime.Now,
                   status = OrderStatusEnum.Delivered,
                   TotalAmount = 2390.5,                       
                   OrderItems = new List<OrderItemsSet>()
                   {
                      new OrderItemsSet
                      {
                          OrderId=1,
                          Price = 2390.5,
                          ProductId= 1,
                          Quantity = 1,                              
                      }
                   }
               },
               new OrderSet
               {
                   CustomerId = 2,
                   OrderDate = DateTime.Now,
                   status = OrderStatusEnum.Shipped,
                   TotalAmount = 1250,
                   OrderItems = new List<OrderItemsSet>()
                   {
                      new OrderItemsSet
                      {
                          OrderId=2,
                          Price = 1250,
                          ProductId= 2,
                          Quantity = 1,
                      }
                   }
               }
           };
            await context.Orders.AddRangeAsync(orders);
        }

        await context.SaveChangesAsync();
    }
}
