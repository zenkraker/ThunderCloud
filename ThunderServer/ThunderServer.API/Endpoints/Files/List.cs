using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ThunderServer.API.Domain;
using ThunderServer.API.Storage;

namespace ThunderServer.API.Endpoints.Files
{
    public class List : EndpointBaseAsync.WithoutRequest.WithActionResult<List<ThunderFile>>
    {
        private readonly ThunderServerContext _serverContext;

        //https://apiendpoints.ardalis.com/getting-started/patterns-used.html
        //USE THIS VIDEO FOR INFO https://www.youtube.com/watch?v=SDu0MA6TmuM
        public List(ThunderServerContext serverContext)
        {
            this._serverContext = serverContext;
        }

        [HttpGet($"/api/{nameof(ThunderFile)}/List")]
        [SwaggerOperation(
           Summary = "Creates a new Author",
           Description = "Creates a new Author",
           OperationId = "Author_Create",
           Tags = [$"{nameof(ThunderFile)}Endpoint"])]
        public override Task<ActionResult<List<ThunderFile>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
