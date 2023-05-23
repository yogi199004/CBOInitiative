using AAPS.L10nPortal.Batch.Model;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.L10nPortal.Batch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigureController : ControllerBase
    {

        [HttpGet]
        [Route("[action]")]
        public string CheckBatch()
        {
            return "Batch is Live! " + DateTime.Now.ToString("MM/dd/yyyy");
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
            Task.Delay(120).Wait();
        }
    }
}
