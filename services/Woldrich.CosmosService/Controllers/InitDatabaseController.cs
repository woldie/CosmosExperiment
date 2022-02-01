using Microsoft.AspNetCore.Mvc;
using Woldrich.CosmosData;
using Woldrich.CosmosModel;

namespace Woldrich.CosmosService.Controllers;

// TODO: Endpoint specific to local development.  Come up with a separate migration scheme that will initialize the Cosmos DB and
//  initialize containers prior to accepting traffic on TEST/PROD

[ApiController]
[Route("/woldrich/initDb")]
public class InitDatabaseController : ControllerBase
{
    [HttpDelete]
    public async Task ResetDatabaseAsync() {
        await CosmosDataContext.ResetDatabase();
    }
}