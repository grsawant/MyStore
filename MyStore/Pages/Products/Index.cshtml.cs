using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Products
{
    public class IndexModel : PageModel
    {
	    public List<ProductInfo> listProducts = new List<ProductInfo>();
        public void OnGet()
        {
		try
		{
			String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM products";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							ProductInfo productInfo = new ProductInfo();
							productInfo.id = "" + reader.GetInt32(0);
							productInfo.name = reader.GetString(1);
							productInfo.description = reader.GetString(2);
							productInfo.created_at = reader.GetString(3);

							listProducts.Add(productInfo);
						}
					}
				}

			}
		}

		catch (Exception ex)
		{
				Console.WriteLine("Exception: " + ex.ToString());
		}
        }
    }

    public class ProductInfo
    {
	    public String id;
	    public String name;
	    public String description;
	    public String created_at;
    }

}
