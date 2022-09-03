using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class CountriesEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example shows how cascading lists can be created.
        /// </summary>

        app.MapMethods("/api/countries", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                var values = request.Form["values[team.continent]"];
                if (values.Count == 0 || values[0].Length == 0) {
                    var result = new Dictionary<string, object>();
                    return JsonConvert.SerializeObject(result);
                }

                using var db = new Database(dbType, dbConnection);
                {
                    var query = db.Select(
                        "country",
                        new[] {"id as value", "name as label"},
                        new Dictionary<string, dynamic>(){{"continent", request.Form["values[team.continent]"]}}
                    );
         
                    var result = new Dictionary<string, object>();
                    var options = new Dictionary<string, object>();
                    options["team.country"] = query.FetchAll();
                    result["options"] = options;

                    return JsonConvert.SerializeObject(result);
                }
            });
    }
}