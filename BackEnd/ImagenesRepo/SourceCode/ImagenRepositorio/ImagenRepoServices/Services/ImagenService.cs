using ImagenRepoDomain.Dtos;
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
    public class ImagenService: BaseService<Imagen> , IImagenService
    {
        IGenericRepository<Tag> TagGenericRepo;

        public ImagenService(IGenericRepository<Imagen> genericRepo, IGenericRepository<Tag> TagGenericRepo)
            : base(genericRepo)
        {
            this.TagGenericRepo = TagGenericRepo;
        }

        public override IEnumerable<Imagen> GetAll()
        {

            var lista = base.GetAll().ToList();
            List<Imagen> everyNonDeletedImage = base.GetAll().ToList();
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

        public override void Update(Imagen imagenToEdit)
        {
            base.Update(imagenToEdit);
        }

        public IEnumerable<Imagen> GetLatestImagenes()
        {
            return base.GetAll().ToList().Take(5).ToList();
        }

        public void DownloadImage(ImagenDto imagen)
        {
            WebClient webClient = new WebClient();

            string remoteFileUrl = imagen.OriginalUrl;
            string localFilePath = GetLocalFilePath() + GetLocalFileName(imagen);
            webClient.DownloadFile(remoteFileUrl, localFilePath);
        }

        // Mover a helper?
        public string GetImagePath(ImagenDto imagen)
        {
            string serverUrl = "http://localhost:55069/Content/username/Images/";
            return serverUrl + GetLocalFileName(imagen);
        }

        private string GetLocalFileName(ImagenDto imagen)
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

        private string GetLocalFilePath()
        {
            string imageDirPath = "Content/username/Images/";
            return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imageDirPath;
        }

    }
}
