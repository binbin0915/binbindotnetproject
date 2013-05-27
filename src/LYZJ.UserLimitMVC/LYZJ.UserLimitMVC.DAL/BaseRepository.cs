using System.Data;

using LYZJ.UserLimitMVC.Model;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;
using LYZJ.UserLimitMVC.IDAL;



namespace LYZJ.UserLimitMVC.DAL
{

    /// <summary>

    /// 实现对数据库的操作(增删改查)的基类

    /// </summary>

    /// <typeparam name="T">定义泛型，约束其是一个类</typeparam>

    public class BaseRepository<T> where T : class
    {

        //创建EF框架的上下文

        private DataModelContainer db = new DataModelContainer();



        // 实现对数据库的添加功能,添加实现EF框架的引用

        public T AddEntity(T entity)
        {

            //EF4.0的写法   添加实体

            //db.CreateObjectSet<T>().AddObject(entity);

            //EF5.0的写法

            db.Entry<T>(entity).State = EntityState.Added;



            //下面的写法统一

            db.SaveChanges();

            return entity;

        }



        //实现对数据库的修改功能

        public bool UpdateEntity(T entity)
        {

            //EF4.0的写法

            //db.CreateObjectSet<T>().Addach(entity);

            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

            //EF5.0的写法

            db.Set<T>().Attach(entity);

            db.Entry<T>(entity).State = EntityState.Modified;



            return db.SaveChanges() > 0;

        }



        //实现对数据库的删除功能

        public bool DeleteEntity(T entity)
        {

            //EF4.0的写法

            //db.CreateObjectSet<T>().Addach(entity);

            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Deleted);

            //EF5.0的写法

            db.Set<T>().Attach(entity);

            db.Entry<T>(entity).State = EntityState.Deleted;



            return db.SaveChanges() > 0;

        }



        //实现对数据库的查询  --简单查询

        public IQueryable<T> LoadEntities(Func<T, bool> whereLambda)
        {

            //EF4.0的写法

            //return db.CreateObjectSet<T>().Where<T>(whereLambda).AsQueryable();

            //EF5.0的写法

            return db.Set<T>().Where<T>(whereLambda).AsQueryable();

        }



        /// <summary>

        /// 实现对数据的分页查询

        /// </summary>

        /// <typeparam name="S">按照某个类进行排序</typeparam>

        /// <param name="pageIndex">当前第几页</param>

        /// <param name="pageSize">一页显示多少条数据</param>

        /// <param name="total">总条数</param>

        /// <param name="whereLambda">取得排序的条件</param>

        /// <param name="isAsc">如何排序，根据倒叙还是升序</param>

        /// <param name="orderByLambda">根据那个字段进行排序</param>

        /// <returns></returns>

        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out  int total, Func<T, bool> whereLambda,

            bool isAsc, Func<T, S> orderByLambda)
        {

            //EF4.0和上面的查询一样

            //EF5.0

            var temp = db.Set<T>().Where<T>(whereLambda);

            total = temp.Count(); //得到总的条数

            //排序,获取当前页的数据

            if (isAsc)
            {

                temp = temp.OrderBy<T, S>(orderByLambda)

                     .Skip<T>(pageSize * (pageIndex - 1)) //越过多少条

                     .Take<T>(pageSize).AsQueryable(); //取出多少条

            }

            else
            {

                temp = temp.OrderByDescending<T, S>(orderByLambda)

                    .Skip<T>(pageSize * (pageIndex - 1)) //越过多少条

                    .Take<T>(pageSize).AsQueryable(); //取出多少条

            }

            return temp.AsQueryable();

        }

    }

}