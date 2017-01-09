using OneDo.Data.Repositories;
using System;

namespace OneDo.Application.Services.DataService
{
    public interface IDataService : IDisposable
    {
        IRepositoryFactory RepositoryFactory { get; }
    }
}