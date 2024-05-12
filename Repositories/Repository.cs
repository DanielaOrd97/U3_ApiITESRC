using U3Api.Models.Entities;

namespace Proyecto_U3.Repositories
{
    public class Repository <T> where T : class
    {
        public ItesrcneActividadesContext Context { get; }

        public Repository(ItesrcneActividadesContext context)
        {
            Context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {

            Context.Remove(entity);
            Context.SaveChanges();
        }

    }
}
