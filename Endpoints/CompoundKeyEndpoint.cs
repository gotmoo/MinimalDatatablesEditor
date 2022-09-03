using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class CompoundKeyEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMethods("/api/compoundKey", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users_visits", new[] {"user_id", "visit_date"})
                    .TryCatch(false)
                    .Model<CompoundKeyModel>()
                    .Field(new Field("users_visits.user_id")
                        .Options(new Options()
                            .Table("users")
                            .Value("id")
                            .Label(new[] {"first_name", "last_name"})
                        )
                    )
                    .Field(new Field("users_visits.site_id")
                        .Options(new Options()
                            .Table("sites")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .Field(new Field("users_visits.visit_date")
                        .Validator(Validation.DateFormat(
                            Format.DATE_ISO_8601,
                            new ValidationOpts {Message = "Please enter a date in the format yyyy-mm-dd"}
                        ))
                        .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                        .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Field(new Field("sites.name").Set(false))
                    .Field(new Field("users.first_name").Set(false))
                    .Field(new Field("users.last_name").Set(false))
                    .LeftJoin("sites", "users_visits.site_id", "=", "sites.id")
                    .LeftJoin("users", "users_visits.user_id", "=", "users.id")
                    .Validator((editor, type, args) =>
                    {
                        if (type == DtRequest.RequestTypes.EditorCreate)
                        {
                            // Detect duplicates on create
                            foreach (var d in args.Data)
                            {
                                var userVisits = d.Value as Dictionary<string, object>;
                                var submitUserVisits = userVisits["users_visits"] as Dictionary<string, object>;

                                var any = editor.Db().Any("users_visits", (q) =>
                                {
                                    q.Where("user_id", submitUserVisits["user_id"]);
                                    q.Where("visit_date", submitUserVisits["visit_date"]);
                                });

                                if (any)
                                {
                                    return "This staff member is already busy that day.";
                                }
                            }
                        }
                        else if (type == DtRequest.RequestTypes.EditorEdit)
                        {
                            // Detect duplicates on edit
                            foreach (var d in args.Data)
                            {
                                var pkey = editor.PkeyToArray(d.Key);
                                var keyUserVisits = pkey["users_visits"] as Dictionary<string, object>;
                                var userVisits = d.Value as Dictionary<string, object>;
                                var submitUserVisits = userVisits["users_visits"] as Dictionary<string, object>;

                                // Discount the row being edited
                                if (keyUserVisits["user_id"] != submitUserVisits["user_id"] &&
                                    keyUserVisits["visit_date"] != submitUserVisits["visit_date"])
                                {
                                    var any = editor.Db().Any("users_visits", (q) =>
                                    {
                                        q.Where("user_id", submitUserVisits["user_id"]);
                                        q.Where("visit_date", submitUserVisits["visit_date"]);
                                    });

                                    if (any)
                                    {
                                        return "This staff member is already busy that day.";
                                    }
                                }
                            }
                        }

                        return null;
                    })
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}