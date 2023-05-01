using BeautyControl.API.Extensions;
using FluentResults;
using Serilog.Core;
using Serilog.Events;

namespace BeautyControl.API.Infra.Logging
{
    // Não precisa dessa configuração customizada de logging porque meu LoggingPipelineBehavior já faz o log necessário. Mas foi legal para aprender
    public class CustomFluentValidationLogger : IResultLogger
    {
        private readonly Logger _logger;
        private readonly IReadOnlyDictionary<LogLevel, LogEventLevel> _logLevelEventsMap;

        public CustomFluentValidationLogger(Logger logger)
        {
            _logger = logger;
            _logLevelEventsMap = new Dictionary<LogLevel, LogEventLevel>
            {
                { LogLevel.Trace, LogEventLevel.Verbose },
                { LogLevel.Debug, LogEventLevel.Debug },
                { LogLevel.Information, LogEventLevel.Information },
                { LogLevel.Warning, LogEventLevel.Warning },
                { LogLevel.Error, LogEventLevel.Error },
                { LogLevel.Critical, LogEventLevel.Fatal }
            };
        }

        public void Log(string context, string content, ResultBase result, LogLevel logLevel)
        {
            (var message, var reasons, var sucesssToken) = BuildLogMessage(result);

            _logger.ForContext(context, value: null)
                .Write(_logLevelEventsMap[logLevel], message, reasons, sucesssToken, content);
        }

        public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
        {
            (var message, var reasons, var sucesssToken) = BuildLogMessage(result);

            _logger.ForContext(typeof(TContext))
                .Write(_logLevelEventsMap[logLevel], message, reasons, sucesssToken, content);
        }

        private static (string Message, IEnumerable<string>, string sucesssToken) BuildLogMessage(ResultBase result)
        {
            var message = string.Join(
                Environment.NewLine,
                "Mensagens: {@Mensagens}",
                "Sucesso: {IsSuccess}",
                "Payload: {@Content}"
            );

            var reasons = result.GetReasonsMessages();
            var sucesssToken = result.IsSuccess ? "Sim" : "Não";

            return (message, reasons, sucesssToken);
        }
    }
}
