﻿using Parser.Extensions;
using System;
using System.Collections.Generic;

namespace Parser
{

    class ConsolePrinter
    {
        int _SelectedFieldCounter;
        AgregatedKeyList _AllKeys;
        List<ObjectFields> _ParsedObjects;
        List<int> _CheckedMenuItems;
        int _checkedField;
        string[] allSortKeys = { "FirstName", "LastName" };
        public ConsolePrinter(List<ObjectFields> p_parsedObjects)
        {
             
            _AllKeys = new AgregatedKeyList(p_parsedObjects);
            _ParsedObjects = p_parsedObjects;
         _CheckedMenuItems = new List<int>();
        }
        private void PrintLineSeparator(int p_columnLength, int p_columnNumber, int p_columnCount)
        {
            if (p_columnNumber == p_columnCount - 1)
            {
                for (int j = 0; j < p_columnLength; j++)
                {
                    Console.Write("-");
                }
            }
            else
            {
                for (int j = 0; j < p_columnLength + Constants.IntercolumnShift; j++)
                {
                    Console.Write("-");
                }
            }
        }
        private void PrintFieldsAsTableByKeys(List<string> selectedKeys, string sortKey)
        {
            var firstRomNumber = Console.CursorTop;
            var rowCounter = firstRomNumber + 1;
            var columnStartX = 0;
            int totalWidth = 0;
            
            for (int i = 0; i < selectedKeys.Count; i++)
            {
                totalWidth += MaxColumnLength(_ParsedObjects, selectedKeys[i]) + Constants.IntercolumnShift;              
            }
            if (totalWidth > Console.BufferWidth)
            {
                Console.Clear();                              
                Console.BufferWidth = totalWidth;
                Console.WindowHeight=Console.WindowHeight;
                Console.WindowHeight = Console.WindowHeight;
            }            
            for (int i = 0; i < selectedKeys.Count; i++)
            {                             
                Console.SetCursorPosition(columnStartX, rowCounter);
                var maxColumnLength = MaxColumnLength(_ParsedObjects, selectedKeys[i]);
                Console.WriteLine(selectedKeys[i]);
                rowCounter++;
                totalWidth += maxColumnLength+Constants.IntercolumnShift;
                Console.SetCursorPosition(columnStartX, rowCounter);
                PrintLineSeparator(maxColumnLength, i, selectedKeys.Count);                
                rowCounter++;
                Console.SetCursorPosition(columnStartX, rowCounter);
                for (int j = 0; j < _ParsedObjects.Count; j++)
                {
                    Console.SetCursorPosition(columnStartX, rowCounter);
                    if (_ParsedObjects[j].KeyIndexOf(selectedKeys[i]) == -1)
                    {
                        Console.WriteLine(Constants.TextForUndefinedField);
                    }
                    else
                    {
                        if (IsFieldForColumnToLong(_ParsedObjects[j][selectedKeys[i]].Length, selectedKeys[i].Length))
                        {
                          
                            Console.WriteLine(_ParsedObjects[j][selectedKeys[i]].Substring(0, selectedKeys[i].Length * Constants.FieldToLongСoefficient) + Constants.CuttingStringForTooLongField);
                        }
                        else
                        {
                            Console.WriteLine(_ParsedObjects[j][selectedKeys[i]]);
                        }
                    }                            
                    rowCounter = Console.CursorTop;
                }
                columnStartX += maxColumnLength + Constants.IntercolumnShift;
               rowCounter = firstRomNumber + 1;
            }          
        }
        private void PrintCurrentMenuView(List<int> p_CheckedMenuItems, int p_selectedFieldCounter)
        {
            for (int i = 0; i < _AllKeys.Count; i++)
            {
                if (i == p_selectedFieldCounter)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                if (p_CheckedMenuItems.IndexOf(i) == -1)
                {
                    Console.WriteLine(Constants.UncheckedFieldOnUIPrefix + _AllKeys[i]);

                }
                else
                {
                    Console.WriteLine(Constants.CheckedFieldOnUIPrefix + _AllKeys[i]);
                }
            }
        }
        private  List<string> GetSelectedKeysFromUI()
        {

            List<string> selectedKeys = new List<string>();
            int cursorTopPosition = Console.CursorTop;
            Console.CursorVisible = false;
            while (true)
            {
               
                Console.SetCursorPosition(0, cursorTopPosition);
                PrintCurrentMenuView(_CheckedMenuItems, _SelectedFieldCounter);
                Console.BackgroundColor = ConsoleColor.Black;
                ConsoleKey pressedKeyWord = Console.ReadKey().Key;
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (_SelectedFieldCounter < _AllKeys.Count - 1)
                    {
                        _SelectedFieldCounter++;
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (_SelectedFieldCounter > 0)
                    {
                        _SelectedFieldCounter--;
                    }

                    continue;
                }

                if (pressedKeyWord == ConsoleKey.Spacebar)
                {
                    if (_CheckedMenuItems.IndexOf(_SelectedFieldCounter) == -1)
                    {
                        _CheckedMenuItems.Add(_SelectedFieldCounter);

                    }
                    else
                    {
                        _CheckedMenuItems.Remove(_SelectedFieldCounter);

                    }
                    Console.Clear();
                    Console.SetCursorPosition(0, cursorTopPosition);

                    for (int i = 0; i < _AllKeys.Count; i++)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (_CheckedMenuItems.IndexOf(i) != -1)
                        {
                            Console.WriteLine("[X] {0}", _AllKeys[i]);
                        }
                        else
                        {
                            Console.WriteLine("[ ] {0}", _AllKeys[i]);
                        }
                    }
                    break;                    
                }              

                else
                {
                    continue;
                }
            }
           _CheckedMenuItems.Sort();
            for (int i = 0; i < _CheckedMenuItems.Count; i++)
            {                               
                    selectedKeys.Add(_AllKeys[_CheckedMenuItems[i]]);                              
            }
           return selectedKeys;            
        }
        private void PrintCurrentSortKeyMenuView(int p_CheckedMenuItem, int p_selectedFieldCounter)
        {
           
            for (int i = 0; i < allSortKeys.Length; i++)
            {
                if (i == p_selectedFieldCounter)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                if (p_CheckedMenuItem != i)
                {
                    Console.WriteLine(Constants.UncheckedFieldOnUIPrefix + allSortKeys[i]);

                }
                else
                {
                    Console.WriteLine(Constants.CheckedFieldOnUIPrefix + allSortKeys[i]);
                }
            }
        }
        private string GetSortKeysFromUI()
        {                
            int cursorTopPosition = Console.CursorTop;
            Console.CursorVisible = false;
            while (true)
            {

                Console.SetCursorPosition(0, cursorTopPosition);
                PrintCurrentSortKeyMenuView(_checkedField, _SelectedFieldCounter);
                Console.BackgroundColor = ConsoleColor.Black;
                ConsoleKey pressedKeyWord = Console.ReadKey().Key;
                if (pressedKeyWord == ConsoleKey.DownArrow)
                {
                    if (_checkedField<=1)
                    {
                       _checkedField++;
                    }
                    continue;
                }
                if (pressedKeyWord == ConsoleKey.UpArrow)
                {
                    if (_checkedField > 0)
                    {
                        _checkedField--;
                    }

                    continue;
                }

                if (pressedKeyWord == ConsoleKey.Spacebar)
                {
                                                          
                    break;
                }

                else
                {
                    continue;
                }
            }
            return allSortKeys[_checkedField];
        }
        public void PrintJSONObjectsAsTable()
        {
            string Sortkey="";
            bool ftib = true;
            while (true)
            {
                Console.CursorTop = 0;
                for (int i = 0;  i< Constants.PreambleStrings.Length; i++)
                {
                    Console.WriteLine(Constants.PreambleStrings[i]);                  
                }
                if (ftib)
                {
                    Sortkey = GetSortKeysFromUI();
                    ftib = false;
                    Console.Clear();
                    continue;
                   
                }
                List<string> selectedKeys = GetSelectedKeysFromUI();
                PrintFieldsAsTableByKeys(selectedKeys, Sortkey);                         
            }
        }        

