using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Data.Entities
{
    public interface IEntityData
    {
        Guid Id { get; set; }
    }
}
