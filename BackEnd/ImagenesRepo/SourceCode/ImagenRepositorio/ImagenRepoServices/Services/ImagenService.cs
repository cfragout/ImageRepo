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
using ImagenRepoHelpers;
using ImagenRepoHelpers.MappingHelpers;

namespace ImagenRepoServices.Services
{
    public class ImagenService: BaseService<Imagen> , IImagenService
    {
        ITagService tagService;
        IImagenTagService imagenTagService;
        ImagenMap imagenMap;

        public ImagenService(IGenericRepository<Imagen> genericRepo,
            ITagService tagService,
            IImagenTagService imagenTagService,
            ImagenMap imagenMap)
            : base(genericRepo)
        {
            this.tagService = tagService;
            this.imagenTagService = imagenTagService;
            this.imagenMap = imagenMap;
        }

        public IEnumerable<ImagenDto> GetAll()
        {
            var oringinalImages = base.GetAll().ToList();
            var filteredImages = new List<ImagenDto>();

            foreach (var image in oringinalImages)
            {
                var imagenDto = imagenMap.CreateImagenDto(image);
                filteredImages.Add(imagenDto);
            }

            return filteredImages;
        }

        public Imagen CreateImage(ImagenDto entityToCreate)
        {
            var imageToCreate = imagenMap.CreateImagenFromDto(entityToCreate);

            //imagenMap.ConvertToImagen(entityToCreate, imageToCreate);

            return base.Create(imageToCreate);
        }

        public void Update(ImagenDto imagenDto, Imagen originalImagen)
        {
            //Este metodo mapea las propiedades simples como hidden o favorito
            MapSimplePropertiesToEntity(imagenDto, originalImagen);

            var TagsNames = imagenDto.Tags.Select(tag => tag.Name).ToList();
            var TagsToRemove = new Dictionary<int,int>();

            //Hago esto para remover de la coleccion original aquellos tags que fueron borrados en la vista
            originalImagen.ImagenTags.ToList().ForEach(imagenTag =>
                {
                    if (!TagsNames.Contains(imagenTag.Tag.Name))
                    {
                        TagsToRemove.Add(imagenTag.Tag.Id, imagenTag.Imagen.Id);
                    }
                }
            );

            TagsToRemove.ToList().ForEach(item => 
                {
                    imagenTagService.DeleteTag(item.Key, item.Value);
                }
            );

            //Hago esto para ver si se agregaron tags nuevos.
            AddNewTagsToOriginalImage(imagenDto, originalImagen);

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
            string localFilePath = PathsAndUrlsHelper.GetLocalImagesDirectoryPathForCurrentLoggedInUser() + PathsAndUrlsHelper.CreateLocalFileName(imagen.Name, imagen.OriginalUrl);
            webClient.DownloadFile(remoteFileUrl, localFilePath);
        }

        public string GetImagePath(ImagenDto imagen)
        {
            return PathsAndUrlsHelper.CreateImageFullUrl(imagen.Name, imagen.OriginalUrl);
        }

        #region Private Methods

        private void AddNewTagsToOriginalImage(ImagenDto imagenDto, Imagen originalImagen)
        {
            var originalTagsNames = originalImagen.ImagenTags.Select(originalTag => originalTag.Tag.Name);
            foreach (var tagDto in imagenDto.Tags)
            {
                //Sino lo contiene tengo que ver si exise o si tengo que crear un tag nuevo
                if (!originalTagsNames.Contains(tagDto.Name))
                {
                    var imagenTag = new ImagenTag();
                    //Busco el tag por el nombre
                    var originalTag = tagService.GetTagByName(tagDto.Name);
                    //Me fijo que si encontró el tag
                    if (originalTag != null)
                    {
                        //Si lo encontro lo asigno
                        imagenTag.Tag = originalTag;
                    }
                    else
                    {
                        //Sino lo encontró entonces creo un tag nuevo
                        var tag = new Tag()
                        {
                            IsHidden = tagDto.IsHidden,
                            Name = tagDto.Name
                        };
                        //Asigno el tag
                        imagenTag.Tag = tag;

                    }
                    //Le agrego el tag a la imagenOriginal
                    originalImagen.ImagenTags.Add(imagenTag);
                }
            }
        }

        //private ImagenDto createImageDto(Imagen image)
        //{
        //    var imageDto = new ImagenDto()
        //    {
        //        Created = image.Created,
        //        Id = image.Id,
        //        IsFavourite = image.IsFavourite,
        //        Name = image.Name,
        //        OriginalUrl = image.OriginalUrl,
        //        Path = image.Path,
        //        UserUploaded = image.UserUploaded
        //    };

        //    foreach (var imagenTag in image.ImagenTags)
        //    {
        //        var tagDto = createTagDto(imagenTag);
        //        imageDto.Tags.Add(tagDto);
        //    }

        //    return imageDto;
        //}

        //private TagDto createTagDto(ImagenTag imagenTag)
        //{
        //    var tagDto = new TagDto()
        //    {
        //        Id = imagenTag.Tag.Id,
        //        IsHidden = imagenTag.Tag.IsHidden,
        //        Name = imagenTag.Tag.Name
        //    };

        //    return tagDto;
        //}

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

        #endregion

    }
}
