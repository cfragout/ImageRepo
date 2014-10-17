using ImagenRepoEntities.Entities;
using ImagenRepoEntities.Models;
using ImagenRepoEntities.UoW;
using ImagenRepoRepository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoRepository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> 
        where T : BaseEntity
    {
        private ImagenRepoUnitOfWork imagenRepoUnitOfWork;
        private ModelContainer context;

        public GenericRepository(ModelContainer container, ImagenRepoUnitOfWork imagenRepoUnitOfWork)
        {
            this.imagenRepoUnitOfWork = imagenRepoUnitOfWork;
            this.context = container;
        }

        public IEnumerable<T> Get()
        {
            return this.imagenRepoUnitOfWork.Set<T>()
              .Where(item => item.IsDeleted != true)
              .ToList();
        }
       
        public T Get(int id)
        {
            return this.imagenRepoUnitOfWork.Set<T>().Find(id);
        }

        
        public T Create(T entityToCreate)
        {
            entityToCreate.IsDeleted = false;
            this.imagenRepoUnitOfWork.Set<T>().Add(entityToCreate);
            this.imagenRepoUnitOfWork.SaveChanges();

            //Esto es por si necesitas usar el Id de la entidad creada. Como es identity lo tenes luego de que lo persiste
            return entityToCreate;
        }

        public void Edit(T entityToEdit)
        {
            //var entity = this.imagenRepoUnitOfWork.Entry<T>(entityToEdit);
            //entity.State = EntityState.Modified;
            this.imagenRepoUnitOfWork.SaveChanges();
        }

        public void Delete(T entityToDelete)
        {
            entityToDelete.IsDeleted = true;
            this.Edit(entityToDelete);
        }

        protected ModelContainer GetContext()
        {
            return this.context;
        }

        public void Dispose()
        {
            if (this.context != null)
                this.context.Dispose();
        }
    }
}