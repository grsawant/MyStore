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
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM products WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id",id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								productInfo.id = "" + reader.GetInt32(0);
								productInfo.name = reader.GetString(1);
								productInfo.description = reader.GetString(2);
								productInfo.created_at = reader.GetDateTime(3).ToString();
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
		}
		public void OnPost()
		{
			productInfo.id = Request.Form["id"];
			productInfo.name = Request.Form["name"];
			productInfo.description = Request.Form["description"];

			if (productInfo.id.Length == 0 || productInfo.name.Length == 0 || 
				productInfo.description.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE products " +
						"SET name=@name, description=@description " +
						"WHERE id=@id";


					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id",productInfo.id);
						command.Parameters.AddWithValue("@name",productInfo.name);
						command.Parameters.AddWithValue("@description",productInfo.description);
							
						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			Response.Redirect("/Products/Index");
		}
    }
}
