using Microsoft.AspNetCore.Mvc;
using Woldrich.CosmosData;
using Woldrich.CosmosModel;

namespace Woldrich.CosmosService.Controllers;

[ApiController]
[Route("/woldrich/initDb")]
public class InitDatabaseController : ControllerBase
{
    [HttpDelete]
    public async Task ResetDatabaseAsync() {
        await CosmosDataContext.ResetDatabase();
    }
}