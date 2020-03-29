using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;

namespace demo_quartz
{
    public class JobManager
    {
        IScheduler _schedule;
        public JobManager(IOptions<Dictionary<string,ApiConfig>> options)
        {
            var apis = options.Value;
            _schedule = JobManager.GetScheduler();
            foreach(var item in apis)
            {
                var detail = JobManager.CreateJobDetail(item.Key,item.Value);
                var trigger = JobManager.CreateCronTrigger(item.Key,item.Value.Cron);
                _schedule.ScheduleJob(detail,trigger);
            }
        }

        public void Start()
        {
            _schedule.Start();
        }

        public static IScheduler GetScheduler()
        {
            return StdSchedulerFactory.GetDefaultScheduler().Result;
        }

        public static IJobDetail CreateJobDetail(string name,ApiConfig api)
        {
            var dataMap = new JobDataMap();
            dataMap.Add("Method",api.Method);
            dataMap.Add("Url",api.Url);
            dataMap.Add("BodyContent",api.BodyContent);
            IJobDetail jobDetail = JobBuilder.Create<ApiJob>()
                                            .WithIdentity(name)
                                            .UsingJobData(dataMap)
                                            .Build();
            return jobDetail;
        }

        public static ITrigger CreateSimpleTrigger(string name,DateTimeOffset start,int intervalSeconds,int? repeatCounts)
        {
            ITrigger trigger= TriggerBuilder.Create()
                                            .StartAt(DateTimeOffset.Now)
                                            .WithSimpleSchedule(x=>{
                                                x.WithIntervalInSeconds(10);
                                                x.WithRepeatCount(10);
                                            })
                                            .WithIdentity(name)
                                            .Build();
            return trigger;
        }

        public static ITrigger CreateCronTrigger(string name,string cron)
        {
            ITrigger trigger= TriggerBuilder.Create()
                                            .StartAt(DateTimeOffset.Now)
                                            .WithCronSchedule(cron)
                                            .WithIdentity(name)
                                            .Build();
            return trigger;
        }

    }

    public class ApiConfig
    {
        public string Method{get;set;}

        public string Url{get;set;}

        public string BodyContent{get;set;}

        public string Cron{get;set;}
    }
}