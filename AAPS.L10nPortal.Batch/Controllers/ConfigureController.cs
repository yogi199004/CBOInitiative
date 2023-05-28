using AAPS.L10nPortal.Batch.Model;
using AAPS.L10NPortal.Common;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AAPS.L10nPortal.Batch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigureController : ControllerBase
    {
        private readonly LogApi Logapi;

        public ConfigureController(IOptions<AppSettings> appSettings) 
        {            
            Logapi = new LogApi(appSettings?.Value);
        }

        [HttpGet]
        [Route("[action]")]
        public string CheckBatch()
        {
            try
            {
                Logapi.WriteToLog("Batch is Live! ", LogLevelEnum.Information);
                return "Batch is Live! " + DateTime.Now.ToString("MM/dd/yyyy");
            }
            catch (Exception ex)
            {
                Logapi.WriteToLog(ex.GetBaseException().Message, LogLevelEnum.Error);
                return null;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SetUp()
        {
            var EasternTimeZone = "Eastern Standard Time";
            var cronmapping = new CronMapping
            {
                CronTime = "*/15 * * * *",
                BatchDescription = "Test Batch",
                CronDescription = "Every 15 minutes",
                IsActive = true
            };

            TestBatch(EasternTimeZone, cronmapping);
            return Ok();
        }

        private void TestBatch(string EasternTimeZone, CronMapping cron)
        {
            RecurringJob.AddOrUpdate(cron.BatchDescription,
                () => TestBatchImpl(null),
                cron.IsActive ? cron.CronTime : Cron.Never(),
                TimeZoneInfo.FindSystemTimeZoneById(EasternTimeZone));
        }
        public void TestBatchImpl(PerformContext? performContext)
        {
            int number1 = 3000;
            int number2 = 0;
           
              var abc=number1 / number2;
            
            Task.Delay(120).Wait();
        }
    }
}
