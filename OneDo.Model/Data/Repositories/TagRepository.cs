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
        public TagRepository()
        {

        }

        public TagRepository(IEnumerable<Tag> items) : base(items)
        {

        }
    }
}
