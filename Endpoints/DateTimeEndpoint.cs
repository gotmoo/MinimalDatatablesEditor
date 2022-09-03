using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class DateTimeEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example shows how a field can use formatters and validators to
        /// work with date fields - traditionally a fairly complex task, made easy
        /// here.
        ///
        /// Note how the <code>ValidationOpts</code> class is used to specify a
        /// custom error message for the validation methods.
        /// </summary>

        app.MapMethods("/api/datetime", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<DatesModel>()
                    .Field(new Field("updated_date")
                        .Validator(Validation.DateFormat(
                            "MM-dd-yyyy h:mm tt",
                            new ValidationOpts
                                {Message = "Please enter a date and time in the format MM-dd-yyyy h:mm A"}
                        ))
                        .GetFormatter(Format.DateTime("yyyy-MM-dd hh:mm", "MM-dd-yyyy h:mm tt"))
                        .SetFormatter(Format.DateTime("MM-dd-yyyy h:mm tt", "yyyy-MM-dd hh:mm"))
                    )
                    .Field(new Field("registered_date")
                        .Validator(Validation.DateFormat(
                            "d MMM yyyy HH:mm",
                            new ValidationOpts
                                {Message = "Please enter a date and time in the format d MMMM yyyy HH:mm"}
                        ))
                        .GetFormatter(Format.DateTime("yyyy-MM-dd hh:mm", "d MMM yyyy HH:mm"))
                        .SetFormatter(Format.DateTime("d MMM yyyy HH:mm", "yyyy-MM-dd hh:mm"))
                    )
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}