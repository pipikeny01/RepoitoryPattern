using MvcTemplate.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MvcTemplate.Service
{
    /// <summary>
    /// 通用行的Service layer實作
    /// </summary>
    /// <typeparam name="T">主要的Entity形態</typeparam>
    public abstract class BaseService<T> : IService<T>
        where T : class
    {
        protected IUnitOfWork _db;

        public BaseService(IUnitOfWork db)
        {
            _db = db;
        }

        public List<TViewModel> SelectToViewModel<TViewModel>(params Expression<Func<T, object>>[] includes)
        {
            var data = _db.Repository<T>().Select();

            foreach (var item in includes)
            {
                data.Include(item);
            }

            return AutoMapper.Mapper.Map<List<TViewModel>>(data);
        }

        /// <summary>
        /// 取得符合條件的Entity並且轉成對應的ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel的形態</typeparam>
        /// <param name="wherePredicate">過濾邏輯</param>
        /// <param name="includes">需要Include的Entity</param>
        /// <returns>取得轉換過的ViewModel List</returns>
        public virtual List<TViewModel> SelectToViewModel<TViewModel>(
            Expression<Func<T, bool>> wherePredicate,
            params Expression<Func<T, object>>[] includes)
        {
            var data = _db.Repository<T>().Select();

            foreach (var item in includes)
            {
                data.Include(item);
            }

            return AutoMapper.Mapper.Map<List<TViewModel>>(data.Where(wherePredicate));
        }

        /// <summary>
        /// 取得某一個條件下面的某一筆Entity並且轉成對應的ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel的形態</typeparam>
        /// <param name="wherePredicate">過濾邏輯</param>
        /// <param name="includes">需要Include的Entity</param>
        /// <returns>取得轉換過的ViewModel或者是null</returns>
        public virtual TViewModel GetDetailToViewModel<TViewModel>(
            Expression<Func<T, bool>> wherePredicate,
            params Expression<Func<T, object>>[] includes)
        {
            var data = _db.Repository<T>().Select();

            foreach (var item in includes)
            {
                data.Include(item);
            }

            return AutoMapper.Mapper.Map<TViewModel>(data.Where(wherePredicate).FirstOrDefault());
        }

        /// <summary>
        /// 依照某一個ViewModel的值，更新對應的Entity
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel的形態</typeparam>
        /// <param name="viewModel">ViewModel的值</param>
        /// <param name="wherePredicate">過濾條件 - 要被更新的那一筆過濾調價</param>
        /// <returns>是否刪除成功</returns>
        public virtual void UpdateViewModelToDatabase<TViewModel>(TViewModel viewModel, Expression<Func<T, bool>> wherePredicate)
        {
            var entity = _db.Repository<T>().GetFirst(wherePredicate);

            AutoMapper.Mapper.Map(viewModel, entity);

            _db.Repository<T>().Update(entity);

            _db.Save();
        }

        /// <summary>
        /// 刪除某一筆Entity
        /// </summary>
        /// <param name="wherePredicate">過濾出要被刪除的Entity條件</param>
        /// <returns>是否刪除成功</returns>
        public virtual void Delete(Expression<Func<T, bool>> wherePredicate)
        {
            var data = _db.Repository<T>().GetFirst(wherePredicate);
            _db.Repository<T>().Delete(data);

            _db.Save();
        }

        /// <summary>
        /// 依照某一個ViewModel的值，產生對應的Entity並且新增到資料庫
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel的形態</typeparam>
        /// <param name="viewModel">ViewModel的Reference</param>
        /// <returns>是否儲存成功</returns>
        public void CreateViewModelToDatabase<TViewModel>(TViewModel viewModel)
        {
            var entity = AutoMapper.Mapper.Map<T>(viewModel);

            _db.Repository<T>().Create(entity);

            _db.Save();
        }
    }
}