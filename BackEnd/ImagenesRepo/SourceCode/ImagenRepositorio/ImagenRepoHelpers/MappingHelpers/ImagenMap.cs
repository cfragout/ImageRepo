using ImagenRepoDomain.Dtos;
using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoHelpers.MappingHelpers
{
    public class ImagenMap
    {
        private TagMap tagMap;

        public ImagenMap(TagMap tagMap)
        {
            this.tagMap = tagMap;
        }

        public Imagen ConvertToImagen(ImagenDto imagenDto, Imagen imagen)
        {
            MapSimplePropertiesToEntity(imagenDto, imagen);

            ConvertTagDtoToImagenTag(imagenDto, imagen);

            return imagen;
        }

        private void ConvertTagDtoToImagenTag(ImagenDto imagenDto, Imagen imagen)
        {
            foreach (var tag in imagenDto.Tags)
            {
                var imagenTag = new ImagenTag()
                {
                    Tag = tagMap.ConvertToTag(tag)
                };

                imagen.ImagenTags.Add(imagenTag);
            }
        }

        public ImagenDto ConvertToImagenDto(Imagen imagen , ImagenDto imagenDto)
        {
            return imagenDto;
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
    }
}
