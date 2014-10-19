using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoEntities.UoW
{
    public class ImagenRepoUnitOfWork : DbContext, IImagenRepoUnitOfWork
    {
        public ImagenRepoUnitOfWork()
            //: base("name=repoLaburo")
            : base("name=ImageRepoFrapanDesktop")
        {
            base.Configuration.ProxyCreationEnabled = true;
   
        }

        IDbSet<Imagen> imagenes;
        public IDbSet<Imagen> Imagenes
        {
            get
            {
                if (imagenes == null)
                    imagenes = base.Set<Imagen>();
                return imagenes;
            }

            set
            {
                imagenes = value;
            }
        }


        IDbSet<Tag> tags;
        public IDbSet<Tag> Tags
        {
            get
            {
                if (imagenTags == null)
                    tags = base.Set<Tag>();
                return tags;
            }

            set
            {
                tags = value;
            }
        }

        IDbSet<ImagenTag> imagenTags;
        public IDbSet<ImagenTag> ImagenTags
        {
            get
            {
                if (imagenTags == null)
                    imagenTags = base.Set<ImagenTag>();
                return imagenTags;
            }

            set
            {
                imagenTags = value;
            }
        }

        // Implementación de IEntityFrameworkUnitOfWork

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (base.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                base.Set<TEntity>().Attach(entity);
            }
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        // Implementación de IUnitOfWork

        public void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });
                }
            } while (saveFailed);
        }

        public void Rollback()
        {
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        // Sobreescibimos OnModelCreating de DdContext

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.Configuration.ProxyCreationEnabled = true;

            #region Entities Mapping

            // modelBuilder.Configurations.Add(new ImagenMap());

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public new void Dispose()
        {
            base.Dispose();
        }
       
    }
}
