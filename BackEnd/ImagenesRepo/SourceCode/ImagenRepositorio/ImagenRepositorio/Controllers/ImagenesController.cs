using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ImagenRepoEntities.Models;
using ImagenRepoEntities.Entities;
using ImagenRepoServices.IServices;
using ImagenRepoServices.Services;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace ImagenRepositorio.Controllers
{
    public class ImagenesController : ApiController
    {
        private ModelContainer db = new ModelContainer();

        private IImagenService<Imagen> imagenService;
        

        public ImagenesController(
            IImagenService<Imagen> imagenService)
        {
            this.imagenService = imagenService;
        }

        // GET: api/Imagenes
        public IEnumerable<Imagen> GetImages()
        {
            return this.imagenService.GetAll();
        }

        // GET: api/Imagenes/GetLatestImages
        public IEnumerable<Imagen> GetLatestImages()
        {
            return this.imagenService.GetLatestImagenes();
        }

        // GET: api/Imagenes/5
        [ResponseType(typeof(Imagen))]
        public IHttpActionResult GetImagen(int id)
        {
            Imagen imagen = db.Imagenes.Find(id);
            if (imagen == null)
            {
                return NotFound();
            }

            return Ok(imagen);
        }

        // PUT: api/Imagenes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutImagen(int id, Imagen imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != imagen.Id)
            {
                return BadRequest();
            }

            db.Entry(imagen).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Imagenes/MarkImagesAsFavourite
        public List<int> MarkImagesAsFavourite()
        {
            var request = HttpContext.Current.Request;
            List<int> modifiedImagesIds = new List<int>();

            if (request.Form["imagesIds"] == null)
            {
                return modifiedImagesIds;
            }

            string[] ids = request.Form["imagesIds"].Split(',');

            for (int i = 0; i < ids.Length; i++)
            {
                var id = Convert.ToInt32(ids[i]);
                if (ImagenExists(id))
                {
                    Imagen imagen = db.Imagenes.Find(id);
                    imagen.IsFavourite = !imagen.IsFavourite;
                    db.Entry(imagen).State = EntityState.Modified;

                    modifiedImagesIds.Add(id);
                }

            }

            db.SaveChanges();

            return modifiedImagesIds;
        }

        // POST: api/Imagenes
        [ResponseType(typeof(Imagen))]
        public IHttpActionResult PostImage(Imagen imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WebClient webClient = new WebClient();

            string remoteFileUrl = imagen.OriginalUrl;
            string serverUrl = "http://localhost:55069/Content/Images/";
            string localFilePath = getLocalFilePath() + getLocalFileName(imagen);

            imagen.Path = serverUrl + getLocalFileName(imagen);
            imagen.Created = DateTime.Today;
            imagen.IsDeleted = false;

            try
            {
                if (imagen.UserUploaded == false)
                {
                    webClient.DownloadFile(remoteFileUrl, localFilePath);
                }

                db.Imagenes.Add(imagen);
                db.SaveChanges();

                return Ok(imagen);
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST: api/Imagenes/UploadImage
        [ResponseType(typeof(Imagen))]
        public IHttpActionResult UploadImage()
        {

            var request = HttpContext.Current.Request;
            HttpPostedFile file = request.Files["pcFile"];

            if (file != null)
            {
                string imageDirPath = "Content/Images/";

                Imagen newImage = new Imagen { 
                    Name = request.Form["imageName"],
                    OriginalUrl = request.Form["url"],
                    Created = DateTime.Today,
                    IsDeleted = false,
                    UserUploaded = true
                };

                string pic = getLocalFileName(newImage);
                string path = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imageDirPath, pic);
                newImage.Path = path;

                // file is uploaded and info stored in the DB
                file.SaveAs(path);
                db.Imagenes.Add(newImage);
                db.SaveChanges();

                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                return Ok(newImage);

            }

            // after successfully uploading redirect the user
            return BadRequest();
        }

        // DELETE: api/Imagenes/5
        [ResponseType(typeof(Imagen))]
        public IHttpActionResult DeleteImagen(int id)
        {
            Imagen imagen = db.Imagenes.Find(id);
            if (imagen == null)
            {
                return NotFound();
            }

            db.Imagenes.Remove(imagen);
            db.SaveChanges();

            return Ok(imagen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImagenExists(int id)
        {
            return db.Imagenes.Count(e => e.Id == id) > 0;
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
            string originalUrl = imagen.OriginalUrl;

            if (originalUrl.IndexOf('?') > -1)
            {
                originalUrl = originalUrl.Remove(originalUrl.IndexOf('?'));
            }

            datetime = datetime.Replace('/', '_').Replace('.', '_').Replace(':', '_').Replace(' ', '_').Replace('?', '_');
            return username + "_" + imagen.Name + "_" + datetime + Path.GetExtension(originalUrl);
        }
    }
}