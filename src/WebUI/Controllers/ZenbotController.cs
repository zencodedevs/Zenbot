using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zen.Mvc;
using Zenbot.Services.Jira.Model;

namespace ZenAchitecture.WebUI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ZenbotController : ZenController
    {


        [HttpPost]
        [Route((nameof(jira)))]
        public async Task jira(JiraPost model)
        {

            /// should retrive the data posted by jira
            /// 
            
        }


    }
}
