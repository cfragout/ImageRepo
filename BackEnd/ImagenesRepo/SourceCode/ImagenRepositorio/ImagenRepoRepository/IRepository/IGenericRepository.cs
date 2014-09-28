using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoRepository.IRepository
{
    public interface IGenericRepository<T>: IDisposable
    {
        IEnumerable<T> Get();

        T Get(int id);

        T Create(T entityToCreate);

        void Edit(T entityToEdit);

        void Delete(T entityToDelete);
    }
}
