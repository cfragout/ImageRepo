﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace ImagenRepoEntities.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
