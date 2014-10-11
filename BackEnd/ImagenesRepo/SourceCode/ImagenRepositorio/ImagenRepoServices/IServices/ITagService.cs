using ImagenRepoEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagenRepoServices.IServices
{
    public interface ITagService: IBaseService<Tag>
    {
        Tag GetTagByName(string name);
    }
}
