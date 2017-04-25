using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sodao.Dapper
{
    public class DataContextConfig
    {
        #region Static Private Members
        static private long _times = 0;
        static private string _connNmeOfMaster = "master";
        static private string[] _arrConnNameOfSecondary;
        static private int _secondaryCount;
        #endregion

        #region Private Member
        /// <summary>
        /// 主库
        /// </summary>
        private string Master
        {
            get { return _connNmeOfMaster; }
        }
        /// <summary>
        /// 从库
        /// </summary>
        private string Secondary
        {
            get
            {
                return _arrConnNameOfSecondary[(int)(Interlocked.Increment(ref _times) % _secondaryCount)];
            }
        }
        #endregion

        #region Single Instance
        static private readonly object lockHelper = new object();
        static private volatile DataContextConfig _Default;
        static public DataContextConfig Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (lockHelper)
                    {
                        _Default = new DataContextConfig();
                    }
                }
                return _Default;
            }
        }
        #endregion

        #region Construct Method
        /// <summary>
        /// 构造函数
        /// </summary>
        private DataContextConfig()
        {
            //查找从库设置
            var list = new List<string>();
            foreach (ConnectionStringSettings child in ConfigurationManager.ConnectionStrings)
            {
                if (child.Name.StartsWith("secondary"))
                    list.Add(child.Name);
            }
            if (list.Count == 0)
                throw new ArgumentException("can't find any secondary.");

            _arrConnNameOfSecondary = list.ToArray();
            _secondaryCount = _arrConnNameOfSecondary.Length;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="isMaster"></param>
        /// <returns></returns>
        public ConnectionStringSettings Setting(bool isMaster = false)
        {
            var connName = string.Empty;
            if (isMaster)
                connName = Master;
            else
                connName = Secondary;

            var setting = ConfigurationManager.ConnectionStrings[connName];
            if (setting == null || string.IsNullOrEmpty(setting.ConnectionString))
                throw new ArgumentException($"{connName} not exist.");

            return setting;
        }
        #endregion
    }
}
