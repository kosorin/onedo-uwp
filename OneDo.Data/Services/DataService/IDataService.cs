using OneDo.Infrastructure.Repositories;
using System;

namespace OneDo.Infrastructure.Services.DataService
{
    public interface IDataService : IDisposable
    {
        IRepositoryFactory RepositoryFactory { get; }
    }
}