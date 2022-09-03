using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class SitesNestedEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMethods("/api/sitesNested", new[] {"POST", "GET"},
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
                    .Field(new Field("continent")
                        .Validator(Validation.NotEmpty())
                    )
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}