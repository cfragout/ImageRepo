using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ImageRepoBackend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void SaveImageFromUrl(string remoteFileUrl)
        {
            try
            {
                WebClient webClient = new WebClient();
                string localFileName = Server.MapPath("~") + "Content/Images/test.png";
                webClient.DownloadFile(remoteFileUrl, localFileName);
            }
            catch
            { }
        }
    }
}
