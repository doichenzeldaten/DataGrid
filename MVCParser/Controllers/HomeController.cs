﻿using MVCParser.Models;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;


namespace MVCParser.Controllers
{
    public class HomeController : Controller
    {


        DBModel Db = new DBModel();
        List<Column> Columns = new List<Column>();

        public ActionResult Index()
        {

            return View("Index");
        }


        [HttpPost]
        public ActionResult GetData(List<string> myKey, string sortColumn, string sortDirection, int pageSize)
        {
            bool isSortedColumnExist = false;
            var item = Db.Workers.FirstOrDefault();           
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
            foreach (PropertyDescriptor property in properties)
            {
                if (property.Name==sortColumn)
                {
                    isSortedColumnExist= true;
                    break;

                }

            }
            if (!isSortedColumnExist)
            {
                return HttpNotFound("Incoorect Column Name");
            }
            switch (sortDirection)
            {
                case "ASC":
                    {
                        var totalItemsCount = Db.Workers.Count();
                        var selectedItems = Db.Workers.OrderBy(sortColumn).Skip(0).Take(pageSize).AsParallel().AsOrdered().ToList().HideFields(myKey);
                        List<object> dataToReturn = new List<object>();
                        dataToReturn.Add(selectedItems);
                        dataToReturn.Add(totalItemsCount);                  
                        return Json(dataToReturn, JsonRequestBehavior.AllowGet);
                    }
                case "DESC":
                    {
                        var totalItemsCount = Db.Workers.Count();
                        var selectedItems = Db.Workers.OrderByDescending(sortColumn).Skip(0).Take(pageSize).AsParallel().AsOrdered().ToList().HideFields(myKey);
                        List<object> dataToReturn = new List<object>();
                        dataToReturn.Add(selectedItems);
                        dataToReturn.Add(totalItemsCount);
                        return Json(dataToReturn, JsonRequestBehavior.AllowGet);
                    }
                default:
                    {
                        return HttpNotFound("Incoorect Sort Direction");
                    }
            }
           
            



        }
       private void HideFields<T>(IEnumerable<T> source, List<string> notHiddenFields)
        {
            foreach (var item in source)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
                foreach (PropertyDescriptor property in properties)
                {
                    if (!notHiddenFields.Contains(property.Name))
                    {
                        property.SetValue(item, null);

                    }

                }

            }
        }

        public ActionResult GetAllColumns()
        {
            GetColumnsInfo();
            return Json(Columns, JsonRequestBehavior.AllowGet);

        }
        private void GetColumnsInfo()
        {

            var data = Db.Workers.FirstOrDefault();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(data);
            foreach (PropertyDescriptor property in properties)
            {
                bool isExist = false;
                for (int i = 0; i < Columns.Count; i++)
                {
                    if (Columns[i].Name == property.Name)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    Column col = new Column();
                    col.Name = property.Name;
                    col.Type = property.PropertyType.ToString();
                    Columns.Add(col);
                }
            }
        }

    }
   

  


}

