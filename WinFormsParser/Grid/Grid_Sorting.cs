﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Parser
{
    partial class MyDataGrid
    {
        private async Task<List<object>> AsyncToggleSorting(int skipCount, int takeCount, CancellationToken token)
        {

            IsSortingFinished = false;
            List<object> list = new List<object>();
            await Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(500);
                    list = TooggleSorting(skipCount, takeCount)?.ToList();
                }
                catch (Exception ex)
                {

                }
            }, token);
            IsSortingFinished = true;
            return list;
           

        }
        CancellationTokenSource cts2;

        private async void SortData()
        {
            if (cts2 != null)
            {
                cts2.Cancel();
            }
            
            cts2 = new CancellationTokenSource();
            

            List<object> sortedItems = new List<object>();
            Task t1= Task.Factory.StartNew(async delegate
            {
              
                try
                {
                  
                    IsSortingFinished = false;
                    sortedItems = TooggleSorting(_CurrentPage.SkipElementsCount, _CurrentPage.TakeElementsCount)?.ToList();                 


                    IsSortingFinished = true;
                }
                catch (Exception ex)
                {

                }
            },cts2.Token);

            Task t2 = Task.Factory.StartNew(async delegate
            {

            while (!IsSortingFinished)
            {
                this.BackColor = Color.Black;

            }
                if (IsSortingFinished)
                {


                    this.BackColor = Color.White;
                    
                   

                }

            }, cts2.Token);
            await Task.WhenAll(new[] { t1, t2 });         
            Dictionary<string, Type> columns = GetColumnsInfo();
            int index = 0;
            foreach (var item in columns)
            {
                List<string> ColumnItems = new List<string>();
                ColumnItems = GetColumnItemsFromSource(item.Key, item.Value, sortedItems);
                List<string> viewPortItems = new List<string>();
                var a = _Source[index].First();
                _Source[index].Clear();
                _Source[index].Add(a);
                _Source[index].AddRange(ColumnItems);
                _Source[index].AddRange(viewPortItems);
                index++;
            }
            _Buffer.Clear();
            for (int j = 0; j < _Source.Count; j++)
            {
                AddToBufer(_Source[j].First());
            }
            UpdateColumnsPosition();
            UpdateHeadersWidth();
            RecalculateTotalTableWidth();
           Invalidate();

        }
        private IQueryable<object> TooggleSorting(int skipCount, int takeCount)
        {

            if (skipCount < 0)
            {
                skipCount = 0;
            }

            if (_API.SortedColumnIndex != -1)
            {
                if (_API.SortDirection == SortDirections.ASC)
                {
                    var items = _ItemsSource?.OrderBy(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }
                if (_API.SortDirection == SortDirections.DESC)
                {
                    var items = _ItemsSource?.OrderByDescending(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }

                else if (_API.SortDirection == SortDirections.None)
                {
                    var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                    var items = _ItemsSource?.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                    return items;
                }
            }
            else
            {
                var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                var items = _ItemsSource?.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsQueryable();
                return items;
            }
            return _ItemsSource;
        }
    }
}