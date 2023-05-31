using Etrade.Business.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Business.Concrete
{
    public class Repository<Tcontext, Tentity> : IRepository<Tentity>
        where Tentity : class
        where Tcontext : DbContext, new()
    {

        Tcontext Table { get;set; }
        public Repository()
        {
            Table = new Tcontext();
        }
        public void Add(Tentity entity)
        {


            Table.Add(entity);
            //db.Set<Tentity>().Add(entity)
            //db.Entry(entity).State=EntityState.Added
            Table.SaveChanges();

            
        }

        public void Delete(Tentity entity)
        {

            Table.Remove(entity);
            Table.SaveChanges();
            
        }

        public Tentity Get(int id)
        {
         
                var entity = Table.Find<Tentity>(id);
                //var entity = db.Set<Tentiy>Find(id);
                return entity;
            
        }

        public List<Tentity> GetAll(Expression<Func<Tentity, bool>> filter = null)
        {
           
                //if(filter == null)
                //    return db.Set<Tentity>().ToList();
                //else
                //    return db.Set<Tentity>().Where(filter).ToList();
          
                return filter==null? Table.Set<Tentity>().ToList(): Table.Set<Tentity>().Where(filter).ToList();
            
            
        }

        public void Update(Tentity entity)
        {

            Table.Update(entity);
            Table.SaveChanges ();
            
        }
    }
}
