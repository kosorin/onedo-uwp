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
        private readonly Dictionary<Guid, T> items = new Dictionary<Guid, T>();

        protected Repository()
        {

        }

        protected Repository(IEnumerable<T> items)
        {
            if (items != null)
            {
                this.items = items.ToDictionary(i => i.Id);
            }
        }


        public void Add(T item)
        {
            TrySetId(item);
            items[item.Id] = item;
        }

        public void AddAll(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                TrySetId(item);
                this.items[item.Id] = item;
            }
        }


        public T Get(Guid id)
        {
            T item;
            return items.TryGetValue(id, out item) ? item : null;
        }

        public T Get(Func<T, bool> predicate)
        {
            return items.Values.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return items.Values;
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return items.Values.Where(predicate);
        }


        public void Remove(Guid id)
        {
            items.Remove(id);
        }

        public void RemoveAll()
        {
            items.Clear();
        }


        private void TrySetId(T item)
        {
            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();
            }
        }
    }
}
