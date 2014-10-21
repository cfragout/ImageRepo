using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoHelpers
{
    public class PathsAndUrlsHelper
    {
        public static string GetCurrentUserImagesDirectoryFullUrl()
        {
            return string.Format("{0}{1}", GetServerUrl(), GetCurrentUserImagesDirectoryRelativePath());
        }

        public static string GetCurrentUserImagesDirectoryRelativePath()
        {
            return "Content/users/test_user@hotmail.com/images/";
        }

        public static string GetCurrentUserBackupDirectoryFullPath()
        {
            return string.Format("{0}{1}", System.AppDomain.CurrentDomain.BaseDirectory, GetCurrentUserBackupDirectoryRelativePath());
        }

        public static string GetCurrentUserBackupDirectoryRelativePath()
        {
            return "Content/users/test_user@hotmail.com/";
        }

        public static string GetLocalImagesDirectoryPathForCurrentLoggedInUser()
        {
            // Path in local disk to ImagenRepositorio web project + images directory for current logged in user
            return System.AppDomain.CurrentDomain.BaseDirectory + GetCurrentUserImagesDirectoryRelativePath();
        }

        public static string CreateImageFullUrl(string imageName, string imageOriginalUrl)
        {
            return GetCurrentUserImagesDirectoryFullUrl() + CreateLocalFileName(imageName, imageOriginalUrl);
        }

        public static string CreateLocalFileName(string imageName, string imageOriginalUrl)
        {
            string username = "test_user@hotmail.com";
            string datetime = DateTime.Today.ToString();
            string originalUrl = imageOriginalUrl;

            if (originalUrl.IndexOf('?') > -1)
            {
                originalUrl = originalUrl.Remove(originalUrl.IndexOf('?'));
            }

            datetime = datetime.Replace('/', '_').Replace('.', '_').Replace(':', '_').Replace(' ', '_').Replace('?', '_');
            return string.Format("{0}_{1}_{2}{3}", username, imageName, datetime, Path.GetExtension(originalUrl)); // username + "_" + imagen.Name + "_" + datetime + Path.GetExtension(originalUrl);
        }

        public static string GetServerUrl()
        {
            //var request = HttpContext.Current.Request;
            //return string.Format("{0}://{1}/", request.Url.Scheme, request.Url.Authority);
            return "http://localhost:55069/";
        }
    }
}
