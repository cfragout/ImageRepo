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
    public class ImagenService: BaseService<Imagen> , IImagenService<Imagen>
    {
        public ImagenService(IGenericRepository<Imagen> genericRepo)
            : base(genericRepo)
        {

        }

        public IEnumerable<Imagen> GetLatestImagenes()
        {
            return base.GetAll().ToList().Take(5).ToList();
        }
    }
}
