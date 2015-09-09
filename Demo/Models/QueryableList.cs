using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Demo.Models
{
    public interface IQueryableList<T> : IList<T>, ICollection<T>, IQueryable<T>
    {
        void AddRange(IEnumerable<T> collection);
        Task<T> FindAsync(string key);
    }

    public abstract class QueryableList<T> : IQueryableList<T>
    {
        protected List<T> list = new List<T>();

        public T this[int index]
        {
            get
            {
                return this.list[index];
            }

            set
            {
                this.list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public Type ElementType
        {
            get
            {
                return this.list.AsQueryable().ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return this.list.AsQueryable().Expression;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return this.list.AsQueryable().Provider;
            }
        }

        public void Add(T item)
        {
            this.list.Add(item);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(T item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(this.list.ToArray<T>(), arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.list.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return this.list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public void AddRange(IEnumerable<T> collection)
        {
            this.list.AddRange(collection);
        }

        // this method must be override in child class.
        public virtual Task<T> FindAsync(string key)
        {
            return null;
        }
    }

    public class PeopleList : QueryableList<Person>
    {
        public override Task<Person> FindAsync(string key)
        {
            Task<Person> task = new Task<Person>(new Func<Person>(() => {
                Person entity = list.Where(p => p.ID == key).First();
                return entity;
            }));

            task.Start();

            return task;
        }            
    }

    public class TripList : QueryableList<Trip>
    {
        public override Task<Trip> FindAsync(string key)
        {
            Task<Trip> task = new Task<Trip>(new Func<Trip>(() => {
                Trip entity = list.Where(p => p.ID == key).First();
                return entity;
            }));

            task.Start();

            return task;
        }
    }
}