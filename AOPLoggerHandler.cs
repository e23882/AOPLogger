using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using NLog;

namespace AOPLogger
{
    public sealed class AOPLoggerHandler : IMessageSink
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();        
        private IMessageSink _nextSink;

        /// <summary>
        /// 取得接收鏈結中的下一個訊息接收
        /// </summary>
        public IMessageSink NextSink
        {
            get { return _nextSink; }
        }

        public AOPLoggerHandler(IMessageSink nextSink)
        {
            this._nextSink = nextSink;
        }

        /// <summary>
        /// 同步處理方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage message = null;

            //方法呼叫訊息介面
            IMethodCallMessage call = msg as IMethodCallMessage;

            //如果被呼叫的方法為AOPLoggerMethodAttribute標籤
            if (call != null && (Attribute.GetCustomAttribute(call.MethodBase, typeof(AOPLoggerMethodAttribute))) != null)
            {                  
                //取得完整Function名稱(含Namespace)      
                var method = (System.Runtime.Remoting.Messaging.IMethodMessage)msg;
                var fullName = string.Format("{0}.{1}()", method.MethodBase.DeclaringType.ToString(), method.MethodName);

                //JoinPoint,Method執行前
                _logger.Trace(string.Format("[FuncBegin] {0}", fullName));                

                //執行原方法
                message = _nextSink.SyncProcessMessage(msg);

                //JoinPoint,Metho發生Exception
                IMethodReturnMessage methodReturn = message as IMethodReturnMessage;
                if (methodReturn.Exception != null)
                    _logger.TraceException($"[Exception] Execute {fullName}\r\nException {methodReturn.Exception.Message}\r\nStackTrace {methodReturn.Exception.StackTrace}", methodReturn.Exception);

                //JoinPoint,Method執行後
                _logger.Trace(string.Format("[FuncEnd] {0}", fullName));                
            }
            else
                message = _nextSink.SyncProcessMessage(msg);
            
            return message;
        }

        /// <summary>
        /// 非同步處理方法
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="replySink"></param>
        /// <returns></returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }
    }
}
