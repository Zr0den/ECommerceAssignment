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
            ProductId = 1
        });
        _repository.Add(new Product
        {
            ProductId = 2
        });
        _repository.Add(new Product
        {
            ProductId = 3
        });
    }
    
    public IEnumerable<Product> GetOrderProducts(int[] productIds)
    {
        // DONE
        return _repository.GetAll().Where(x => productIds.Contains(x.ProductId));
    }
    
    public IEnumerable<Product> GetProducts()
    {
        return _repository.GetAll();
    }
}