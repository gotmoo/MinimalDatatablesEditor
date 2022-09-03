using DataTables;
using MinimalDatatablesEditor.Endpoints.Internal;
using MinimalDatatablesEditor.Models;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class SequenceEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMethods("/api/sequence", new[] {"POST", "GET"},
            (HttpRequest request) =>
            {
                var dbType = Environment.GetEnvironmentVariable("DBTYPE");
                var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

                using var db = new Database(dbType, dbConnection);
                var editor = new Editor(db, "audiobooks")
                    .Model<SequenceModel>()
                    .Field(new Field("title").Validator(Validation.NotEmpty()))
                    .Field(new Field("author").Validator(Validation.NotEmpty()))
                    .Field(new Field("duration").Validator(Validation.Numeric()))
                    .Field(new Field("readingOrder").Validator(Validation.Numeric()));

                editor.PreCreate += (sender, e) => e.Editor.Db()
                    .Query("update", "audiobooks")
                    .Set("readingOrder", "readingOrder+1", false)
                    .Where("readingOrder", e.Values["readingOrder"], ">=")
                    .Exec();

                editor.PreRemove += (sender, e) =>
                {
                    // On remove, the sequence needs to be updated to decrement all rows
                    // beyond the deleted row. Get the current reading order by id (don't
                    // use the submitted value in case of a multi-row delete).
                    var order = e.Editor.Db()
                        .Select("audiobooks", new[] {"readingOrder"}, query => query.Where("id", e.Id))
                        .Fetch();

                    e.Editor.Db()
                        .Query("update", "audiobooks")
                        .Set("readingOrder", "readingOrder-1", false)
                        .Where("readingOrder", order["readingOrder"], ">")
                        .Exec();
                };

                var response = editor
                    .Process(request)
                    .Data();

                return JsonConvert.SerializeObject(response);
            });
    }
}