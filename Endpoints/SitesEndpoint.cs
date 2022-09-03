using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class SitesEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMethods("/api/sites", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "sites")
                    .Field(new Field("id")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("name")
                        .Validator(Validation.NotEmpty())
                    )
                    .MJoin(new MJoin("users")
                        .Link("sites.id", "users.site")
                        .Field(new Field("id"))
                    )
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}