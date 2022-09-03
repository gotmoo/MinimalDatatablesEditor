using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class UploadManyEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This is controller is used by the majority of Editor examples as it
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

        app.MapMethods("/api/upload-many", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var response = new Editor(db, "users")
                    .Model<UploadManyModel>()
                    .Field(new Field("users.site")
                        .Options(new Options()
                            .Table("sites")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .LeftJoin("sites", "sites.id", "=", "users.site")
                    .MJoin(new MJoin("files")
                        .Link("users.id", "users_files.user_id")
                        .Link("files.id", "users_files.file_id")
                        .Field(
                            new Field("id")
                                .Upload(new Upload(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads",
                                        "__ID____EXTN__"))
                                    .Db("files", "id", new Dictionary<string, object>
                                    {
                                        {
                                            "web_path",
                                            Path.DirectorySeparatorChar + Path.Combine("uploads", "__ID____EXTN__")
                                        },
                                        {"system_path", Upload.DbType.SystemPath},
                                        {"filename", Upload.DbType.FileName},
                                        {"filesize", Upload.DbType.FileSize}
                                    })
                                    .Validator(Validation.FileSize(500000, "Max file size is 500K."))
                                    .Validator(Validation.FileExtensions(new[] {"jpg", "png", "gif"},
                                        "Please upload an image."))
                                )
                        )
                    )
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}