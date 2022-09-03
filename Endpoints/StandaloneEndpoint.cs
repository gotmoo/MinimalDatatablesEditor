using MinimalDatatablesEditor.Endpoints.Internal;
using Newtonsoft.Json;

namespace MinimalDatatablesEditor.Endpoints;

public class StandaloneEndpoint : IEndpoints
{
    public static void DefineEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// This example is used for the standalone examples. They don't actually
        /// save to the database as the behaviour of such an edit will depend highly
        /// upon the structure of your database. However using the Database methods
        /// such as update() this can easily be done.
        ///
        /// See the .NET manual - http://editor.datatables.net/manual/net - for
        /// information on how to use the Editor .NET libraries.
        /// </summary>

        app.MapMethods("/api/standalone", new[] {"POST", "GET"},
            () => JsonConvert.SerializeObject(new Object()));
    }
}