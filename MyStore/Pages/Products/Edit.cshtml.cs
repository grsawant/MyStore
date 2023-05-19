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
				String sql = "SELECT * FROM products where id=@id";
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
							productInfo.created_at = reader.GetString(3);
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

	public void OnPost()
	{
		productInfo.id = Request.Form["id"];
		productInfo.name = Request.Form["name"];
		productInfo.description = Request.Form["description"];
		productInfo.id = Request.Form["id"];

		if (productInfo.id.Length == 0 || productInfo.name.Length == 0 || 
				productInfo.description == 0)
		{
			errormessage = "All fields are required";
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
					command.Parameters.AddWithValue("@name",productInfo.name);
					command.Parameters.AddWithValue("@description",productInfo.description);

					command.ExecuteNonQuery();
				}
			}
		}
		catch
		{
			errorMessage = ex.Message;
			return;
		}

		Response.Redirect("/Products/Index");
    }
}
