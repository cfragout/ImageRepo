using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoRepository.IRepository
{
    public interface IImagenTagRepository
    {
        void Delete(int tagId, int imagenId);
    }
}
