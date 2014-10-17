using ImagenRepoEntities.Entities;
using ImagenRepoRepository.IRepository;
using ImagenRepoRepository.Repository;
using ImagenRepoServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.Services
{
    public class BaseService<T>: IBaseService<T> 
        where T : class
    {

        private IGenericRepository<T> repository;

        public BaseService(IGenericRepository<T> repository)
        {
            this.repository = repository;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.repository.Get();
        }

        public T Get(int id)
        {
            return this.repository.Get(id);
        }

        public virtual T Create(T entityToCreate)
        {
            return this.repository.Create(entityToCreate);
        }

        public virtual void Update(T entityToUpdate)
        {
            this.repository.Edit(entityToUpdate);
        }

        public void Delete(T entityToDelete)
        {
            this.repository.Delete(entityToDelete);
        }
    }
}
