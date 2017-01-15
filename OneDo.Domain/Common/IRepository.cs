using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Common
{
    public interface IRepository<TAggreagteRoot> where TAggreagteRoot : IAggreagteRoot
    {
        Task<TAggreagteRoot> Get(Guid id);

        Task Add(TAggreagteRoot aggregateRoot);

        Task Update(TAggreagteRoot aggregateRoot);

        Task Delete(Guid id);
    }
}
