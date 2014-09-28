using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.IServices
{
    public interface IBaseService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Create(T entityToCreate);
        void Update(T entityToUpdate);
        void Delete(T entityToDelete);
    }
}
