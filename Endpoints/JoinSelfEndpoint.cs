using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class JoinSelfEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// As with SQL, Editor joins can be self referencing using the `as` option
        /// to alias a table name. This example shows the `users` database table
        /// being used to get a list of all users, but each user also has a manager
        /// who is also a user! The `LeftJoin()` is used to create this relationship.
        ///
        /// Note also the `Options()` method for the `users.manager` field which
        /// uses a delegate to get the list of options for the manager drop down
        /// list on the client-side.
        /// </summary>
        app.MapMethods("/api/joinSelf", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<JoinSelfModel>()
                    .Field(new Field("users.manager")
                        .Options(new Options()
                            .Table("users")
                            .Value("id")
                            .Label(new[] {"first_name", "last_name"})
                        )
                    )
                    .LeftJoin("users as manager", "users.manager", "=", "manager.id")
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}