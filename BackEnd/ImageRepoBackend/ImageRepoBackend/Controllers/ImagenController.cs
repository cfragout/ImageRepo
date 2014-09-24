using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ImageRepoBackend.Controllers
{
    public class ImagenController : ApiController
    {
        private ImageRepoEntities db = new ImageRepoEntities();

        // GET api/Imagen
        public IEnumerable<Imagen> GetImagens()
        {
            return db.Imagens.AsEnumerable();
        }

        // GET api/Imagen/5
        public Imagen GetImagen(int id)
        {
            Imagen imagen = db.Imagens.Find(id);
            if (imagen == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return imagen;
        }

        // PUT api/Imagen/5
        public HttpResponseMessage PutImagen(int id, Imagen imagen)
        {
            if (ModelState.IsValid && id == imagen.id)
            {
                db.Entry(imagen).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Imagen
        public HttpResponseMessage PostImagen(Imagen imagen)
        {
            if (ModelState.IsValid)
            {

                WebClient webClient = new WebClient();

                string remoteFileUrl = imagen.originalURL;
                string serverUrl = "http://localhost:53079/Content/Images/";
                string localFilePath = getLocalFilePath() + getLocalFileName(imagen);

                imagen.path = serverUrl + getLocalFileName(imagen);
                imagen.datetime = DateTime.Today;
                imagen.isDeleted = false;

                try
                {
                    if (imagen.userUploaded == false)
                    {
                        webClient.DownloadFile(remoteFileUrl, localFilePath);
                    }

                    db.Imagens.Add(imagen);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, imagen);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = imagen.id }));
                    return response;
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Imagen/5
        public HttpResponseMessage DeleteImagen(int id)
        {
            Imagen imagen = db.Imagens.Find(id);
            if (imagen == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Imagens.Remove(imagen);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, imagen);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private string getLocalFilePath()
        {
            string imageDirPath = "Content/Images/";
            return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imageDirPath;
        }

        private string getLocalFileName(Imagen imagen)
        {
            string username = "CFR";
            string datetime = DateTime.Today.ToString();
            datetime = datetime.Replace('/', '_').Replace('.', '_').Replace(':', '_').Replace(' ','_').Replace('?', '_');
            return username + "_" + imagen.name + "_" + datetime + Path.GetExtension(imagen.originalURL);
        }
    }
}