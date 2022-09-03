using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class TimeEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMethods("/api/time", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .TryCatch(false)
                    .Model<TimeModel>()
                    .Field(new Field("shift_start")
                        .Validator(Validation.DateFormat(
                            "h:mm tt"
                        ))
                        .GetFormatter(Format.DateTime("HH:mm:ss", "h:mm tt"))
                        .SetFormatter(Format.DateTime("h:mm tt", "HH:mm:ss"))
                    )
                    .Field(new Field("shift_end")
                            .Validator(Validation.DateFormat(
                                "HH:mm:ss"
                            ))
                            .GetFormatter(Format.DateTime("HH:mm:ss"))
                        // No set formatter required, as already in IS0 format
                    )
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}