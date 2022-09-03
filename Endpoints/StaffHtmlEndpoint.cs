using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class StaffHtmlEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example is used only for the DOM sourced table example where the
        /// initial table is read from HTML rather than by Ajax. A `GetFormatter`
        /// is used for the `salary` field to convert the data into the currency
        /// format for the end user to view.
        /// </summary>
        app.MapMethods("/api/staff-html", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "datatables_demo")
                    .Model<StaffModel>()
                    .Field(new Field("first_name").Validator(Validation.NotEmpty()))
                    .Field(new Field("last_name").Validator(Validation.NotEmpty()))
                    .Field(new Field("salary")
                        .Validator(Validation.Numeric())
                        .GetFormatter((val, row) => "$" + ((Int32) val).ToString("N0"))
                    )
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}