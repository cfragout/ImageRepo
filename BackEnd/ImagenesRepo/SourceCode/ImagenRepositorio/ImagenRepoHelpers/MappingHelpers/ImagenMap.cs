using AutoMapper;
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
            Mapper.Map(imagenDto, imagen);

            ConvertTagDtoToImagenTag(imagenDto, imagen);

            return imagen;
        }

        public Imagen CreateImagenFromDto(ImagenDto imagenDto) 
        {
            var imagen = Mapper.Map<Imagen>(imagenDto);

            ConvertTagDtoToImagenTag(imagenDto, imagen);

            return imagen;
        }

        public ImagenDto ConvertToImagenDto(Imagen imagen, ImagenDto imagenDto)
        {
            Mapper.Map(imagen, imagenDto);

            CreateTagDtoFromImagenTag(imagen, imagenDto);

            return imagenDto;
        }

        public ImagenDto CreateImagenDto(Imagen imagen)
        {
            var imageDto = Mapper.Map<ImagenDto>(imagen);

            CreateTagDtoFromImagenTag(imagen, imageDto);

            return imageDto;
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

        private void CreateTagDtoFromImagenTag(Imagen imagen, ImagenDto imageDto)
        {
            foreach (var imagenTag in imagen.ImagenTags)
            {
                var tagDto = tagMap.CreateTagDto(imagenTag);
                imageDto.Tags.Add(tagDto);
            }
        }

        private void MapSimplePropertiesToEntity(ImagenDto entityToCreate, Imagen originalImage)
        {
            Mapper.Map(entityToCreate, originalImage);

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
