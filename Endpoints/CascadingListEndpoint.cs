using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class CascadingListEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example shows how multiple `LeftJoin()` statements can be used to
        /// reference data from multiple database tables. In this case the `user_dept`
        /// table is a "link table" that references both the user and dept tables
        /// to create a reference (a link) between them.
        /// </summary>
        app.MapMethods("/api/cascadingLists", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "team")
                    .Model<CascadingListsModel>()
                    .Field(new Field("team.continent")
                        .Options(new Options()
                            .Table("continent")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .Field(new Field("team.country")
                        .Options(new Options()
                            .Table("country")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .LeftJoin("continent", "continent.id", "=", "team.continent")
                    .LeftJoin("country", "country.id", "=", "team.country")
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}