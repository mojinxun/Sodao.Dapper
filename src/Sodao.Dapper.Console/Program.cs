using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Dapper.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine($"start....");

            for (int i = 0; i < 1; i++)
            {
                InsertBatchTest();

                System.Console.WriteLine($"over {i}");
                System.Console.WriteLine("-----------------------------");
            }

            System.Console.WriteLine($"finish....");
            System.Console.ReadKey();
        }


        static public void InsertBatchTest()
        {
            var list = new List<Users>();
            var count = 200;
            for (int i = 0; i < count; i++)
            {
                var name = $"test_{i}";
                var user = new Users()
                {
                    UserName = name,
                    UserPwd = "123456",
                    UserEmail = $"{name}@sodao.com",
                    IsValid = 1,
                    AddTime = DateTime.Now,
                };
                list.Add(user);
            }

            //var sb = new StringBuilder();
            //foreach (var item in list)
            //{
            //    sb.Append($"insert into Users({nameof(Users.UserName)},{nameof(Users.UserPwd)},{nameof(Users.UserEmail)},{nameof(Users.IsValid)},{nameof(Users.AddTime)})values('{item.UserName}','{item.UserPwd}','{item.UserEmail}','{item.IsValid}','{item.AddTime}');\r\n");
            //}
            //var s = sb.ToString();


            var sw = new Stopwatch();
            using (var ctx = new DataContext(true))
            {

                sw.Restart();
                foreach (var item in list)
                {
                    var result1 = ctx.Insert(item);
                }
                sw.Stop();
                TextHelper.Write($"{nameof(InsertBatchTest)}_{count} foreach总耗时:{sw.ElapsedMilliseconds}");



                sw.Restart();
                var result2 = ctx.InsertBatch(list);
                sw.Stop();
                TextHelper.Write($"{nameof(InsertBatchTest)}_{count} batch总耗时:{sw.ElapsedMilliseconds}");
                TextHelper.Write("  ");
            }
        }
    }
}
