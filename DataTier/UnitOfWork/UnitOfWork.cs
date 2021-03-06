﻿
using DataTier.Models;
using DataTier.Repository;
using System;
using System.Collections.Generic;

namespace DataTier.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly EDMContext _context;
        public UnitOfWork(EDMContext context)
        {
            _context = context;
        }
        Dictionary<Type, object> reposotories = new Dictionary<Type, object>();
        public IGenericRepository<T> Repository<T>()
            where T : class
        {
            Type type = typeof(T);
            if (!reposotories.TryGetValue(type, out object value))
            {
                var GenericRepos = new GenericRepository<T>(_context);
                reposotories.Add(type, GenericRepos);
                return GenericRepos;
            }
            return value as GenericRepository<T>;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
