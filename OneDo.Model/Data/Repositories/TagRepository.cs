using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(List<Tag> items = null) : base(items)
        {

        }
    }
}
