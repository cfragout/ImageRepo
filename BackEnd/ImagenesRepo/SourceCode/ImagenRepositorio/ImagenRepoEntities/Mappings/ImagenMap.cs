using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoEntities.Mappings
{
    public class ImagenMap : EntityTypeConfiguration<Imagen>
    {
        public ImagenMap()
        {


            //this.HasMany(p => p.Tags)
            //    .WithMany(p => p.Images);

        }
    }
}
