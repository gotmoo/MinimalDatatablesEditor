using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class SearchBuilderEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This Controller is used to demonstrate SearchBuilder use through Server Side Processing
        /// </summary>

        app.MapMethods("/api/searchBuilder", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<UploadManyModel>()
                    .Field(new Field("users.first_name")
                        .SearchBuilderOptions(new SearchBuilderOptions())
                    )
                    .Field(new Field("users.last_name")
                        .SearchBuilderOptions(new SearchBuilderOptions())
                    )
                    .Field(new Field("users.phone")
                        .SearchBuilderOptions(new SearchBuilderOptions()
                            .Table("users")
                            .Value("phone")
                        )
                    )
                    .Field(new Field("sites.name")
                        .SearchBuilderOptions(new SearchBuilderOptions()
                            .Label("sites.name")
                            .Value("sites.name")
                            .LeftJoin("sites", "sites.id", "=", "users.site")
                        )
                    )
                    .Field(new Field("users.site")
                        .Options(new Options()
                            .Table("sites")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .LeftJoin("sites", "sites.id", "=", "users.site")
                    .Debug(true)
                    .TryCatch(false)
                    .Process(request)
                    .Data();
                return JsonConvert.SerializeObject(response);
            });
    }
}