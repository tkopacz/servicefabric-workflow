using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.ServiceFabric.Actors;
using Statistics.Interfaces;
using Microsoft.ApplicationInsights;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class StatusController : Controller
    {
        const string  SERVICE_URI = "fabric:/WorkflowSample";
        static readonly ActorId m_actorIdStatistics = new ActorId(0);
        private readonly TelemetryClient m_tc;

        public StatusController(TelemetryClient tc)
        {
            this.m_tc = tc;
        }
        public IActionResult Index()
        {
            m_tc.TrackPageView("StatusController - Index");
            IStatistics instance=null;
            m_tc.MeasureTime("ActorProxy", () =>
            {
                instance = ActorProxy.Create<IStatistics>(m_actorIdStatistics, SERVICE_URI);
            });
            StatisticsState result=null;
            m_tc.MeasureTime("ActorCall-GetCountAsync", async () =>
            {
                result = await instance.GetCountAsync();
            });
            return View(result);
        }
    }
}
