using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using MinimalDatatablesEditor.Endpoints.Internal;
using MySql.Data.MySqlClient;
using Npgsql;

// Edit launchSettings.json with the correct database server and connection string
// Populate your database with the SQL script located here:
// https://editor.datatables.net/manual/net/core#Examples-SQL
var dbType = Environment.GetEnvironmentVariable("DBTYPE");
switch (dbType)
{
    case "sqlserver":
        DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
        break;
    case "mysql":
        DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
        break;
    case "postgres":
        DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);
        break;
    case "sqlite":
        DbProviderFactories.RegisterFactory("Microsoft.Data.Sqlite", SqliteFactory.Instance);
        break;
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapGet("/", () => Results.Redirect("/examples/index.html"));
//Register api Endpoints
app.UseEndpoints<Program>();
app.Run();