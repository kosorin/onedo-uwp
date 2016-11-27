using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Async;
using System.Linq.Expressions;

namespace OneDo.Model.Data.Repositories
{
    public class FolderRepository : Repository<Folder>
    {
        private readonly IRepository<Note> noteRepository;

        public FolderRepository(SQLiteAsyncConnection connection, IRepository<Note> noteRepository) : base(connection)
        {
            this.noteRepository = noteRepository;
        }

        public override async Task Delete(Folder entity)
        {
            await noteRepository.DeleteAll(x => x.FolderId == entity.Id);
            await base.Delete(entity);
        }

        public override async Task DeleteAll()
        {
            await RunBulkDelete(async () =>
            {
                var entities = await GetAll();
                foreach (var entity in entities)
                {
                    await noteRepository.DeleteAll(x => x.FolderId == entity.Id);
                    await Delete(entity);
                }
                OnBulkDeleted(entities);
            });
        }

        public override async Task DeleteAll(Expression<Func<Folder, bool>> predicate)
        {
            await RunBulkDelete(async () =>
            {
                var entities = await GetAll(predicate);
                foreach (var entity in entities)
                {
                    await noteRepository.DeleteAll(x => x.FolderId == entity.Id);
                    await Delete(entity);
                }
                OnBulkDeleted(entities);
            });
        }
    }
}
