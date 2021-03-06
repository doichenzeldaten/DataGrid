﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    partial class MyDataGrid
    {
        private async void SortData()
        {
            if (_CancellationTokenSourceForSorting != null)
            {
                _CancellationTokenSourceForSorting.Cancel();
            }
            _CancellationTokenSourceForSorting = new CancellationTokenSource();
            ShowProgressScrren();
            List<object> sortedItems = new List<object>();
            Task t1 = Task.Factory.StartNew(() =>
             {
               
                 try
                 {
                     IsSortingFinished = false;

                     sortedItems = TooggleSorting(_CurrentPage.SkipElementsCount, _CurrentPage.TakeElementsCount, _CancellationTokenSourceForSorting.Token)?.ToList();
                     IsSortingFinished = true;
                 }
                 catch (Exception ex)
                 {

                 }
             }, _CancellationTokenSourceForSorting.Token);
            Task t2 = Task.Factory.StartNew(() =>
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                foreach (var item in _Header)
                {
                    item.Invalidate();
                }
                _ProgressScreen.RunProgressBar(cts.Token);
                while (!IsSortingFinished && !_CancellationTokenSourceForSorting.IsCancellationRequested)
                {
                    continue;
                }
                if (IsSortingFinished || _CancellationTokenSourceForSorting.IsCancellationRequested)
                {
                    cts.Cancel();
                    foreach (var item in _Header)
                    {
                        item.Invalidate();
                    }
                }

            }, _CancellationTokenSourceForSorting.Token);
            try
            {
                await Task.WhenAll(new[] { t1, t2 });
            }
            catch
            { 
            
            }
            if (IsSortingFinished)
            {
                RemoveProgressScreen(true);
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

        }
      
        private IQueryable<object> TooggleSorting(int skipCount, int takeCount, CancellationToken token)
        {

            if (skipCount < 0)
            {
                skipCount = 0;
            }

            if (_API.SortedColumnIndex != -1)
            {
                if (_API.SortDirection == SortDirections.ASC)
                {
                    var items = _ItemsSource?.OrderBy(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsParallel().AsOrdered().WithCancellation(token).AsQueryable();
                    return items;
                }
                if (_API.SortDirection == SortDirections.DESC)
                {
                    var items = _ItemsSource?.OrderByDescending(_API.Columns[_API.SortedColumnIndex].HeaderText).Skip(skipCount).Take(takeCount).AsParallel().AsOrdered().WithCancellation(token).AsQueryable();
                    return items;
                }

                else if (_API.SortDirection == SortDirections.None)
                {
                    var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                    var items = _ItemsSource?.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsParallel().AsOrdered().WithCancellation(token).AsQueryable();
                    return items;
                }
            }
            else
            {
                var sortedColumn = _API.Columns.Select(k => k).Where(k => k.HeaderText == PrivateKeyColumn).FirstOrDefault();
                var items = _ItemsSource?.OrderBy(sortedColumn.HeaderText).Skip(skipCount).Take(takeCount).AsParallel().AsOrdered().WithCancellation(token).AsQueryable();
                return items;
            }
            return _ItemsSource;
        }
    }
}
