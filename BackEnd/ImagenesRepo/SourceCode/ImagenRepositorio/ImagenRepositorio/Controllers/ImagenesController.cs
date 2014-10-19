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
using ImagenRepoHelpers;
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
            var images = this.imagenService.GetAll().ToList();

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

            var tags = new List<TagDto>();

            var imagentDto = ConvertToDto(imagen);

            foreach (var item in imagen.ImagenTags)
            {
                var tagDto = new TagDto()
                {
                 IsHidden = item.Tag.IsHidden,
                 Name = item.Tag.Name

                };
                tags.Add(tagDto);
            }

            imagentDto.Tags = tags;
            return Ok(imagentDto);
        }

        // PUT: api/Imagenes/5
        [ResponseType(typeof(ImagenDto))]
        public IHttpActionResult PutImagen(ImagenDto imagenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Este get trae una imagen que es trackeada por EF. Cuando en el repositorio quiero guardar la imagen,
            // se trula porque estoy trayendo de nuevo la misma imagen con el metodo  this.context.Entry<T>(entityToEdit);
            var originalImage = this.imagenService.Get(imagenDto.Id);
            if (originalImage != null)
            {
               // originalImage = ConvertFromDto(imagen);

                this.imagenService.Update(imagenDto,originalImage);
                return Ok(imagenDto);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Imagenes
        [ResponseType(typeof(ImagenDto))]
        public IHttpActionResult PostImage(ImagenDto imagenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            imagenDto.Path = this.imagenService.GetImagePath(imagenDto);

            try
            {
                if (imagenDto.UserUploaded == false)
                {
                    this.imagenService.DownloadImage(imagenDto);
                }

                imagenDto.Created = DateTime.Now;
                var createdImage = this.imagenService.CreateImage(imagenDto);
                imagenDto.Id = createdImage.Id;
                return Ok(imagenDto);
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
                //string serverImagesDirectoryForCurrentUserUrl = PathsAndUrlsHelper.GetCurrentUserImagesDirectoryFullPath();

                var newImage = new Imagen 
                { 
                    Name = request.Form["imageName"],
                    OriginalUrl = request.Form["url"],
                    Created = DateTime.Now,
                    UserUploaded = true
                };

                SetTagsToUserUploadedImage(request.Form["tags"], newImage);


                string pic = PathsAndUrlsHelper.CreateLocalFileName(newImage.Name, newImage.OriginalUrl);
                string localPath = Path.Combine(PathsAndUrlsHelper.GetLocalImagesDirectoryPathForCurrentLoggedInUser(), pic);
                newImage.Path = PathsAndUrlsHelper.CreateImageFullUrl(newImage.Name, newImage.OriginalUrl);

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
           // originalImage.Tags = imagen.Tags;
            originalImage.UserUploaded = imagen.UserUploaded;
        }

        // Refactor
        private void SetTagsToUserUploadedImage(string tags, Imagen img)
        {
            if (!String.IsNullOrEmpty(tags))
            {
                var tagsArray = tags.Split(',');

                foreach (var currentTag in tagsArray)
                {
                    var originalTag = this.tagService.GetTagByName(currentTag);
                    var imagenTag = new ImagenTag();
                    if (originalTag != null)
                    {
                        imagenTag.Tag = originalTag;
                    }
                    else
                    {
                        var tag = new Tag() 
                        {
                            Name = currentTag,
                        };
                        imagenTag.Tag = tag;
                    }
                    img.ImagenTags.Add(imagenTag);
                }
            }
        }

        private void SaveTag()
        {
        }

        private static ImagenDto ConvertToDto(Imagen imagen)
        {
            return Mapper.Map<ImagenDto>(imagen);
        }

        private static Imagen ConvertFromDto(ImagenDto imagenDto)
        {
            return Mapper.Map<Imagen>(imagenDto);
        }
    }
}