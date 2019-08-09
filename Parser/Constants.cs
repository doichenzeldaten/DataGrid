﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    static class Constants
    {
        public const int MinLengthMyListThenTrimExcessObjects = 100000;
        public const int IntercolumnShift = 5;
        public const string TextForUndefinedField = "Undefined field";
        public const int FieldToLongСoefficient = 3;
        public const string CuttingStringForTooLongField = "...";
        public const string CheckedFieldOnUIPrefix = "[X] ";
        public const string UncheckedFieldOnUIPrefix = "[ ] ";
    }
}