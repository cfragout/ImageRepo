using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.IServices
{
    public interface IImagenService<T>: IBaseService<T>
        where T : Imagen
    {

        IEnumerable<Imagen> GetLatestImagenes();

        string GetImagePath(Imagen imagen);

        void DownloadImage(Imagen imagen);
    }
}
