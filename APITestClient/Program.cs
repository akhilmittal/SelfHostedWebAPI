#region Using namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using SelfHostedAPI; 
#endregion

namespace APITestClient
{
    internal class Program
    {
        #region Private member variables
        private static readonly HttpClient Client = new HttpClient(); 
        #endregion

        #region Main method for execution entry
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            Client.BaseAddress = new Uri("http://localhost:8082");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            GetAllProducts();
            GetProduct();
            AddProduct();
            EditProduct();
            DeleteProduct();
            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();
        } 
        #endregion

        #region Private client methods
        /// <summary>
        /// Fetch all products
        /// </summary>
        private static void GetAllProducts()
        {
            HttpResponseMessage resp = Client.GetAsync("api/product").Result;
            resp.EnsureSuccessStatusCode();

            var products = resp.Content.ReadAsAsync<IEnumerable<SelfHostedAPI.Product>>().Result.ToList();
            if (products.Any())
            {
                Console.WriteLine("Displaying all the products...");
                foreach (var p in products)
                {
                    Console.WriteLine("{0} {1} ", p.ProductId, p.ProductName);
                }
            }
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        private static void GetProduct()
        {
            const int id = 1;
            var resp = Client.GetAsync(string.Format("api/product/{0}", id)).Result;
            resp.EnsureSuccessStatusCode();

            var product = resp.Content.ReadAsAsync<SelfHostedAPI.Product>().Result;
            Console.WriteLine("Displaying product having id : " + id);
            Console.WriteLine("ID {0}: {1}", id, product.ProductName);
        }

        /// <summary>
        /// Add product
        /// </summary>
        private static void AddProduct()
        {
            var newProduct = new Product() { ProductName = "IPad" };
            var response = Client.PostAsJsonAsync("api/product", newProduct);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine("Product added.");
            }
        }

        /// <summary>
        /// Edit product 
        /// </summary>
        private static void EditProduct()
        {
            const int productToEdit = 4;
            var product = new Product() { ProductName = "Xamarin" };

            var response =
                Client.PutAsJsonAsync("api/product/" + productToEdit, product);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine("Product edited.");
            }

        }

        /// <summary>
        /// Delete product
        /// </summary>
        private static void DeleteProduct()
        {
            const int productToDelete = 2;
            var response = Client.DeleteAsync("api/product/" + productToDelete);
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine("Product deleted.");
            }
        }

        #endregion
    }
}
