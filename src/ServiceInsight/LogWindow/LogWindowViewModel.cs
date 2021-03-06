﻿namespace ServiceInsight.LogWindow
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Windows.Input;
    using System.Windows.Media;
    using Caliburn.Micro;
    using Framework;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Formatting.Display;

    public class LogWindowViewModel : Screen
    {
        public static Subject<LogEvent> LogObserver = new Subject<LogEvent>();

        readonly IClipboard clipboard;
        ITextFormatter textFormatter;
        const int MaxTextLength = 5000;

        public LogWindowViewModel(IClipboard clipboard)
        {
            this.clipboard = clipboard;

            Logs = new ObservableCollection<LogMessage>();

            textFormatter = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", CultureInfo.InvariantCulture);
            LogObserver
                .ObserveOnDispatcher()
                .Subscribe(UpdateLog);

            ClearCommand = Command.Create(Clear);
            CopyCommand = Command.Create(Copy);
        }

        public ObservableCollection<LogMessage> Logs { get; set; }

        public ICommand ClearCommand { get; }

        public ICommand CopyCommand { get; }

        void Clear()
        {
            Logs.Clear();
        }

        void Copy()
        {
            clipboard.CopyTo(Logs.Aggregate("", (s, l) => s + l.Log));
        }

        void UpdateLog(LogEvent loggingEvent)
        {
            if (Logs.Count > MaxTextLength)
            {
                Clear();
            }

            var sr = new StringWriter();
            textFormatter.Format(loggingEvent, sr);
            var log = sr.ToString();

            switch (loggingEvent.Level)
            {
                case LogEventLevel.Information:
                    Logs.Add(new LogMessage(log, Colors.Black, true));
                    break;

                case LogEventLevel.Warning:
                    Logs.Add(new LogMessage(log, Colors.DarkOrange));
                    break;

                case LogEventLevel.Error:
                    Logs.Add(new LogMessage(log, Colors.Red));
                    break;

                case LogEventLevel.Fatal:
                    Logs.Add(new LogMessage(log, Colors.DarkOrange));
                    break;

                case LogEventLevel.Debug:
                    Logs.Add(new LogMessage(log, Colors.Green));
                    break;

                default:
                    Logs.Add(new LogMessage(log, Colors.Black));
                    break;
            }
        }
    }
}