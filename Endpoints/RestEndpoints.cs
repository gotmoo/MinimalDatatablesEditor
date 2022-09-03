using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class RestEndpoints : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example shows how the Editor libraries can be used in a REST like
        /// environment, where different actions are sent to different URLs and / or
        /// use different HTTP verbs.
        /// 
        /// Note that the Editor instance will automatically detect which request
        /// type is being made, so all of the routes can be passed through to a
        /// single controller method.
        /// </summary>
        ///
        /// 
        string Rest(HttpRequest request)
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using var db = new Database(dbType, dbConnection);
            var response = new Editor(db, "datatables_demo")
                .Model<StaffModel>()
                .Field(new Field("first_name").Validator(Validation.NotEmpty()))
                .Field(new Field("last_name").Validator(Validation.NotEmpty()))
                .Field(new Field("extn").Validator(Validation.Numeric()))
                .Field(new Field("age").Validator(Validation.Numeric()))
                .Field(new Field("salary").Validator(Validation.Numeric()))
                .Field(new Field("start_date")
                    .Validator(Validation.DateFormat(
                        Format.DATE_ISO_8601,
                        new ValidationOpts {Message = "Please enter a date in the format yyyy-mm-dd"}
                    ))
                    .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                    .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                )
                .Process(request)
                .Data();

            return JsonConvert.SerializeObject(response);
        }

        app.MapGet("/api/rest/get", Rest);
        app.MapPost("/api/rest/create", Rest);
        app.MapPut("/api/rest/edit", Rest);
        app.MapDelete("/api/rest/remove", Rest);
    }
}