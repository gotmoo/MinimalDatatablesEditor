using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class JoinLinkTableEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example shows how multiple `LeftJoin()` statements can be used to
        /// reference data from multiple database tables. In this case the `user_dept`
        /// table is a "link table" that references both the user and dept tables
        /// to create a reference (a link) between them.
        /// </summary>

        app.MapMethods("/api/joinLinkTable", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<JoinLinkTableModel>()
                    .Field(new Field("users.site")
                        .Options(new Options()
                            .Table("sites")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .Field(new Field("user_dept.dept_id")
                        .Options(new Options()
                            .Table("dept")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .LeftJoin("sites", "sites.id", "=", "users.site")
                    .LeftJoin("user_dept", "users.id", "=", "user_dept.user_id")
                    .LeftJoin("dept", "user_dept.dept_id", "=", "dept.id")
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}