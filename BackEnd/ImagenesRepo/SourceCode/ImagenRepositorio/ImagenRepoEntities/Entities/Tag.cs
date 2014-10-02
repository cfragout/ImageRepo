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
            Imagenes = new List<Imagen>();

        }

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public virtual ICollection<Imagen> Imagenes { get; set; }

    }
}