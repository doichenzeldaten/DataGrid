﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGrid.Models
{
    public interface IRepository<T> : IDisposable where T: class
    {
        IEnumerable<T> GetWorkers(); // получение всех объектов
        T GetWorker(int id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        void Save();  // сохранение изменений
    }
    public class Repository : IRepository<WorkersSmall>

    {
        DataContext db = new DataContext();
        public void Create(WorkersSmall item)
        {
            db.WorkersSmall.Add(item);
        }     

        public void Delete(int id)
        {
            WorkersSmall worker = db.WorkersSmall.Find(id);
            if (worker != null)
                db.WorkersSmall.Remove(worker);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<WorkersSmall> GetWorkers()
        {
            return db.WorkersSmall;
        }

        public WorkersSmall GetWorker(int id)
        {
            return db.WorkersSmall.Find(id); 
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(WorkersSmall item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}