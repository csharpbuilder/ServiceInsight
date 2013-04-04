﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Caliburn.PresentationFramework.ApplicationModel;
using NServiceBus.Profiler.Common.Models;
using NServiceBus.Profiler.Core;
using NServiceBus.Profiler.Core.MessageDecoders;

namespace NServiceBus.Profiler.Desktop.MessageHeaders
{
    [TypeConverter(typeof(HeaderInfoTypeConverter))]
    public class PerformanceHeaderViewModel : HeaderInfoViewModelBase, IPerformanceHeaderViewModel
    {
        public PerformanceHeaderViewModel(
            IEventAggregator eventAggregator, 
            IContentDecoder<IList<HeaderInfo>> decoder, 
            IQueueManagerAsync queueManager) 
            : base(eventAggregator, decoder, queueManager)
        {
            DisplayName = "Performance";
        }

        [Description("Time the message was sent")]
        public string TimeSent { get; set; }

        [Description("Time the processing started")]
        public string ProcessingStarted { get; set; }

        [Description("Time the processing finished")]
        public string ProcessingEnded { get; set; }

        protected override void MapHeaderKeys()
        {
            ConditionsMap.Add(h => h.Key.EndsWith("TimeSent", StringComparison.OrdinalIgnoreCase), h => TimeSent = h.Value);
            ConditionsMap.Add(h => h.Key.EndsWith("ProcessingStarted", StringComparison.OrdinalIgnoreCase), h => ProcessingStarted = h.Value);
            ConditionsMap.Add(h => h.Key.EndsWith("ProcessingEnded", StringComparison.OrdinalIgnoreCase), h => ProcessingEnded = h.Value);
        }

        protected override void ClearHeaderValues()
        {
            TimeSent = null;
            ProcessingStarted = null;
            ProcessingEnded = null;
        }
    }
}