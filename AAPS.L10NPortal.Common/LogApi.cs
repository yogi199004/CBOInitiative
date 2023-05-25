using Serilog;
using System.Diagnostics;

namespace AAPS.L10NPortal.Common
{
    public sealed class LogApi
    {
        private readonly int processId = Process.GetCurrentProcess().Id;
        private readonly string processName = Process.GetCurrentProcess().ProcessName;
        private readonly string machineName = Environment.MachineName;
        private string SLogSqlCon = string.Empty;
        private AppSettings appSettings;
        public long hangfireJobId { get; set; }

        public LogApi(AppSettings? _appSettings = null)
        {
            appSettings = _appSettings;

        }

        /// <summary>
        /// Writes to log Seq 
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="ApplicationType">Type of the application.</param>
        public void WriteToLog(string message, LogLevelEnum logType = LogLevelEnum.Information, ApplicationTypeEnum applicationType = ApplicationTypeEnum.UI)
        {
            using (var logSeq = new LoggerConfiguration()
                        .Enrich.WithProperty("Application", applicationType)
                        .Enrich.WithProperty("Environment", appSettings.Environment)
                        .Enrich.WithProperty("HangfireJobId", hangfireJobId)
                        .MinimumLevel.Verbose()
                        .WriteTo.Seq(appSettings.Sequrl,
                                period: TimeSpan.Zero,
                                batchPostingLimit: 5)
                            .CreateLogger())
            {

                switch (logType)
                {
                    case LogLevelEnum.Verbose:
                        logSeq.Verbose(message);
                        break;
                    case LogLevelEnum.Debug:
                        logSeq.Debug(message);
                        break;
                    case LogLevelEnum.Information:
                        logSeq.Information(message);
                        break;
                    case LogLevelEnum.Warning:
                        logSeq.Warning(message);
                        break;
                    case LogLevelEnum.Error:
                        logSeq.Error(message);
                        break;
                    case LogLevelEnum.Fatal:
                        logSeq.Fatal(message);
                        break;
                    default:
                        break;
                }
            }

        }

    }
}
