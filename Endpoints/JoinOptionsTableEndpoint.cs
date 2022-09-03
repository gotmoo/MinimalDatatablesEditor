using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class JoinOptionsTableEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example shows a very simple join using the `LeftJoin` method.
        /// Of particular note in this example is that the `JoinModel` defines two
        /// nested classes that obtain the data required from the two tables, where
        /// the nested class name defines the database table and the properties of
        /// the nested classes define the columns in each table.
        ///
        /// Note also the use of the `Options()` method for the `users.site` method
        /// which is used to retrieve the information for the `Site` drop down list
        /// on the client-side, which is automatically populated when the data is
        /// loaded.
        /// </summary>

        app.MapMethods("/api/joinOptionsTable", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<JoinModelUsers>("users")
                    .Model<JoinModelSites>("sites")
                    .Field(new Field("users.site")
                        .Options(() =>
                        {
                            return db
                                .Select(
                                    "sites",
                                    new[] {"id", "name", "continent"},
                                    new Dictionary<string, dynamic>()
                                )
                                .FetchAll();
                        })
                        .Validator(Validation.DbValues(
                            new ValidationOpts {Empty = false},
                            "id",
                            "sites"
                        ))
                    )
                    .LeftJoin("sites", "sites.id", "=", "users.site")
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}