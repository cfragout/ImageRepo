using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImagenRepoEntities.UoW
{
    public interface IImagenRepoUnitOfWork: IEntityFrameworkUnitOfWork
    {
        IDbSet<Imagen> Imagenes { get; set; }

        IDbSet<Tag> Tags { get; set; }

        IDbSet<ImagenTag> ImagenTags { get; set; }
    }
}