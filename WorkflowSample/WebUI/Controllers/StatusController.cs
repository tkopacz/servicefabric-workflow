using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.ServiceFabric.Actors;
using Statistics.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class StatusController : Controller
    {
        const string  SERVICE_URI = "fabric:/WorkflowSample";
        static readonly ActorId m_actorIdStatistics = new ActorId(0);
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var instance = ActorProxy.Create<IStatistics>(m_actorIdStatistics, SERVICE_URI);
            return View(await instance.GetCountAsync());
        }
    }
}
