using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagenRepoEntities.Entities
{
    public class Tag
    {
        public Tag()
        {
            Images = new List<Imagen>();

        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsHidden { get; set; }

        public ICollection<Imagen> Images { get; set; }

    }
}