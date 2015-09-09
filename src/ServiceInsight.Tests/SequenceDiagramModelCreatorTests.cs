﻿namespace Particular.ServiceInsight.Tests
{
    using System;
    using System.Collections.Generic;
    using global::ServiceInsight.SequenceDiagram;
    using NUnit.Framework;
    using Particular.ServiceInsight.Desktop.Models;
    using Particular.ServiceInsight.Desktop.SequenceDiagram;

    [TestFixture]
    class SequenceDiagramModelCreatorTests
    {
        [Test]
        public void NoMessages()
        {
            var messages = new List<ReceivedMessage>();
            var creator = new ModelCreator(messages);
            var result = creator.GetModel();

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void SequentialFlowWithDistinctEndpoints()
        {
            var messages = new List<ReceivedMessage>
            {
                new ReceivedMessage
                {
                    message_id = "3",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "D"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "2"
                        }
                    }
                },
                new ReceivedMessage
                {
                    message_id = "1",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>()
                },
                new ReceivedMessage
                {
                    message_id = "2",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    }
                }
            };

            var creator = new ModelCreator(messages);
            var result = creator.GetModel();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("A", result[0].Title);
            Assert.AreEqual("B", result[1].Title);
            Assert.AreEqual("C", result[2].Title);
            Assert.AreEqual("D", result[3].Title);
        }

        [Test]
        public void SequentialFlowWithMissingRelatedTo()
        {
            var messages = new List<ReceivedMessage>
            {
                new ReceivedMessage
                {
                    message_id = "2",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    }
                }
            };

            var creator = new ModelCreator(messages);
            var result = creator.GetModel();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("A", result[0].Title);
            Assert.AreEqual("B", result[1].Title);
        }

        [Test]
        public void SequentialFlowWithSharedSourceEndpoints()
        {
            var messages = new List<ReceivedMessage>
            {
                new ReceivedMessage
                {
                    message_id = "2",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    }
                },
                new ReceivedMessage
                {
                    message_id = "3",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "D"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    }
                },
                new ReceivedMessage
                {
                    message_id = "1",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>()
                }
            };

            var creator = new ModelCreator(messages);
            var result = creator.GetModel();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("A", result[0].Title);
            Assert.AreEqual("B", result[1].Title);
            Assert.AreEqual("C", result[2].Title);
            Assert.AreEqual("D", result[3].Title);
        }

        [Test]
        public void SequentialFlowWithSharedDestinationEndpoints()
        {
            var messages = new List<ReceivedMessage>
            {
                new ReceivedMessage
                {
                    message_id = "2",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    }
                },
                new ReceivedMessage
                {
                    message_id = "3",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    }
                },
                new ReceivedMessage
                {
                    message_id = "1",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>()
                }
            };

            var creator = new ModelCreator(messages);
            var result = creator.GetModel();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("A", result[0].Title);
            Assert.AreEqual("B", result[1].Title);
            Assert.AreEqual("C", result[2].Title);
        }

        [Test]
        public void SequentialOrderOfHandlers()
        {
            var currentDateTime = DateTime.UtcNow;

            var messages = new List<ReceivedMessage>
            {
                new ReceivedMessage
                {
                    message_id = "3",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "D"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "2"
                        }
                    },
                    message_type = "Message3",
                    processed_at = currentDateTime.AddMinutes(2),
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                },
                new ReceivedMessage
                {
                    message_id = "1",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>(),
                    message_type = "Message1",
                    processed_at = currentDateTime,
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                },
                new ReceivedMessage
                {
                    message_id = "2",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    },
                    message_type = "Message2",
                    processed_at = currentDateTime.AddMinutes(1),
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                },
                new ReceivedMessage
                {
                    message_id = "4",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    },
                    message_type = "Message4",
                    processed_at = currentDateTime.AddMinutes(1),
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                },
                new ReceivedMessage
                {
                    message_id = "5",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "C"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "4"
                        }
                    },
                    message_type = "Message5",
                    processed_at = currentDateTime.AddMinutes(2),
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                }
            };

            var creator = new ModelCreator(messages);
            var result = creator.GetModel();

            Assert.AreEqual(0, result[0].Handlers.Count);
            Assert.AreEqual(2, result[1].Handlers.Count);
            Assert.AreEqual(2, result[2].Handlers.Count);
            Assert.AreEqual(1, result[3].Handlers.Count);

            Assert.AreEqual("Message1", result[1].Handlers[0].Title);
            Assert.AreEqual("Message5", result[1].Handlers[1].Title);

        }

        [Test]
        public void HandlerMessageInAndOut()
        {
            var currentDateTime = DateTime.UtcNow;

            var messages = new List<ReceivedMessage>
            {
                new ReceivedMessage
                {
                    message_id = "2",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    },
                    message_type = "Message2",
                    processed_at = currentDateTime,
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                },
                new ReceivedMessage
                {
                    message_id = "3",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    },
                    message_type = "Message3",
                    processed_at = currentDateTime,
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                },
                new ReceivedMessage
                {
                    message_id = "4",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    },
                    message_type = "Message4",
                    processed_at = currentDateTime,
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                },
                new ReceivedMessage
                {
                    message_id = "5",
                    sending_endpoint = new EndpointAddress
                    {
                        name = "A"
                    },
                    receiving_endpoint = new EndpointAddress
                    {
                        name = "B"
                    },
                    headers = new List<Header>
                    {
                        new Header
                        {
                            key = MessageHeaderKeys.RelatedTo,
                            value = "1"
                        }
                    },
                    message_type = "Message5",
                    processed_at = currentDateTime,
                    message_intent = MessageIntent.Send,
                    status = MessageStatus.Successful,
                }
            };

            var creator = new ModelCreator(messages);
            var result = creator.GetModel();

            Assert.AreEqual(1, result[0].Handlers.Count);
            Assert.AreEqual(4, result[1].Handlers.Count);
            Assert.AreEqual(4, result[0].Handlers[0].Out.Count);

        }
    }
}
