using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sodao.Dapper
{
    public class DataContext : IDisposable
    {
        #region Private Members
        private string _paramPrefix = "@";
        private string _providerName = "System.Data.SqlClient";
        #endregion

        #region Public Members
        private IDbConnection dbConnecttion;
        private DbProviderFactory dbFactory;
        private DBType _dbType = DBType.SqlServer;
        public IDbConnection DbConnecttion
        {
            get
            {
                return dbConnecttion;
            }
        }
        public string ParamPrefix
        {
            get
            {
                return _paramPrefix;
            }
        }
        public string ProviderName
        {
            get
            {
                return _providerName;
            }
        }
        public DBType DbType
        {
            get
            {
                return _dbType;
            }
        }
        #endregion

        public DataContext(bool isMaster = false)
        {
            OpenConnection(isMaster);
            SetParamPrefix();
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="isMaster"></param>
        private void OpenConnection(bool isMaster = false)
        {
            var _connSetting = DataContextConfig.Default.Setting(isMaster);
            var _connString = _connSetting.ConnectionString;
            _providerName = _connSetting.ProviderName;

            if (string.IsNullOrEmpty(_providerName))
                throw new Exception("ConnectionStrings中没有配置提供程序ProviderName！");

            dbFactory = DbProviderFactories.GetFactory(_providerName);
            dbConnecttion = dbFactory.CreateConnection();
            dbConnecttion.ConnectionString = _connString;
            dbConnecttion.Open();
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        private void SetParamPrefix()
        {
            string dbtype = (dbFactory == null ? dbConnecttion.GetType() : dbFactory.GetType()).Name;

            // 使用类型名判断
            if (dbtype.StartsWith("MySql")) _dbType = DBType.MySql;
            else if (dbtype.StartsWith("SqlCe")) _dbType = DBType.SqlServerCE;
            else if (dbtype.StartsWith("Npgsql")) _dbType = DBType.PostgreSQL;
            else if (dbtype.StartsWith("Oracle")) _dbType = DBType.Oracle;
            else if (dbtype.StartsWith("SQLite")) _dbType = DBType.SQLite;
            else if (dbtype.StartsWith("System.Data.SqlClient.")) _dbType = DBType.SqlServer;
            // else try with provider name
            else if (_providerName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.MySql;
            else if (_providerName.IndexOf("SqlServerCe", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SqlServerCE;
            else if (_providerName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.PostgreSQL;
            else if (_providerName.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.Oracle;
            else if (_providerName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0) _dbType = DBType.SQLite;

            if (_dbType == DBType.MySql && dbConnecttion != null && dbConnecttion.ConnectionString != null && dbConnecttion.ConnectionString.IndexOf("Allow User Variables=true") >= 0)
                _paramPrefix = "?";
            if (_dbType == DBType.Oracle)
                _paramPrefix = ":";
        }

        /// <summary>
        /// 垃圾回收
        /// </summary>
        public void Dispose()
        {
            if (dbConnecttion == null)
                return;
            try
            {
                dbConnecttion.Dispose();
            }
            catch { }
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        SqlServer,
        SqlServerCE,
        MySql,
        PostgreSQL,
        Oracle,
        SQLite
    }
}
