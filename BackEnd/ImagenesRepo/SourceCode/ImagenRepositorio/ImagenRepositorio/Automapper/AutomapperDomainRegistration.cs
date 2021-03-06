﻿using AutoMapper;
using ImagenRepoDomain.Dtos;
using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagenRepositorio.Automapper
{
    public static class AutomapperDomainRegistration
    {
        public static void Register()
        {
            RegisterTagMapping();
            RegisterImagenMapping();
        }

        private static void RegisterTagMapping()
        {
            Mapper.CreateMap<Tag, TagDto>();

            Mapper.CreateMap<TagDto, Tag>();

              //.ForMember(x => x.Factura, opt => opt.MapFrom(c => c.Factura))
        }

        private static void RegisterImagenMapping()
        {
            Mapper.CreateMap<Imagen, ImagenDto>()
                .ForMember(x => x.Tags, opt => opt.Ignore()); ;

            Mapper.CreateMap<ImagenDto, Imagen>()
                .ForMember(x => x.ImagenTags, opt => opt.Ignore());
        }
    }
}