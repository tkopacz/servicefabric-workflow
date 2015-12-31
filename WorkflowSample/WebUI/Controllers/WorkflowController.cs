using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.ServiceFabric.Actors;
using WorkflowStateHost.Interfaces;
using Microsoft.ApplicationInsights;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class WorkflowController : Controller
    {
        const string SERVICE_URI = "fabric:/WorkflowSample";
        private readonly TelemetryClient m_tc;
        // GET: /<controller>/
        public WorkflowController(TelemetryClient tc)
        {
            this.m_tc = tc;
        }
        public IActionResult Index()
        {
            var instance = ActorProxy.Create<ITKWorkflow>(ActorId.NewId(), SERVICE_URI);
            string actorid="";
            m_tc.MeasureTime("ActorGetActorId", () =>
            {
                actorid = instance.GetActorId().GetLongId().ToString();
            });
            //Response.Cookies.Append("ACTORID", actorid);
            Response.Headers.Add("ACTORID", actorid);
            return View();
        }

        public IActionResult Name()
        {
            return View(new Model.WFText() { Text = "Name1" }); //Khem
        }

        [HttpPost]
        public IActionResult NameSend(Model.WFText model)
        {
            //var cookie = Request.Cookies["ACTORID"];
            
            var id = long.Parse(Request.Headers["ACTORID"].ToString());
            ITKWorkflow instance = null;
            m_tc.MeasureTime("ActorCallProxy-SetName", async () =>
            {
                instance = ActorProxy.Create<ITKWorkflow>(new ActorId(id), SERVICE_URI);
                await instance.SetName(model.Text);
            });
            return RedirectToAction("Surname");
        }
        public IActionResult Surname()
        {
            return View(new Model.WFText() { Text = "Surname1" }); //Khem
        }
        [HttpPost]
        public async Task<IActionResult> SurnameSend(Model.WFText model)
        {
            var cookie = Request.Cookies["ACTORID"];
            var id = long.Parse(cookie[0]);
            m_tc.MeasureTime("ActorCallProxy-SetSurname", async () =>
            {
                var instance = ActorProxy.Create<ITKWorkflow>(new ActorId(id), SERVICE_URI);
                await instance.SetSurname(model.Text);
            });
            return RedirectToAction("Comment");
        }
        public IActionResult Comment()
        {
            return View(new Model.WFComment() { Text = "Comment1" }); //Khem
        }
        [HttpPost]
        public async Task<IActionResult> CommentSend(Model.WFComment model)
        {
            var cookie = Request.Cookies["ACTORID"];
            var id = long.Parse(cookie[0]);
            var ts = DateTime.Now;
            var instance = ActorProxy.Create<ITKWorkflow>(new ActorId(id), SERVICE_URI);
            m_tc.MeasureTime("ActorCall-AddNewComment", async () =>
            {
                await instance.AddNewComment(model.Text, ts);
            });
            if (model.NextComment)
            {
                m_tc.MeasureTime("ActorCall-IsMoreComments", async () =>
                {
                    await instance.IsMoreComments(true);
                });
                return RedirectToAction("Comment");
            }
            else {
                m_tc.MeasureTime("ActorCall-IsMoreComments", async () =>
                {
                    await instance.IsMoreComments(false);
                });
            }
            return RedirectToAction("Summary");
        }
        public async Task<IActionResult> Summary()
        {
            var cookie = Request.Cookies["ACTORID"];
            var id = long.Parse(cookie[0]);
            var instance = ActorProxy.Create<ITKWorkflow>(new ActorId(id), SERVICE_URI);
            var state = await instance.GetCurrentState();
            return View(state);
        }

    }
}
