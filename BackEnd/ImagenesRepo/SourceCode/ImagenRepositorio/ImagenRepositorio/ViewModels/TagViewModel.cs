using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagenRepositorio.ViewModels
{
    public class TagViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsHidden { get; set; }
    }
}