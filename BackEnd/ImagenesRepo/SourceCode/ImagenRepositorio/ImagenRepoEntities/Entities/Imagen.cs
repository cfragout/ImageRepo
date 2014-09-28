using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagenRepoEntities.Entities
{
    public class Imagen
    {
        public Imagen()
        {
            Tags = new List<Tag>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Path { get; set; }

        [Required]
        public bool UserUploaded { get; set; }

        public string OriginalUrl { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Created { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}