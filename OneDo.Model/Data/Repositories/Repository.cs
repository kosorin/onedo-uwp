using OneDo.Model.Data.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data.Repositories
{
    public abstract class Repository<T> where T : class, IModel<T>
    {
        private readonly Dictionary<Guid, T> items;

        protected Repository(IEnumerable<T> items = null)
        {
            this.items = (items ?? new List<T>()).ToDictionary(i => i.Id);
        }

        public void AddOrUpdate(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();
            }

            items[item.Id] = item;
        }

        public void Remove(Guid id)
        {
            items.Remove(id);
        }

        public IEnumerable<T> GetAll()
        {
            return items.Select(i => i.Value);
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return items.Where(i => predicate(i.Value)).Select(i => i.Value);
        }

        public T GetById(Guid id)
        {
            T item;
            return items.TryGetValue(id, out item) ? item : null;
        }

        public T GetCloneById(Guid id)
        {
            return GetById(id)?.Clone();
        }


        public List<T> ToList()
        {
            return GetAll().ToList();
        }
    }
}
