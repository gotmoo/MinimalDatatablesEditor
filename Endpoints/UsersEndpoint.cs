using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class UsersEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// Users controller
        /// </summary>

        app.MapMethods("/api/users", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                // there is not a "FromForm" option for Minimal API. This is a quick a dirty workaround.
                int site = -1;
                try
                {
                    request.Form.TryGetValue("site", out var siteString);
                    int.TryParse(siteString, out site);
                }
                catch (InvalidOperationException)
                {
                }


                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<JoinModelUsers>("users")
                    .Model<JoinModelSites>("sites")
                    .Field(new Field("users.site")
                        .Options(new Options()
                            .Table("sites")
                            .Value("id")
                            .Label("name")
                        )
                        .Validator(Validation.DbValues(new ValidationOpts {Empty = false}))
                    )
                    .LeftJoin("sites", "sites.id", "=", "users.site")
                    .Where("site", site)
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}