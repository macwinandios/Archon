using Archon.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Archon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        //Created an empty webapi

        //Archon.WebApi/Properties/launchSettings.json is where the launch configs are. I added "launchUrl": "api/my", to the "$schema": packet. Now it defaults to this url instead of swagger.

        //ctrl + f5 or Menu/Debug/"Start w/o debugging" allows you to see changes reflected w/o stopping app. Just refresh.

        //Archon.WebApi/appsettings.json is used to configure APP LEVEL settings like CONNECTION STRINGS.



        //to get all
        //then in iis it is localhost:18029/api/my
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Jacksonville", "Miami", "St Augustine" };
        }



        //to get by id
        //then in iis it is localhost:18029/api/my/1
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Jax";
        }

    }
}
