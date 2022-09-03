using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class CheckboxEndpoint:IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMethods("/api/checkbox", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {   
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<CheckboxModel>()
                    .Field(new Field("image")
                        .SetFormatter((val, data) => (string)val == "" ? 0 : 1)
                    )

                    .Data();

                return JsonConvert.SerializeObject(response);

            });
    }
}