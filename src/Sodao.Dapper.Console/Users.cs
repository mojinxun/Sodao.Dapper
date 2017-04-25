using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Dapper.Console
{
    [Table(Name = "Users")]
    public class Users
    {
        /// <summary>
        /// UserId
        /// </summary>
        [PrimaryKey(Name = "UserId")]
        [Column(AutoIncrement = true)]
        public int UserId { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// UserPwd
        /// </summary>
        public string UserPwd { get; set; }
        /// <summary>
        /// UserEmail
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// IsValid
        /// </summary>
        public int IsValid { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
