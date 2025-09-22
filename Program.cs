using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add SQL connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var app = builder.Build();

app.MapGet("/", () => "SQL Server Minimal API is running 🚀");

// Example endpoint: GET /customers/1
app.MapGet("/customers/{id:int}", async (int id) =>
{
    using var conn = new SqlConnection(connectionString);
    await conn.OpenAsync();

    using var cmd = new SqlCommand("GetCustomerById", conn);
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@CustomerId", id);

    using var reader = await cmd.ExecuteReaderAsync();
    var results = new List<object>();

    while (await reader.ReadAsync())
    {
        results.Add(new
        {
            CustomerID = reader["CustomerID"],
            FirstName = reader["FirstName"],
            MiddleName = reader["MiddleName"],
            LastName = reader["LastName"],
            Email = reader["EmailAddress"]
        });
    }

    return Results.Ok(results);
});

await app.RunAsync();
