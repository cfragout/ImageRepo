using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ImageRepoBackend.Controllers
{
    public class ImagenesController : ApiController
    {
        private ImageRepoEntities db = new ImageRepoEntities();

        // GET api/Imagenes
        public IEnumerable<Imagen> GetImagens()
        {
            return db.Imagens.AsEnumerable();
        }

        // GET api/Imagenes/5
        public Imagen GetImagen(int id)
        {
            Imagen imagen = db.Imagens.Find(id);
            if (imagen == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return imagen;
        }

        // PUT api/Imagenes/5
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

        // POST api/Imagenes
        public HttpResponseMessage PostImagen(Imagen imagen)
        {
            if (ModelState.IsValid)
            {
                db.Imagens.Add(imagen);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, imagen);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = imagen.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Imagenes/5
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
    }
}