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
    public class ImagenTagService: IImagenTagService
    {
        IImagenTagRepository imagenTagRepo;

        public ImagenTagService(IImagenTagRepository imagenTagRepo)
        {
            this.imagenTagRepo = imagenTagRepo;
        }

        public void DeleteTag(int tagId , int imagenId)
        {
            this.imagenTagRepo.Delete(tagId, imagenId);
        }
    }
}
