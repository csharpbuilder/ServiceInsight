﻿namespace Particular.ServiceInsight.Desktop.SequenceDiagram
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Caliburn.Micro;
    using Common;
    using Events;
    using Framework;
    using Models;
    using ReactiveUI;
    using Search;
    using ServiceControl;

    public class SequenceDiagramViewModel : Screen, IHandle<SelectedMessageChanged>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IServiceControl serviceControl;

        public SequenceDiagramViewModel(IClipboard clipboard, IEventAggregator eventAggregator, IServiceControl serviceControl, SearchBarViewModel searchBar)
        {
            this.eventAggregator = eventAggregator;
            this.serviceControl = serviceControl;

            CopyConversationIDCommand = new CopyConversationIDCommand(clipboard);
            CopyMessageURICommand = new CopyMessageURICommand(clipboard, serviceControl);
            RetryMessageCommand = new RetryMessageCommand(eventAggregator, serviceControl);
            SearchByMessageIDCommand = new SearchByMessageIDCommand(eventAggregator, searchBar);
        }

        public ReactiveList<EndpointInfo> Endpoints { get; set; }

        public IEnumerable<MessageInfo> Messages { get; set; }

        public ICommand RetryMessageCommand { get; private set; }
        public ICommand CopyConversationIDCommand { get; private set; }
        public ICommand CopyMessageURICommand { get; private set; }
        public ICommand SearchByMessageIDCommand { get; private set; }

        public void Handle(SelectedMessageChanged message)
        {
            var storedMessage = message.Message;
            if (storedMessage == null)
                return;

            var conversationId = storedMessage.ConversationId;
            if (conversationId == null)
                return;

            var messages = serviceControl.GetConversationById(conversationId).ToList();

            CreateEndpoints(messages);

            CreateMessages(messages);
        }

        private void CreateEndpoints(IEnumerable<StoredMessage> messages)
        {
            Endpoints = new ReactiveList<EndpointInfo>(messages
                .OrderBy(m => m.TimeSent)
                .SelectMany(m => new[] { new EndpointInfo(m.SendingEndpoint, m), new EndpointInfo(m.ReceivingEndpoint, m) })
                .Distinct());
        }

        private void CreateMessages(IEnumerable<StoredMessage> messages)
        {
            Messages = messages.OrderBy(m => m.TimeSent).Select(m => new MessageInfo(eventAggregator, this, m, Endpoints)).ToList();

            Messages.First().IsFirst = true;

            foreach (var message in Messages)
            {
                eventAggregator.Subscribe(message);
            }
        }
    }
}