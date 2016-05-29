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
        private readonly List<T> items;

        protected Repository(List<T> items = null)
        {
            this.items = items ?? new List<T>();
        }


        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();
            }
            items.Add(item);
        }

        public void Remove(Guid id)
        {
            var item = GetById(id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        public IEnumerable<T> GetAll()
        {
            return items;
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return items.Where(predicate);
        }

        public T GetById(Guid id)
        {
            return items.FirstOrDefault(i => i.Id == id);
        }

        public T GetCloneById(Guid id)
        {
            return GetById(id)?.Clone();
        }
    }
}
