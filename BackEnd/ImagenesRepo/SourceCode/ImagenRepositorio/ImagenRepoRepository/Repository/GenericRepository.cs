﻿using ImagenRepoEntities.Models;
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
        where T : class
    {

        private ModelContainer context;

        public GenericRepository()
        {
            this.context = new ModelContainer();
        }

        public GenericRepository(ModelContainer container)
        {
            this.context = container;
        }

        public IEnumerable<T> Get()
        {
          return this.context.Set<T>().ToList();
        }
       
        public T Get(int id)
        {
            return this.context.Set<T>().Find(id);
        }

        
        public T Create(T entityToCreate)
        {
            this.context.Set<T>().Add(entityToCreate);
            this.context.SaveChanges();
            //Esto es por si necesitas usar el Id de la entidad creada. Como es identity lo tenes luego de que lo persiste
            return entityToCreate;
        }

        public void Edit(T entityToEdit)
        {
            var entity = this.context.Entry<T>(entityToEdit);
            entity.State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public void Delete(T entityToDelete)
        {
            this.context.Set<T>().Remove(entityToDelete);
            this.context.SaveChanges();
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