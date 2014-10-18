using ImagenRepoEntities.Entities;
using ImagenRepoServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.IServices
{
    public interface IImagenTagService
    {
        void DeleteTag(int tagId, int imageId);
    }
}
