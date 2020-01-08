﻿using System;
using System.Collections.Generic;
using Parser.Properties;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Parser
{



    public partial class MyDataGrid : UserControl
    {
      
        List<List<string>> _Source;
        List<Row> _Bufer;       
        APICore _API;
        int _RowHeight;
        int _LineWidth = 1;
        int _FirstPrintedRowIndex = 0;
        int _VerticalScrollValueRatio = 10;
        int _HorisontalScrollValueRatio = 1;
        int _CellMinMargin = 2;
        int _TotalRowsCount;
        int _ViewPortRowsCount;
        float _TableWidth;
        Brush _Brush;
        Pen _Pen;
        public MyDataGrid()
        {
            AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            components = new System.ComponentModel.Container();
            _Source = new List<List<string>>();
            _Bufer = new List<Row>();           
            _API = new APICore();
            _API.PropertyChanged += APIPropertyChanged;
            Columns.CollectionChanged += ColumnsCollectionChanged;
            ResizeRedraw = true;         
            VerticalScrollBar.Minimum = 0;
            VerticalScrollBar.Value = 0;
            if (VerticalScrollBar.Visible)
            {
                HorisontalScrollBar.Width = this.ClientSize.Width - VerticalScrollBar.Width;
            }
            else
            {
                HorisontalScrollBar.Width = this.Width;
            }
            HorisontalScrollBar.Maximum = 0;
            HorisontalScrollBar.Minimum = 0;
            HorisontalScrollBar.Value = 0;
            _Brush = new SolidBrush(ForeColor);
            _Pen = new Pen(LineColor, _LineWidth);

        }

        //!why internal?
        internal ObservableCollection<Column> Columns
        {
            get
            {
                return _API.Columns;
            }

        }      
        [DisplayName(@"Source")]
        public object Source
        {
            get => _Source;
            set
            {
                _Source = (List<List<string>>)value;

            }
            
        }
        [DisplayName(@"ColumnsAutoGeneretion"), Description("Если value=true - колонки генерируются автоматически из коллекции Source"), DefaultValue(false)]
        public bool ColumnsAutoGeneretion { get; set; } = false;
        public Color LineColor { get; set; }
        public int RowHeight
        {
            get => _RowHeight;
            set
            {
                if (value > Font.Height + 2 * _CellMinMargin + _LineWidth)
                {
                    _RowHeight = value;
                    //!why are you recreating the buffer on changing the property?
                    UpdateControl();
                }
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                _RowHeight = (_RowHeight < Font.Height + 2 * _CellMinMargin + _LineWidth) ? (Font.Height + 2 * _CellMinMargin + _LineWidth) : _RowHeight;
                //!why are you recreating the buffer on changing the property?
                UpdateControl();
            }
        }




        public void UpdateBufer()
        {
            _Bufer.Clear();
            int rowCounts = _Source.First().Count();
            for (int j = 0; j < rowCounts; j++)
            {
                _Bufer.Add(new Row());
            }      
            for (int i = 0; i < _API.Columns.Count; i++)
            {             
                int index = 0;
                for (int j = 0; j < _Source.Count; j++)
                {
                    if (_API.Columns[i].HeaderText == _Source[j].First())
                    {
                        index = j;
                    }
                }        
                for (int k = 0; k < _Source[index].Count; k++)
                {
                    Cell temp = new Cell();
                    temp.Body = _Source[index][k];                  
                    temp.SourceXIndex = index;                        
                    temp.SourceYIndex = k;
                    _Bufer[k].Cells.Add(temp);
                }
            }                                 
        }
        private void UpdateControl()
        {
           
            if (_API.Columns.Count > 0)
            {
                UpdateBufer();
                _TotalRowsCount = _Bufer.Count;
                _ViewPortRowsCount = (this.Height) / (RowHeight) - 1;
                if (_TableWidth > this.Width)
                {                   
                    var remainder = _RowHeight % HorisontalScrollBar.Height;
                    if (remainder != 0)
                    {
                        _ViewPortRowsCount--;
                    }
                }
                if (_TotalRowsCount > _ViewPortRowsCount)
                {
                    VerticalScrollBar.Visible = true;
                }
                VerticalScrollBar.Minimum = 0;
                VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio) - 1;
                VerticalScrollBar.SmallChange = _VerticalScrollValueRatio;
                VerticalScrollBar.LargeChange = _VerticalScrollValueRatio;
                UpdateHorizontalScroll();
                HorisontalScrollBar.Value = 0;
                HorisontalScrollBar.SmallChange = _HorisontalScrollValueRatio;
                HorisontalScrollBar.LargeChange = _HorisontalScrollValueRatio;
                Invalidate();
                for (int i = 0; i < Controls.Count; i++)
                {
                    Type Type = Controls[i].GetType();

                    if (Type == typeof(TypeSelector))
                    {
                        Controls[i].Visible = false;
                    }
                }
            }
            else
            {
                //!why?
                Invalidate();
            }
        }     
        
        private void APIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SortDirection")
            {
                Invalidate();
            }
        }
        private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visible")
            {
                Invalidate();
            }
        }
        private void ColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
               
                UpdateControl();
                foreach (var item in Columns)
                {
                    if (!item.IsSignedToPropertyChange)
                    {
                        item.PropertyChanged += ColumnPropertyChanged;
                        item.IsSignedToPropertyChange = true;
                    }
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //!why are you performing such a major refresh on hiding just a column?
                UpdateControl();
            }
        }
        public void ChangeSorting(string columnName, Sort sortDirection)
        {
            var newSortedColumn = Columns.Select(k => k).Where(u => u.HeaderText == columnName).Single();
            if (newSortedColumn.Visible)
            {
                _API.SortDirection = sortDirection;
                _API.SortedColumnIndex = newSortedColumn.Index;
            }
        }
       
        protected override void OnPaint(PaintEventArgs e)
        {
            _Pen.Color = LineColor;
            _TableWidth = 0;
            foreach (var APIColumn in _API.Columns)
            {
                int index = 0;
                for (int i = 0; i < _Bufer.First().Cells.Count; i++)
                {
                    if (APIColumn.HeaderText == _Bufer.First().Cells[i].Body)
                    {
                        index = i;
                    }
                }              
                var columnItems = _Bufer.Select(k => k.Cells[index].Body).ToList();
                int columnWidth = (columnItems.Max(k => k.Length) > APIColumn.HeaderText.Length) ? columnItems.Max(k => k.Length) : APIColumn.HeaderText.Length;
                APIColumn.Width = columnWidth;
                if (APIColumn.Visible)
                {
                    _TableWidth += (_LineWidth + _CellMinMargin + APIColumn.Width * (int)this.Font.Size + _CellMinMargin);
                }
            }
            foreach (var item in _API.Columns)
            {
               
            }
            UpdateHorizontalScroll();
            DrawTable(e);

        }


        private void DrawOutsideFrame(PaintEventArgs e)
        {
            e.Graphics.DrawLine(_Pen, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(_Pen, this.Width, 0, this.Width, this.Height);
            e.Graphics.DrawLine(_Pen, this.Width, this.Height, 0, this.Height);
            e.Graphics.DrawLine(_Pen, 0, this.Height, 0, 0);
        }

        private void DrawTable(PaintEventArgs e)
        {
            DrawOutsideFrame(e);
            if (_Bufer.Count != 0 && _Bufer.First().Cells.Count != 0 && Columns.Count > 0)
            {
                _API.SortColumns();
                Row firstRowBufer = new Row();
                firstRowBufer = _Bufer.First();
                int sortedIndex = 0;
                if (_API.SortedColumnIndex != -1)
                {
                    for (int k = 0; k < _Bufer.First().Cells.Count; k++)
                    {

                        if (_API.Columns[_API.SortedColumnIndex].HeaderText == _Bufer.First().Cells[k].Body)
                        {
                            sortedIndex = k;
                        }
                    }
                }
                _Bufer.RemoveAt(0);               
                if (_API.SortDirection != Sort.None)
                {
                    RowComparer u = (_API.SortDirection == Sort.ASC) ? new RowComparer(true, sortedIndex, _API.Columns[_API.SortedColumnIndex].Type) : new RowComparer(false, sortedIndex, _API.Columns[_API.SortedColumnIndex].Type);
                   _Bufer.Sort(u);
                }
                else if (_API.SortDirection == Sort.None)
                {                   
                    //!why are you using lambda instead of a seaprate row comparer here? or, why do you use the row comparer in the code above?
                    _Bufer.Sort((a, b) => a.Cells.First().SourceYIndex.CompareTo(b.Cells.First().SourceYIndex));
                }
                _Bufer.Insert(0, firstRowBufer);         

                if (_ViewPortRowsCount > _Bufer.Count - 1)
                {
                    _ViewPortRowsCount = _Bufer.Count - 1;
                }
                int xCounterForLine = 0;           
 
                Control HorizontalScroll = Controls[0];
                Control VerticalScroll = Controls[1];
                Controls.Clear();
                Controls.Add(HorizontalScroll);
                Controls.Add(VerticalScroll);
                int xCounter = 0;
                int xCounterForText = _LineWidth + _CellMinMargin;
                List<HeaderCell> Header = new List<HeaderCell>();
                for (int i = 0; i < _API.Columns.Count; i++)
                {
                    int bufferRowIndex = _FirstPrintedRowIndex + 1;
                    int viewPortRowIndex = 1;
                    if (_API.Columns[i].Visible)
                    {
                        HeaderCell Cell = new HeaderCell(_API.Columns[i]);
                        Cell.Font = this.Font;
                        Cell.Width = (int)(_LineWidth * 2 + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin);
                        Cell.Height = _RowHeight;
                        Cell.Location = new Point(xCounter - HorisontalScrollBar.Value, 0);
                        Cell._API = _API;
                        xCounter += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                        Header.Add(Cell);
                        Controls.Add(Cell);
                        for (int j = 0; j < _ViewPortRowsCount; j++)
                        {
                            if (bufferRowIndex < _Bufer.Count)
                            {
                                int index = 0;
                                for (int k = 0; k < _Bufer.First().Cells.Count; k++)
                                {
                                    if (_API.Columns[i].HeaderText == _Bufer.First().Cells[k].Body)
                                    {
                                        index = k;
                                    }
                                }                                
                                Cell TCell = _Bufer[bufferRowIndex].Cells[index];
                                e.Graphics.DrawString(TCell.Body, this.Font, _Brush, xCounterForText - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex) + (RowHeight - FontHeight) / 2);       
                                e.Graphics.DrawLine(_Pen, -HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1), _TableWidth - HorisontalScrollBar.Value, RowHeight * (viewPortRowIndex + 1));
                                viewPortRowIndex++;
                                bufferRowIndex++;
                            }
                        }
                        e.Graphics.DrawLine(_Pen, xCounterForLine - HorisontalScrollBar.Value, 0, xCounterForLine - HorisontalScrollBar.Value, RowHeight * (_ViewPortRowsCount + 1));
                        xCounterForLine += _LineWidth + _CellMinMargin + _API.Columns[i].Width * (int)this.Font.Size + _CellMinMargin;
                    }
                    xCounterForText += _API.Columns[i].Width * (int)Font.Size + _CellMinMargin + _LineWidth + _CellMinMargin;
                    foreach (var headerCell in Header)
                    {
                        List<HeaderCell> temp = new List<HeaderCell>();
                        var OtherCells = Header.Select(k => k).Where(k => k != headerCell).ToList();
                        headerCell.NeighborCells.Clear();
                        headerCell.NeighborCells.AddRange(OtherCells);
                    }
                }
                e.Graphics.DrawLine(_Pen, _TableWidth - HorisontalScrollBar.Value, 0, _TableWidth - HorisontalScrollBar.Value, 0 + RowHeight * (_ViewPortRowsCount + 1));
                e.Graphics.DrawLine(_Pen, 0 - HorisontalScrollBar.Value, RowHeight, _TableWidth - HorisontalScrollBar.Value, RowHeight);
            }
        }
          
       
       
        
        private void UpdateHorizontalScroll()
        {
            var viewportWidth = this.ClientSize.Width - (VerticalScrollBar.Visible ? VerticalScrollBar.Width : 0);
            if (_TableWidth >= viewportWidth)
            {
                HorisontalScrollBar.Visible = true;
                HorisontalScrollBar.Maximum = (int)(_TableWidth - viewportWidth + 1);
            }
            else
            {
                HorisontalScrollBar.Visible = false;
                HorisontalScrollBar.Maximum = 0;
            }
       }
     

        class Cell
        {
            public string Body { get; set; }          
            public int SourceXIndex { get; set; }
            public int SourceYIndex { get; set; }
           
        }
        class Row
        {
            public List<Cell> Cells { get; set; } = new List<Cell>();

        }
        class RowComparer : IComparer<Row>
        {
            bool _Direction;
            int _ColumnIndex;
            Type _Type;
            public RowComparer(bool direction, int index, Type t)
            {
                _Direction = direction;
                _ColumnIndex = index;
                _Type = t;
            }
            public int Compare(Row x, Row y)
            {
                object xValue=null;
                object yValue=null;
                if (x.Cells[_ColumnIndex].Body != Resources.UndefinedFieldText)
               {
                    try
                    {
                        xValue = Convert.ChangeType(x.Cells[_ColumnIndex].Body, _Type);
                    }
                    catch
                    {
                        xValue = null; 
                    }
               }
                if (y.Cells[_ColumnIndex].Body != Resources.UndefinedFieldText)
                {
                    try
                    {
                        yValue = Convert.ChangeType(y.Cells[_ColumnIndex].Body, _Type);
                    }
                    catch
                    {
                        yValue = null;
                    }
                }

                int dir = (_Direction)?1:-1;           
                              
                if (xValue != null && xValue is IComparable)
                {

                    return (xValue as IComparable).CompareTo(yValue) * ((_Direction) ? 1 : -1);
                }
                if (yValue != null && xValue is IComparable)
                {
                    return (yValue as IComparable).CompareTo(xValue) * ((_Direction) ? 1 : -1);
                }
                else
                {
                    return (x.Cells[_ColumnIndex].Body as string).CompareTo(y.Cells[_ColumnIndex].Body as string) * ((_Direction) ? 1 : -1);
                }        

            }
        }        
        
        private void MyDataGrid_Resize(object sender, EventArgs e)
        {
            if (_Bufer.Count != 0)
            {
                _ViewPortRowsCount = this.Height / (RowHeight) - 1;
                if (_TableWidth > this.Width-VerticalScrollBar.Width)
                {
                    var hidenRowsCount = _RowHeight / HorisontalScrollBar.Height;
                    var remainder = _RowHeight % HorisontalScrollBar.Height;
                    if (remainder != 0)
                    {
                        _ViewPortRowsCount--;
                    }
                 
                }
                if (_TotalRowsCount > _ViewPortRowsCount + 1)
                {
                    VerticalScrollBar.Visible = true;
                }
                else
                {
                    VerticalScrollBar.Visible = false;
                }
                if (VerticalScrollBar.Value < 0)
                {
                    VerticalScrollBar.Minimum = 0;
                    VerticalScrollBar.Value = 0;
                }
                VerticalScrollBar.Maximum = ((_TotalRowsCount - _ViewPortRowsCount) * _VerticalScrollValueRatio - 1);
            }          
            UpdateHorizontalScroll();
            Invalidate();
        }
        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            _FirstPrintedRowIndex = VerticalScrollBar.Value / _VerticalScrollValueRatio;
            if (VerticalScrollBar.Value < 0)
            {
                _FirstPrintedRowIndex = 0;
            }
            Invalidate();
        }    
        private void VerticalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            if (!VerticalScrollBar.Visible)
            {
                HorisontalScrollBar.Width = this.Width;
            }
            else
            {
                HorisontalScrollBar.Width = this.Width - VerticalScrollBar.Width;
            }
        }
        private void HorisontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void MyDataGrid_Load(object sender, EventArgs e)
        {
           
        }

        private void MyDataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            var X = e.Location.X+HorisontalScrollBar.Value;
            var Y = e.Location.Y;
            int xend = 0; 
            int xstart = 0;
            foreach (var item in _API.Columns)
            {
                if (item.Visible)
                {
                xend += (int)(item.Width * this.Font.Size + _CellMinMargin * 2 + _LineWidth);
                if (xstart < X && X < xend)
                {
                    int ItemIndex = Y / RowHeight + _FirstPrintedRowIndex;
                    MessageBox.Show("Name: " + item.HeaderText + "\n" + "Type: " + item.Type + "\n" + "ItemIndex: " + ItemIndex);
                    if (item.Type == typeof(string))
                    {
                       // item.Items[ItemIndex] = "Опа";
                    }
                    if (item.Type == typeof(int))
                    {
                      //  var a = 007;
                       // item.Items[ItemIndex] = a.ToString();
                    }
                }
            }
                xstart = xend;

            }
            UpdateControl();
            Invalidate();
        }
    }
 
     
   



    
}

