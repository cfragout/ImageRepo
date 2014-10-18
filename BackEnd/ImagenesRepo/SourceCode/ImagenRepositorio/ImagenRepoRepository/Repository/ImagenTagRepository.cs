using ImagenRepoEntities.UoW;
using ImagenRepoRepository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoRepository.Repository
{
    public class ImagenTagRepository : IImagenTagRepository
    {
        
        private ImagenRepoUnitOfWork imagenRepoUnitOfWork;

        public ImagenTagRepository(ImagenRepoUnitOfWork imagenRepoUnitOfWork) 
        {
            this.imagenRepoUnitOfWork = imagenRepoUnitOfWork;
        }

        public void Delete(int tagId, int imagenId)
        {
           var imagenTag = this.imagenRepoUnitOfWork.ImagenTags.FirstOrDefault(x =>
                x.Imagen.Id == imagenId &&
                x.Tag.Id == tagId);
           this.imagenRepoUnitOfWork.ImagenTags.Remove(imagenTag);
           this.imagenRepoUnitOfWork.SaveChanges();
        }

    }
}
