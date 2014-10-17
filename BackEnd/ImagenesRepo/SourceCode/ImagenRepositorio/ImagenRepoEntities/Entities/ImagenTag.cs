﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoEntities.Entities
{
    public class ImagenTag
    {
        public int Id { get; set; }

        public virtual Imagen Imagen { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
