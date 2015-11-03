﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTester.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnIgnoreAttribute: Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class EditColumnIgnoreAttribute : Attribute
    {
    }
}
