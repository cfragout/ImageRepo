using ImagenRepoDomain.Dtos;
using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.IServices
{
    public interface IImagenService: IBaseService<Imagen>
    {
        IEnumerable<Imagen> GetLatestImagenes();

        string GetImagePath(ImagenDto imagen);

        void DownloadImage(ImagenDto imagen);

        Imagen CreateImage(ImagenDto entityToCreate);

        IEnumerable<ImagenDto> GetAll();

        void Update(ImagenDto imagenDto, Imagen originalImagen);
    }
}
