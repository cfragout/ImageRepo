using System;
using System.Collections.Generic;
using System.IO;
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
        public void UploadPcImage()
        {
            HttpPostedFileBase file = Request.Files["pcFile"];
            if (file != null)
            {
                string imageDirPath = "Content/Images/";

                string pic = getLocalFileName(Path.GetFileName(file.FileName), Request.Form["imageName"]);
                string path = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imageDirPath, pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            // after successfully uploading redirect the user
            return;
        }

        // TODO: This should go in a static class
        private string getLocalFileName(string filename, string imageName)
        {
            string username = "CFR";
            string datetime = DateTime.Today.ToString();
            datetime = datetime.Replace('/', '_').Replace('.', '_').Replace(':', '_').Replace(' ', '_').Replace('?', '_');
            return username + "_" + imageName + "_" + datetime + Path.GetExtension(filename);
        }
    }
}
