using Microsoft.Data.SqlClient;
using System.Data;
using SqlApi.Data;

var builder = WebApplication.CreateBuilder(args);

// The configuration automatically includes user secrets in development
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Replace the placeholder with the actual secret value
connectionString = connectionString?.Replace("{DbPassword}", 
    builder.Configuration["DbPassword"]);


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
