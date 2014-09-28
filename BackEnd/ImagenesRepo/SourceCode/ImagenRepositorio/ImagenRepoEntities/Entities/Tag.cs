using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagenRepoEntities.Entities
{
    public class Tag
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public virtual ICollection<Imagen> Imagenes { get; set; }

    }
}