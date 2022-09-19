using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zen.Mvc;

namespace ZenAchitecture.WebUI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ZenbotController : ZenController
    {


        [HttpPost]
        [Route((nameof(jira)))]
        public async Task jira()
        {
            /// should retrive the data posted by jira
            /// 
            
        }


    }
}
