using AutoMapper;
using ImagenRepoDomain.Dtos;
using ImagenRepoEntities.Entities;
using ImagenRepoRepository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoHelpers.MappingHelpers
{
    public class TagMap
    {
        private IGenericRepository<Tag> tagRepo;

        public TagMap(IGenericRepository<Tag> tagRepo)
        {
            this.tagRepo = tagRepo;
        }

        public Tag ConvertToTag(TagDto tagDto)
        {
            var tag = tagRepo.Get().Where(t => t.Name == tagDto.Name).FirstOrDefault();
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

        public TagDto ConvertToTagDto(Tag tag, TagDto tagDto)
        {
            return tagDto;
        }

        public TagDto CreateTagDto(ImagenTag imagenTag)
        {
            var tagDto = Mapper.Map<TagDto>(imagenTag.Tag);

            return tagDto;
        }
    }
}
