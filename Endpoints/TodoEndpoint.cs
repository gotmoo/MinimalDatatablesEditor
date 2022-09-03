using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class TodoEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This controller is used by the "To Do" examples on the client-side to
        /// show how Editor can display fields of different types.
        /// </summary>

        app.MapMethods("/api/todo", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "todo")
                    .Model<ToDoModel>()
                    .Field(new Field("done").Validator(Validation.Boolean()))
                    .Field(new Field("priority").Validator(Validation.Numeric()))
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);

            });
    }
}