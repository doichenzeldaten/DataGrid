﻿using MVCGrid.Hubs;
using MVCGrid.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCGrid.Controllers
{

    public class HomeController : Controller
    {

        DataContext _DB = new DataContext();
        public ActionResult Index(int page = 1)
        {

            int pageSize = 10; // количество объектов на страницу          
            IEnumerable<WorkersSmall> workersForPage = _DB.WorkersSmall.OrderBy(k => k.Id).Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = _DB.WorkersSmall.Count() };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Workers = workersForPage };

            return View(ivm);

        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(WorkersSmall worker)
        {
            if (worker == null)
            {
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    _DB.Entry(worker).State = System.Data.Entity.EntityState.Added;
                    _DB.SaveChanges();
                    SendMessage("Добавлен новый работник");
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }

        }
        [HttpGet]
        public ActionResult EditWorker(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            WorkersSmall worker = _DB.WorkersSmall.Find(id);
            if (worker != null)
            {
                return View(worker);
            }
            else
            {
                return HttpNotFound();
            }

        }
        [HttpPost]
        public ActionResult EditWorker([Bind(Include ="Id, FirstName, LastName, Position, Salary")] WorkersSmall worker)
        {
            if (worker == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("Position") && ModelState.IsValidField("Salary"))
                {
                    var workerToSave = _DB.WorkersSmall.Select(k => k).Where(k => k.Id == worker.Id).FirstOrDefault();
                    workerToSave.FirstName = worker.FirstName;
                    workerToSave.LastName = worker.LastName;
                    workerToSave.Position = worker.Position;
                    workerToSave.Salary = worker.Salary;
                    _DB.Entry(workerToSave).State = System.Data.Entity.EntityState.Modified;
                    _DB.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    WorkersSmall worker1 = _DB.WorkersSmall.Find(worker.Id);
                    return View(worker1);
                }
            }
                
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            
                WorkersSmall worker = _DB.WorkersSmall.Find(id);            
                if (worker == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(worker);
                }
           

        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkersSmall worker = _DB.WorkersSmall.Find(id);

            if (worker == null)
            {
                return HttpNotFound();
            }
            else
            {
                _DB.WorkersSmall.Remove(worker);
                _DB.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

       
        [HttpPost]
        public JsonResult JSONWorkerSearch(string firstName)
        {
            var jsondata = _DB.WorkersSmall.Where(a => a.FirstName==firstName).ToList();           
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        private void SendMessage(string message)
        {
           
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();             
            context.Clients.All.displayMessage(message);
        }
        public ActionResult ShowAlcoholics()
        {
            var alcoholics = _DB.WorkersSmall.Where(a => a.IsAlcoholic).ToList();
            List<WorkersSmall> alcoholicsList = new List<WorkersSmall>();
            foreach (var item in alcoholics)
            {
                alcoholicsList.Add(item);
            }
            if (alcoholicsList.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView("PartialShowAlcoholics", alcoholicsList);
        }

       

     
    }
}