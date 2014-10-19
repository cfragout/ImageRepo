using ImagenRepoEntities.Entities;
using ImagenRepoEntities.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ImagenRepoEntities.Models
{
    public class ModelContainer : DbContext
    {
        public ModelContainer()
            //: base("name=repoLaburo")
            : base("name=ImageRepoFrapanDesktop")
        {
            base.Configuration.ProxyCreationEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.Configuration.ProxyCreationEnabled = true;
            
            #region Entities Mapping
            
           // modelBuilder.Configurations.Add(new ImagenMap());

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Imagen> Imagenes { get; set; }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<ImagenTag> ImagenTags { get; set; }


    }
}