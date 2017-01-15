using OneDo.Data.Repositories;
using System;

namespace OneDo.Data.Services.DataService
{
    public interface IDataService : IDisposable
    {
        IRepositoryFactory RepositoryFactory { get; }
    }
}