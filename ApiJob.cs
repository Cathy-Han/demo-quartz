using System;
using System.Net;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace demo_quartz
{
    public class ApiJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var method = dataMap.GetString("Method");
            var url = dataMap.GetString("Url");
            var body = dataMap.GetString("BodyContent");
            Console.WriteLine($"{context.JobDetail.Key} executing……");

            string result=null;
            using(WebClient api = new WebClient())
            {
                switch(method)
                {
                    case "GET":
                        result = api.DownloadString(url);
                        break;
                    case "POST":
                    case "PUT":
                    case "DELETE":
                        result = api.UploadString(url,method,body);
                        break;
                }
            }
            Console.WriteLine($"{context.JobDetail.Key} result:{result}.");
            Console.WriteLine($"{context.JobDetail.Key} executed.");

            return Task.CompletedTask;
        }
    }
}