using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ThunderServer.Infratructure.Storage;
using ThunderServer.Models.Domain;

namespace ThunderServer.API.Endpoints.Folder
{
    public class Create : EndpointBaseAsync.WithRequest<string>.WithActionResult<string>
    {
        private readonly ThunderServerContext _serverContext;

        //https://apiendpoints.ardalis.com/getting-started/patterns-used.html
        //USE THIS VIDEO FOR INFO https://www.youtube.com/watch?v=SDu0MA6TmuM
        public Create(ThunderServerContext serverContext)
        {
            this._serverContext = serverContext;
        }

        [HttpPost($"/api/{nameof(ThunderFolder)}/Create")]
        [SwaggerOperation(
            Summary = "Creates a new Author",
            Description = "Creates a new Author",
            OperationId = "Author_Create",
            Tags = [$"{nameof(ThunderFolder)}Endpoint"])]
        public override async Task<ActionResult<string>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            await this._serverContext.AddAsync(request, cancellationToken);
            await this._serverContext.SaveChangesAsync(cancellationToken);

            return Created();
        }
    }
}
