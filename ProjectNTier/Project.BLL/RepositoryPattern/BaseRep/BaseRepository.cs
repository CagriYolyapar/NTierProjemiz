using Project.BLL.RepositoryPattern.InterfaceRep;
using Project.BLL.SingletonPattern;
using Project.DAL.Context;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.RepositoryPattern.BaseRep
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        //db isimli Field'imizin protected olmasının sebebi yazıldıgı sınıfın icerisinde ve bu sınıfın miras verdigi sınıfların icerisinde kullanılmak istenmesidir(o field miras verilen yerlerde farklı davranıslara girebilir..)
        protected MyContext db;
        public BaseRepository()
        {
            db = DBTool.DBInstance;
        }
        public void Add(T item)
        {
            db.Set<T>().Add(item);
            Save();
        }

        private void Save()
        {
            db.SaveChanges();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return db.Set<T>().Any(exp);
        }

        public T Default(Expression<Func<T, bool>> exp)
        {
            return db.Set<T>().FirstOrDefault(exp);
        }

        public void Delete(T item)
        {
            item.Status = MODEL.Enums.DataStatus.Deleted;
            item.DeletedDate = DateTime.Now;
            Save();
        }

        public T GetByID(int id)
        {
            return db.Set<T>().Find(id);
        }

        public object ListAnonymus(Expression<Func<T, object>> exp)
        {
            return db.Set<T>().Select(exp).ToList();
        }

        public virtual List<T> SelectActives()
        {
            return db.Set<T>().Where(x => x.Status != MODEL.Enums.DataStatus.Deleted).ToList();
        }

        public List<T> SelectAll()
        {
            return db.Set<T>().ToList();
        }

        public List<T> SelectDeleteds()
        {
            return db.Set<T>().Where(x => x.Status == MODEL.Enums.DataStatus.Deleted).ToList();
        }

        public List<T> SelectModifieds()
        {
            return db.Set<T>().Where(x => x.Status == MODEL.Enums.DataStatus.Updated).ToList();
        }

        public void SpecialDelete(int id)
        {
            db.Set<T>().Remove(GetByID(id));
            Save();
        }

        public void Update(T item)
        {
            item.Status = MODEL.Enums.DataStatus.Updated;
            item.ModifiedDate = DateTime.Now;
            T toBeUpdated = GetByID(item.ID);
            db.Entry(toBeUpdated).CurrentValues.SetValues(item);
            Save();

        }

        public List<T> Where(Expression<Func<T, bool>> exp)
        {
            return db.Set<T>().Where(exp).ToList();
        }
    }
}
