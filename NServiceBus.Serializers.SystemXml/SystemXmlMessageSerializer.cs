namespace NServiceBus.Serializers.SystemXml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Serialization;

    public class SystemXmlMessageSerializer : IMessageSerializer
    {
        public bool SkipWrappingElementForSingleMessages { get; set; }
        private readonly string _envelopeNamespace = String.Empty;
        private static readonly Encoding Encoding = Encoding.UTF8;
        public const string EnvelopeName = "Messages";

        public void Serialize(object[] messages, Stream stream)
        {
            if (!messages.Any() && SkipWrappingElementForSingleMessages)
            {
                throw new ArgumentException("Cannot serialize empty object collection without envelope", "messages");
            }

            var writeEnvelope = !SkipWrappingElementForSingleMessages || messages.Length > 1;
            
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings{Encoding = Encoding}))
            {
                if (writeEnvelope)
                {
                    writer.WriteStartElement(EnvelopeName, _envelopeNamespace);
                    foreach (var message in messages)
                    {
                        WriteMessage(message, writer);
                    }
                    writer.WriteEndElement();
                }
                else
                {
                    WriteMessage(messages.First(), writer);
                }
            }
        }

        private static void WriteMessage(object message, XmlWriter writer)
        {
            var serializer = new XmlSerializer(message.GetType());
            serializer.Serialize(writer, message);
        }

        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            if (messageTypes == null || messageTypes.Count < 1)
            {
                throw new ArgumentException("Need one or more types to be specified", "messageTypes");
            }
            var mainType = messageTypes.First();
            var otherTypes = messageTypes.Skip(1).ToArray();
            var xdoc = new XmlDocument();
            xdoc.Load(stream);
            var results = new List<object>();
            var documentElement = xdoc.DocumentElement;
            if (documentElement.Name == EnvelopeName && documentElement.NamespaceURI == _envelopeNamespace)
            {
                foreach (var element in documentElement.ChildNodes)
                {
                    AddDocumentFromElement((XmlElement)element, mainType, otherTypes, results);
                }
            }
            else
            {
                var element = documentElement;
                AddDocumentFromElement(element, mainType, otherTypes, results);
            }
            return results.ToArray();
        }

        private static void AddDocumentFromElement(XmlElement element, Type mainType, IEnumerable<Type> otherTypes, List<object> results)
        {
            using (var tmpStream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(tmpStream, new XmlWriterSettings{Encoding = Encoding}))
                {
                    element.WriteTo(writer);
                }
                tmpStream.Seek(0, SeekOrigin.Begin);
                var serializer = new XmlSerializer(mainType, otherTypes.ToArray());
                results.Add(serializer.Deserialize(tmpStream));
            }
        }

        public string ContentType { get { return ContentTypes.Xml; } }
    }
}
