using Messages;
using Repository;
using StockService.Core.Entities;

namespace StockService.Core.Services;

public class ProductService
{
    private readonly IRepository<Product> _repository;
    
    public ProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }
    
    public void PopulateDb()
    {
        // Populate the database with some products
        _repository.Add(new Product
        {
            ProductId = 1,
            Stock = 20
        });
        _repository.Add(new Product
        {
            ProductId = 2,
            Stock = 30
        });
        _repository.Add(new Product
        {
            ProductId = 3,
            Stock = 40
        });
    }
    
    public IEnumerable<Product> GetOrderProducts(List<OrderItem> orderedItems)
    {
        // DONE
        var products = GetProducts();

        var query = (from product in products
                     join order in orderedItems on product.ProductId equals order.ProductId
                     where product.Stock >= order.Quantity
                     select product);
        
        return query.ToList();
    }
    
    public IEnumerable<Product> GetProducts()
    {
        return _repository.GetAll();
    }
}