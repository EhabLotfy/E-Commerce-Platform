namespace Infrastructure;

public class DataContext:DbContext
{
    public DataContext(DbContextOptions <DataContext> optionas): base(optionas)         
    {
        
    }

    #region DbSets
    public DbSet<CustomerSet> Customers { get; set; }
    public DbSet<ProductSet> Products { get; set; }
    public DbSet<OrderSet> Orders { get; set; }
    public DbSet<OrderItemsSet> OrderItems { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }
}
