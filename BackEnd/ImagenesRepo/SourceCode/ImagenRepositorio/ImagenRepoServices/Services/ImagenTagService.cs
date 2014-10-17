using ImagenRepoEntities.Entities;
using ImagenRepoRepository.IRepository;
using ImagenRepoServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.Services
{
    public class ImagenTagService: BaseService<ImagenTag>, IImagenTagService
    {
        IGenericRepository<ImagenTag> imagenTagGenericRepo;

        public ImagenTagService(IGenericRepository<ImagenTag> imagenTagGenericRepo, IGenericRepository<Tag> TagGenericRepo)
            : base(imagenTagGenericRepo)
        {
            this.imagenTagGenericRepo = imagenTagGenericRepo;
        }
    }
}
