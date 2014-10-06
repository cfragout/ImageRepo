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

        public override IEnumerable<Imagen> GetAll()
        {

            List<Imagen> everyNonDeletedImage =  base.GetAll().Where(i => i.IsDeleted == false).ToList();
            List<Imagen> filteredImages = new List<Imagen>();

            foreach (Imagen image in everyNonDeletedImage)
            {
                if (image.Tags.ToList().Where(t => t.IsHidden == true).Count() == 0)
                {
                    filteredImages.Add(image);
                }
            }

            

            return filteredImages;
        }

        public IEnumerable<Imagen> GetLatestImagenes()
        {
            return base.GetAll().ToList().Take(5).ToList();
        }
    }
}
