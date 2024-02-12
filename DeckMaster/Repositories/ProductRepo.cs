using DeckMaster.Data;
using DeckMaster.ViewModels;
using DeckMaster.Models;

namespace DeckMaster.Repositories
{
    public class ProductRepo
    {

        private readonly ApplicationDbContext _db;

        public ProductRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<ProductVM> GetProductVMs()
        {
            var products = _db.Products.Select(p => new ProductVM
            {
                ID = p.ID,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = decimal.Parse(p.Price),
                Currency = p.Currency,
                ImageName = p.ImageName
            }).ToList();

            return products;
        }



        //     public List<UserVM> GetAllUsers()
        //{
        //    var users = _db.Users.Select(u => new UserVM { Email = u.Email }).ToList();

        //    return users;
        //}

        public ProductVM GetProduct(int id)
        {
            var product = _db.Products.Find(id);
            return new ProductVM
            {
                ID = product.ID,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = decimal.Parse(product.Price),
                Currency = product.Currency,
                ImageName = product.ImageName
            };
        }
    }
}
