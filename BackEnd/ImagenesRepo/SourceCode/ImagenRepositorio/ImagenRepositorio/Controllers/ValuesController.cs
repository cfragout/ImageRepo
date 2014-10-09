using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ImagenRepositorio.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        // Find a place for this action!
        public IHttpActionResult GetRepoBackup()
        {
            // Zip user directory, not complete images directory
            string userImagesDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Content/images";
            DirectoryInfo directorySelected = new DirectoryInfo(userImagesDirectory);
            string zipFile = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Content/"+ "username_images_date.zip";

            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(userImagesDirectory);
                zip.Comment = "Imagenes de USUARIO " + System.DateTime.Now.ToString("G");
                zip.Save(zipFile);
            }

            // Return url for downloading the zip file
            return Ok("some url");
        }
    }
}