        public static int MaxColumnLength(List<ObjectFields> p_Objects, string p_key)
        {
            var maxColumnLength = p_Objects.MaxValue(p_key, GetFieldValueLength);
            var currentColumnLength = p_key.Length;
            if (maxColumnLength > currentColumnLength)
            {
                if (IsFieldForColumnToLong(maxColumnLength, currentColumnLength))
                {
                    currentColumnLength = currentColumnLength * Constants.FieldToLongСoefficient + Constants.CuttingStringForTooLongField.Length;
                }
                else
                {
                    currentColumnLength = maxColumnLength;
                }
            }
            return currentColumnLength;
        }
        public static bool IsFieldForColumnToLong(int p_FieldLength, int p_columnLength)
        {
            if (p_FieldLength > Constants.FieldToLongСoefficient * p_columnLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static int GetFieldValueLength(ObjectFields p_item, string key)
        {
            if (p_item.KeyIndexOf(key) == -1)
            {
                return Constants.TextForUndefinedField.Length;
            }
            else
            {
                return p_item[key].Length;
            }
        }
        // IsValid sounds better. It's more consistent with existing validation API.
        // excessive abstraction. C# is powerful enough to express this logic in a single expression at the place where it should be applied
        // https://trello.com/c/aWrnk1oi/10-c-and-net-platform-features
        public static bool IsInputKeyValid(int keyNumber, int maxKeyValue)
        {
            if (keyNumber <= maxKeyValue && keyNumber >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}