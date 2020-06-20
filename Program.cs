using AOPLogger;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;

namespace GenerateSomeReport
{
    #region Memberfunction
    class Program
    {
        static void Main(string[] args)
        {
            GenerateSomeReport task = new GenerateSomeReport();
			task.GenerateData();
            Console.ReadLine();
        }
    }

    /// <summary>
    /// do something
    /// </summary>
    [AOPLogger]
    public class GenerateData : ContextBoundObject
    {
        #region Declarations
       
        #endregion

        #region Memberfunction
        /// <summary>
        /// 不帶參數建構子
        /// </summary>
        public GenerateLossReportData() { }

        /// <summary>
        /// 產生庫存資料
        /// </summary>
        [AOPLoggerMethod]
        public void GenerateBalanceData()
        {
            try
            {
              
            }
            catch (Exception ie)
            {
                throw new Exception("");
            }
        }

      
        #endregion
    }
    #endregion

    #region Class
    /// <summary>
    /// Send Mail Class
    /// </summary>
    public class MailConnection
    {
        #region Declarations
        MailAddress fromAddress;
        string toAddress;
        string fromPassword = string.Empty;
        string subject = string.Empty;
        string body = string.Empty;
        #endregion

        #region Member Function
        /// <summary>
        /// 寄信類別建構子
        /// </summary>
        /// <param name="SmtpUserName">SMTP Sender Account</param>
        /// <param name="SendMailUserName">SMTP Sender Name</param>
        /// <param name="SmtpUserPassword">SMTP Sender Password</param>
        public MailConnection(string SmtpUserName, string SendMailUserName, string SmtpUserPassword)
        {
            fromAddress = new MailAddress(SmtpUserName, SendMailUserName);
            this.fromPassword = SmtpUserPassword;
        }

        /// <summary>
        /// Set Receiver List
        /// </summary>
        /// <param name="receiverMailAddress">Recipient List</param>
        public void setRecipient(String receiverMailAddress)
        {
            toAddress = receiverMailAddress;
        }

        /// <summary>
        /// 寄信
        /// </summary>
        /// <param name="title">信件標題</param>
        /// <param name="content">信件內容</param>
        public void SendMail(string title, string content)
        {
            var smtp = new SmtpClient
            {
                Host = "mail.xxx.com.tw",
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            var message = new MailMessage();
            message.From = fromAddress;
            message.To.Add(toAddress.ToString());
            message.IsBodyHtml = true;
            message.Subject = title;
            message.Body = "<html><body style='padding:25px 100px;color:black;font-family:微軟正黑體;'><div style='margin-top:50px;margin-bottom:50px;'><h3>" + title + "</h3><div>" + content + "</div></div><footer>本郵件之資訊可能含有機密或特殊管制之資料，僅供指定之收件人使用。若台端非本郵件所指定之收件人，請立即刪除本郵件並通知寄件者。若郵件內容涉及有價證券或金融商品之資訊，其不構成要約、招攬或銷售之任何表示，亦不保證任何收益。網路通訊無法保證本郵件之安全性，若因此造成任何損害，寄件人恕不負責。This email is intended solely for the use of the addressee and may contain confidential and privileged information. If you have received this email in error, please delete the email and notify the sender immediately. If any information contained in this email involves any securities or financial products, it shall not be construed as an offer, solicitation or sale thereof, nor shall it guarantee any earnings. Internet communications cannot be guaranteed to be secure or virus-free; the sender accepts no liability for any errors or omissions.<br></footer></body></html>";
            smtp.Send(message);
        }
        #endregion
    }

    /// <summary>
    /// MS SQL Connection Class
    /// </summary>
    public class Connection
    {
        #region Declarations
        SqlConnection conn;
        private string _ConnectionString = string.Empty;
        IEnumerable<dynamic> result;
        #endregion

        #region Property
        /// <summary>
        /// 連線字串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }
        #endregion

        #region Member Function
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server"></param>
        /// <param name="DB"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="initConnection"></param>
        public Connection(string server, string DB, string userID, string password, bool initConnection = true)
        {
            ConnectionString = $"Data Source = {server}; Initial Catalog = {DB}; User ID = {userID}; Password ={password}";
            if (true)
                conn = new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Execute SQL Query and get result as IEnumerable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> ExecQuery(string query)
        {
            try
            {
                conn.Open();
                result = conn.Query(query);
                return result;
            }
            catch (Exception ie)
            {
                //do nothing,make AOP exceptions everywhere.
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Execute SQL Query
        /// </summary>
        /// <param name="DB">資料庫</param>
        /// <param name="query">SQL指令</param>
        public void ExecuteNonQuery(string Query)
        {
            conn.Open();
            var cmd = new SqlCommand(Query, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteReader();
            conn.Close();
        }
        #endregion
    }
    #endregion
}
