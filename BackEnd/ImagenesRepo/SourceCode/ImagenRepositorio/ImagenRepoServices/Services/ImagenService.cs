using ImagenRepoEntities.Entities;
using ImagenRepoRepository.IRepository;
using ImagenRepoRepository.Repository;
using ImagenRepoServices.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            List<Imagen> everyNonDeletedImage =  base.GetAll().ToList();
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

        public void DownloadImage(Imagen imagen)
        {
            WebClient webClient = new WebClient();

            string remoteFileUrl = imagen.OriginalUrl;
            string localFilePath = getLocalFilePath() + getLocalFileName(imagen);
            webClient.DownloadFile(remoteFileUrl, localFilePath);
        }

        public string GetImagePath(Imagen imagen)
        {
            string serverUrl = "http://localhost:55069/Content/Images/";
            return serverUrl + getLocalFileName(imagen);
        }

        private string getLocalFileName(Imagen imagen)
        {
            string username = "CFR";
            string datetime = DateTime.Today.ToString();
            string originalUrl = imagen.OriginalUrl;

            if (originalUrl.IndexOf('?') > -1)
            {
                originalUrl = originalUrl.Remove(originalUrl.IndexOf('?'));
            }

            datetime = datetime.Replace('/', '_').Replace('.', '_').Replace(':', '_').Replace(' ', '_').Replace('?', '_');
            return username + "_" + imagen.Name + "_" + datetime + Path.GetExtension(originalUrl);
        }

        private string getLocalFilePath()
        {
            string imageDirPath = "Content/Images/";
            return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imageDirPath;
        }

    }
}
