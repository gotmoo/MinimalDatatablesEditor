using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public  class StaffEndpoint : IEndpoints 
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This controller is used by the majority of Editor examples as it
        /// provides a nice rounded set of information for the client-side Editor
        /// Javascript library to show its capabilities.
        ///
        /// In the code here, note that the `StaffModel` is used as the model for
        /// the Editor, which automatically defines the database fields to be read.
        /// Additional instructions can be given for each field by creating a `Field`
        /// instance for it - many of the fields have validation methods applied here
        /// and the date field has a formatter to make it readable to users looking
        /// at the table!
        /// </summary>

        app.MapMethods("/api/staff", new[] {"POST", "GET" },
            (HttpRequest request) => {  
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION"); 
                using var db = new Database("sqlserver", dbConnection);
                var response = new Editor(db, "datatables_demo")
                    .Model<StaffModel>()
                    .Field(new Field("first_name")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("last_name"))
                    .Field(new Field("extn")
                        .Validator(Validation.Numeric())
                    )
                    .Field(new Field("age")
                        .Validator(Validation.Numeric())
                        .SetFormatter(Format.IfEmpty(null))
                    )
                    .Field(new Field("salary")
                        .Validator(Validation.Numeric())
                        .SetFormatter(Format.IfEmpty(null))
                    )
                    .Field(new Field("start_date")
                        .Validator(Validation.DateFormat(
                            Format.DATE_ISO_8601,
                            new ValidationOpts {Message = "Please enter a date in the format yyyy-mm-dd"}
                        ))
                        .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                        .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .TryCatch(false)
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response); 
            });

    }

    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        
    }
}