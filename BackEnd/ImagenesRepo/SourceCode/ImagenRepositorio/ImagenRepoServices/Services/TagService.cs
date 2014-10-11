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
    public class TagService: BaseService<Tag>, ITagService
    {
        public TagService(IGenericRepository<Tag> genericRepo)
            : base(genericRepo)
        {

        }


        public Tag GetTagByName(string name)
        {
            return this.GetAll().Where(tag => tag.Name == name).FirstOrDefault();
        }
    }
}
