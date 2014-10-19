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
            ImagenTag = new List<ImagenTag>();
        }

        
        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required] 
        public ICollection<ImagenTag> ImagenTag { get; set; }

    }
}