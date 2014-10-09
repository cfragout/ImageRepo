using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ImagenRepoEntities.Entities
{
    [DataContract] 
    public class Tag: BaseEntity
    {
        public Tag()
        {
            Images = new List<Imagen>();

        }

        [Required]
        public string Name { get; set; }

        public bool IsHidden { get; set; }

        [IgnoreDataMember] 
        public ICollection<Imagen> Images { get; set; }

    }
}