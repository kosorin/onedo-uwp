using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Entities
{
#warning Přejmenovat na IEntityData? Upravit i stávající názvy proměnných, např. na entityData
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
