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
        private readonly Dictionary<Guid, T> data = new Dictionary<Guid, T>();

        protected Repository()
        {

        }

        protected Repository(IEnumerable<T> items)
        {
            if (items != null)
            {
                data = items.ToDictionary(i => i.Id);
            }
        }


        public void Add(T item)
        {
            SetIdIfNotSet(item);
            data[item.Id] = item;
        }

        public void AddAll(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                SetIdIfNotSet(item);
                data[item.Id] = item;
            }
        }


        public T Get(Guid id)
        {
            T item;
            return data.TryGetValue(id, out item) ? item : null;
        }

        public T Get(Func<T, bool> predicate)
        {
            return data.Values.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return data.Values;
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return data.Values.Where(predicate);
        }


        public void Remove(Guid id)
        {
            data.Remove(id);
        }

        public void RemoveAll()
        {
            data.Clear();
        }


        private void SetIdIfNotSet(T item)
        {
            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();
            }
        }
    }
}
