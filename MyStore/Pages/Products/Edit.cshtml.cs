using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Products
{
    public class EditModel : PageModel
    {
	    public ProductInfo productInfo = new ProductInfo();
	    public String errorMessage = "";
	    public String successMessage = "";

        public void OnGet()
        {
		String id = Request.Query["id"];
		try
		{
			String connectionString = "";
			using (Sqlconnection connection = new Sqlconnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM products where id=@id";
				using (Sqlcommand command = new Sqlcommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id",id);
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
		catch(Exception ex)
		{
			errorMessage=ex.Message;
			return;
		}

		Response.Redirect("/Products/Index");
        }
    }
}
