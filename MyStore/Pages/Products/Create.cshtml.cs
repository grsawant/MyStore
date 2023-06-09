using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Products
{
    public class CreateModel : PageModel
    {
	public ProductInfo productInfo = new ProductInfo();
	public String errorMessage = "";
	public String successMessage = "";
        public void OnGet()
        {
        }

		public void OnPost()
		{
			productInfo.name = Request.Form["name"];
			productInfo.description = Request.Form["description"];
	
			if(productInfo.name.Length == 0 || productInfo.description.Length == 0)
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
					String sql = "INSERT INTO products " +
						"(name, description) VALUES " +
						"(@name, @description);";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name",productInfo.name);
						command.Parameters.AddWithValue("@description",productInfo.description);

						command.ExecuteNonQuery();
					}
				}
			}
			catch(Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			productInfo.name = ""; productInfo.description = "";
			successMessage = "New Product Added Correctly";

			Response.Redirect("/Products/Index");
		}
    }
}
