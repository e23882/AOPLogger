using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOPLogger
{
    /// <summary>
    /// 套用AOPLogger至Method上的Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AOPLoggerMethodAttribute : Attribute 
    {
    }
}
