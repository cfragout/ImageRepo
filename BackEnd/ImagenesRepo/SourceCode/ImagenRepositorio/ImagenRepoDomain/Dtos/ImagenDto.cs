using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoDomain.Dtos
{
    [DataContract]
    public class ImagenDto
    {
        public ImagenDto()
        {
            Tags = new List<TagDto>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsFavourite { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public bool UserUploaded { get; set; }

        [DataMember]
        public string OriginalUrl { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        public ICollection<TagDto> Tags { get; set; }
    }
}
