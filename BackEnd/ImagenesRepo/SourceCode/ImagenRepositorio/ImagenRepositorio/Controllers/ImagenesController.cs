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
using System.Web.Http.Cors;
using ImagenRepoDomain.Dtos;
using AutoMapper;

namespace ImagenRepositorio.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImagenesController : ApiController
    {
        
        private IImagenService imagenService;
        private ITagService tagService;

        public ImagenesController(
            IImagenService imagenService,
            ITagService tagService)
        {
            this.imagenService = imagenService;
            this.tagService = tagService;
        }

        // GET: api/Imagenes
        [ResponseType(typeof(IEnumerable<ImagenDto>))]
        public IHttpActionResult GetImages()
        {
            var images = this.imagenService.GetAll().Select(ConvertToDto).ToList();

            return Ok(images);
        }

        // GET: api/Imagenes/GetLatestImages
        public IEnumerable<ImagenDto> GetLatestImages()
        {
            return this.imagenService.GetLatestImagenes().Select(ConvertToDto).ToList();
        }

        // GET: api/Imagenes/5
        [ResponseType(typeof(ImagenDto))]
        public IHttpActionResult GetImagen(int id)
        {
            Imagen imagen = this.imagenService.Get(id);
            if (imagen == null)
            {
                return NotFound();
            }

            return Ok(ConvertToDto(imagen));
        }

        // PUT: api/Imagenes/5
        [ResponseType(typeof(ImagenDto))]
        public IHttpActionResult PutImagen(ImagenDto imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var originalImage = this.imagenService.Get(imagen.Id);

            if (originalImage != null)
            {
                originalImage = ConvertFromDto(imagen);
                this.imagenService.Update(originalImage);
                return Ok(ConvertToDto(originalImage));
            }
            else
            {
                return NotFound();
            }

        }

       


        //Esto deberia hacerse al modificar la imagen en el put. La imagen llega con los datos nuevos y se hace un update en la base.

        // POST: api/Imagenes/RemoveTag
       /* public bool RemoveTag()
        {
            var request = HttpContext.Current.Request;


            if ((request.Form["imageId"] == null) || (request.Form["tagId"] == null))
            {
                return false;
            }

            int imgId = Convert.ToInt32(request.Form["imageId"]);
            int tagId = Convert.ToInt32(request.Form["tagId"]);

            if (ImagenExists(imgId))
            {
                Imagen imagen = db.Imagenes.Find(imgId);

                // There must be an easier way!
                imagen.Tags = imagen.Tags.ToList().FindAll(t => t.Id != tagId);

                db.Entry(imagen).State = EntityState.Modified;

                db.SaveChanges(); 

                return true;

            }

            return false;
        }*/

        //Esto deberia hacerse al modificar la imagen en el put. La imagen llega con los datos nuevos y se hace un update en la base.

        // POST: api/Imagenes/MarkImagesAsFavourite
        /*public List<int> MarkImagesAsFavourite()
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
        }*/

        // POST: api/Imagenes
        [ResponseType(typeof(ImagenDto))]
        public IHttpActionResult PostImage(ImagenDto imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            imagen.Path = this.imagenService.GetImagePath(imagen);
            try
            {
                if (imagen.UserUploaded == false)
                {
                    this.imagenService.DownloadImage(imagen);
                }

                imagen.Created = DateTime.Now;
                var createdImage = this.imagenService.Create(ConvertFromDto(imagen));

                return Ok(ConvertToDto(createdImage));
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST: api/Imagenes/UploadImage
        [ResponseType(typeof(ImagenDto))]
        public IHttpActionResult UploadImage()
        {

            var request = HttpContext.Current.Request;
            HttpPostedFile file = request.Files["pcFile"];

            if (file != null)
            {
                string serverUrl = "http://localhost:55069/Content/Images/";

                var newImage = new Imagen { 
                    Name = request.Form["imageName"],
                    OriginalUrl = request.Form["url"],
                    Created = DateTime.Now,
                    UserUploaded = true
                };

                SetTagsToUserUploadedImage(request.Form["tags"], newImage);

                string pic = GetLocalFileName(newImage);
                string localPath = Path.Combine(GetLocalFilePath(), pic);
                newImage.Path = serverUrl + pic;

                // file is uploaded and info stored in the DB
                file.SaveAs(localPath);

                var createdImage = this.imagenService.Create(newImage);

                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                return Ok(ConvertToDto(createdImage));

            }

            // after successfully uploading redirect the user
            return BadRequest();
        }

        // DELETE: api/Imagenes/5
        [ResponseType(typeof(ImagenDto))]
        public IHttpActionResult DeleteImagen(int id)
        {
            var imagen = this.imagenService.Get(id);
            if (imagen == null)
            {
                return NotFound();
            }

            this.imagenService.Delete(imagen);
           
            return Ok(imagen);
        }


        private void MapEditedToOriginal(Imagen originalImage, Imagen imagen)
        {
            originalImage.IsDeleted = imagen.IsDeleted;
            originalImage.Created = imagen.Created;
            originalImage.IsFavourite = imagen.IsFavourite;
            originalImage.Name = imagen.Name;
            originalImage.OriginalUrl = imagen.OriginalUrl;
            originalImage.Path = imagen.Path;
            originalImage.Tags = imagen.Tags;
            originalImage.UserUploaded = imagen.UserUploaded;
        }


        private string GetLocalFilePath()
        {
            string imageDirPath = "Content/Images/";
            return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imageDirPath;
        }

        private string GetLocalFileName(Imagen imagen)
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

        // Refactor
        private void SetTagsToUserUploadedImage(string tags, Imagen img)
        {
            List<Tag> tagCollection = new List<Tag>();

            if (!String.IsNullOrEmpty(tags))
            {
                var tagsArray = tags.Split(',');

                foreach (var currentTag in tagsArray)
                {
                    var originalTag = this.tagService.GetTagByName(currentTag);

                    if (originalTag != null)
                    {
                        img.Tags.Add(originalTag);
                    }
                    else
                    {
                        var tag = new Tag() 
                        {
                            Name = currentTag,
                        };

                        img.Tags.Add(tag);
                    }
                }
            }
        }

        // Refactor
        private void SetTagsToInternetFetchedImage(ICollection<Tag> tags, Imagen img)
        {
            List<Tag> tagCollection = new List<Tag>();

                foreach (var currentTag in tags)
                {
                    //var queryResult = db.Tags.Where(t => t.Name == currentTag.Name);
                    //Tag tag;

                    //if (queryResult.Count() > 0)
                    //{
                    //    tag = queryResult.FirstOrDefault();
                    //}
                    //else
                    //{
                    //    tag = new Tag();
                    //    tag = currentTag;
                    //}

                    ////tag.Imagenes.Add(img);
                    //img.Tags.Add(tag);
                }

            return;
        }

        private void SaveTag()
        {

        }

        private static ImagenDto ConvertToDto(Imagen bill)
        {
            return Mapper.Map<ImagenDto>(bill);
        }

        private static Imagen ConvertFromDto(ImagenDto billDto)
        {
            return Mapper.Map<Imagen>(billDto);
        }
    }
}