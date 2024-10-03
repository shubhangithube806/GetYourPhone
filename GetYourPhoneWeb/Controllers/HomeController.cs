using Dapper;
using GetYourPhoneWeb.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Xml.Linq;

namespace GetYourPhoneWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private SqlConnection conn;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM Product ORDER BY CreatedOn;";
            IEnumerable<Product> productList = conn.Query<Product>(query);

            return View(productList);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductId = Guid.NewGuid();
                product.CreatedOn = DateTime.Now;

                string query = $"INSERT INTO Product (ProductId, Name, Description, Price, CreatedOn) VALUES ('{product.ProductId}', '{product.Name}', '{product.Description}', '{product.Price}', '{product.CreatedOn}');";
                conn.Execute(query);

                TempData["success"] = "Product added successfully.";

                return RedirectToAction("Index");
            }

            return View(product);
        }


        [HttpGet]
        public IActionResult Edit(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            string query = $"SELECT * FROM Product WHERE ProductId = '{id}';";
            Product? product = conn.QueryFirstOrDefault<Product>(query);

            if (product == null)
            {
                return NotFound();   
            }

            return View(product);
        }


        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                string query = $"UPDATE Product SET Name = '{product.Name}', Description = '{product.Description}', Price = '{product.Price}', CreatedOn = '{product.CreatedOn}' WHERE ProductId = '{product.ProductId}';";
                conn.Execute(query);

                TempData["success"] = "Product updated successfully.";

                return RedirectToAction("Index");
            }

            return View(product); 
        }


        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string query = $"SELECT * FROM Product WHERE ProductId = '{id}';";
            Product? product = conn.QueryFirstOrDefault<Product>(query);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string query = $"DELETE FROM Product WHERE ProductId = '{id}';";
            conn.Execute(query);

            TempData["success"] = "Product deleted successfully.";

            return RedirectToAction("Index");
        }


        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
