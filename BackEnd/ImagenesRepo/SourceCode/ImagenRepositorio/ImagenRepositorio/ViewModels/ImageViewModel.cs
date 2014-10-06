using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagenRepositorio.ViewModels
{
    public class ImageViewModel
    {
        public ImageViewModel()
        {
            Tags = new List<TagViewModel>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsFavourite { get; set; }

        public string Path { get; set; }

        [Required]
        public bool UserUploaded { get; set; }

        public string OriginalUrl { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Created { get; set; }

        public ICollection<TagViewModel> Tags { get; set; }
    }
}