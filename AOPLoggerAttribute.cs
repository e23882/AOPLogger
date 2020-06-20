using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace AOPLogger
{
    /// <summary>
    /// 套用AOPLogger至Class上的Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AOPLoggerAttribute : ContextAttribute, IContributeObjectSink 
    {
        public AOPLoggerAttribute() : base("AOPLogger")
        { }

        /// <summary>
        /// 訊息接收鏈結
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink next)
        {
            return new AOPLoggerHandler(next);
        }
    }
}
