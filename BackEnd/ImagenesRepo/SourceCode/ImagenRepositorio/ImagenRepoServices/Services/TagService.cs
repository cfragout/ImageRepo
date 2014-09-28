using ImagenRepoEntities.Entities;
using ImagenRepoRepository.IRepository;
using ImagenRepoServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.Services
{
    public class TagService: BaseService<Tag>, ITagService<Tag>
    {
        public TagService(IGenericRepository<Tag> genericRepo)
            : base(genericRepo)
        {

        }
    }
}
