using ImagenRepoDomain.Dtos;
using ImagenRepoEntities.Entities;
using ImagenRepoRepository.IRepository;
using ImagenRepoRepository.Repository;
using ImagenRepoServices.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.Services
{
    public class ImagenService: BaseService<Imagen> , IImagenService
    {
        ITagService tagService;
        IImagenTagService imagenTagService;

        public ImagenService(IGenericRepository<Imagen> genericRepo,
            ITagService tagService,
            IImagenTagService imagenTagService)
            : base(genericRepo)
        {
            this.tagService = tagService;
            this.imagenTagService = imagenTagService;
        }

        public IEnumerable<ImagenDto> GetAll()
        {
            var oringinalImages = base.GetAll().ToList();
            var filteredImages = new List<ImagenDto>();

            foreach (var image in oringinalImages)
            {
                var imagenDto = createImageDto(image);
                filteredImages.Add(imagenDto);
            }

            return filteredImages;
        }

        public Imagen CreateImage(ImagenDto entityToCreate)
        {
            var imageToCreate = new Imagen();
            MapSimplePropertiesToEntity(entityToCreate,imageToCreate);

            foreach (var tag in entityToCreate.Tags)
            {
                var imagenTag = new ImagenTag() 
                {
                    Tag = MapTagDtoToTag(tag)
                };

                imageToCreate.ImagenTags.Add(imagenTag);
            }

            return base.Create(imageToCreate);
        }

        public void Update(ImagenDto imagenDto, Imagen originalImagen)
        {
            MapSimplePropertiesToEntity(imagenDto, originalImagen);

            var TagsNames = imagenDto.Tags.Select(tag => tag.Name).ToList();

            foreach (var item in originalImagen.ImagenTags)
            {
                if (TagsNames.Contains(item.Tag.Name))
                {
                    
                }
            }


            base.Update(originalImagen);
        }

        public IEnumerable<Imagen> GetLatestImagenes()
        {
            return base.GetAll().ToList().Take(5).ToList();
        }

        public void DownloadImage(ImagenDto imagen)
        {
            WebClient webClient = new WebClient();

            string remoteFileUrl = imagen.OriginalUrl;
            string localFilePath = GetLocalFilePath() + GetLocalFileName(imagen);
            webClient.DownloadFile(remoteFileUrl, localFilePath);
        }

        // Mover a helper?
        public string GetImagePath(ImagenDto imagen)
        {
            string serverUrl = "http://localhost:55069/Content/users/test_user@hotmail.com/images/";
            return serverUrl + GetLocalFileName(imagen);
        }

        #region Private Methods

        private ImagenDto createImageDto(Imagen image)
        {
            var imageDto = new ImagenDto()
            {
                Created = image.Created,
                Id = image.Id,
                IsFavourite = image.IsFavourite,
                Name = image.Name,
                OriginalUrl = image.OriginalUrl,
                Path = image.Path,
                UserUploaded = image.UserUploaded
            };

            foreach (var imagenTag in image.ImagenTags)
            {
                var tagDto = createTagDto(imagenTag);
                imageDto.Tags.Add(tagDto);
            }

            return imageDto;
        }

        private TagDto createTagDto(ImagenTag imagenTag)
        {
            var tagDto = new TagDto()
            {
                Id = imagenTag.Tag.Id,
                IsHidden = imagenTag.Tag.IsHidden,
                Name = imagenTag.Tag.Name
            };

            return tagDto;
        }

        private Tag MapTagDtoToTag(TagDto tagDto)
        {
            var tag = tagService.GetTagByName(tagDto.Name);
            if (tag != null)
            {
                return tag;
            }
            else
            {
                return tag = new Tag()
                {
                    IsHidden = tagDto.IsHidden,
                    Name = tagDto.Name
                };
            }
        }

        private void MapSimplePropertiesToEntity(ImagenDto entityToCreate, Imagen originalImage)
        {
            originalImage.Created = entityToCreate.Created;
            originalImage.IsDeleted = entityToCreate.IsDeleted;
            originalImage.IsFavourite = entityToCreate.IsFavourite;
            originalImage.Name = entityToCreate.Name;
            originalImage.OriginalUrl = entityToCreate.OriginalUrl;
            originalImage.Path = entityToCreate.Path;
            originalImage.UserUploaded = entityToCreate.UserUploaded;
        }

        private string GetLocalFileName(ImagenDto imagen)
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

        private string GetLocalFilePath()
        {
            string imageDirPath = "Content/users/test_user@hotmail.com/images/";
            return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imageDirPath;
        }

        #endregion

    }
}
