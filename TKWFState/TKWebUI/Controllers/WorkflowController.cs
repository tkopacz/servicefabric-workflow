using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ApplicationInsights;
using TKWorkflow.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TKWebUI.Controllers
{
    public class WorkflowController : Controller
    {
        const string SERVICE_URI = "fabric:/TKWFState";
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
            Response.Cookies.Append("ACTORID", actorid);
            return View();
        }

        public IActionResult Name()
        {
            return View(new Model.WFText() { Text = "Name1" }); //Khem
        }

        private ActorId getActorId()
        {
            var cookie = Request.Cookies["ACTORID"];
            long id;
            if (!long.TryParse(cookie,out id))
            {
                throw new ApplicationException("Cookie ACTORID not found/invalid");
            }
            return new ActorId(id);
        }
        [HttpPost]
        public IActionResult NameSend(Model.WFText model)
        {
            var instance = ActorProxy.Create<ITKWorkflow>(getActorId(), SERVICE_URI);
            m_tc.MeasureTime("ActorCall-SetName", async () =>
            {
                await instance.SetName(model.Text);
            });
            return RedirectToAction("Surname");
        }
        public IActionResult Surname()
        {
            return View(new Model.WFText() { Text = "Surname1" }); //Khem
        }
        [HttpPost]
        public IActionResult SurnameSend(Model.WFText model)
        {
            m_tc.MeasureTime("ActorCallProxy-SetSurname", async () =>
            {
                var instance = ActorProxy.Create<ITKWorkflow>(getActorId(), SERVICE_URI);
                await instance.SetSurname(model.Text);
            });
            return RedirectToAction("Comment");
        }
        public IActionResult Comment()
        {
            return View(new Model.WFComment() { Text = "Comment1" }); //Khem
        }
        [HttpPost]
        public IActionResult CommentSend(Model.WFComment model)
        {
            var ts = DateTime.Now;
            var instance = ActorProxy.Create<ITKWorkflow>(getActorId(), SERVICE_URI);
            m_tc.MeasureTime("ActorCall-AddNewComment", () =>
            {
                instance.AddNewComment(model.Text, ts).Wait();
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
                m_tc.MeasureTime("ActorCall-IsMoreComments", () =>
                {
                    instance.IsMoreComments(false).Wait();
                });
            }
            return RedirectToAction("Summary");
        }
        public async Task<IActionResult> Summary()
        {
            var instance = ActorProxy.Create<ITKWorkflow>(getActorId(), SERVICE_URI); ;
            var state = await instance.GetCurrentState();
            return View(state);
        }

    }
}
