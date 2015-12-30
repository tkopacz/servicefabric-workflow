using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.ServiceFabric.Actors;
using WorkflowStateHost.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class WorkflowController : Controller
    {
        const string SERVICE_URI = "fabric:/WorkflowSample";
        // GET: /<controller>/
        public IActionResult Index()
        {
            var instance = ActorProxy.Create<ITKWorkflow>(ActorId.NewId(), SERVICE_URI);
            Response.Cookies.Append("ACTORID", instance.GetActorId().GetLongId().ToString());
            return View();
        }

        public IActionResult Name()
        {
            return View(new Model.WFText() { Text = "Name1" }); //Khem
        }

        [HttpPost]
        public async Task<IActionResult> NameSend(Model.WFText model)
        {
            var cookie = Request.Cookies["ACTORID"];
            var id = long.Parse(cookie[0]);
            var instance = ActorProxy.Create<ITKWorkflow>(new ActorId(id), SERVICE_URI);
            await instance.SetName(model.Text);
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
            var instance = ActorProxy.Create<ITKWorkflow>(new ActorId(id), SERVICE_URI);
            await instance.SetSurname(model.Text);
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
            await instance.AddNewComment(model.Text,ts);
            if (model.NextComment)
            {
                await instance.IsMoreComments(true);
                return RedirectToAction("Comment");
            }
            else {
                await instance.IsMoreComments(false);
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
