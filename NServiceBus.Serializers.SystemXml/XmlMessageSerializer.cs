namespace NServiceBus.Serializers.SystemXml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Serialization;

    public class SystemXmlMessageSerializer : IMessageSerializer
    {
        public void Serialize(object[] messages, Stream stream)
        {
            if (messages.Length > 1)
            {
                throw new ArgumentException("Cannot serialize multiple objects", "messages");
            }
            foreach (var message in messages)
            {
                var serializer = new XmlSerializer(message.GetType());
                serializer.Serialize(stream, message);
            }
        }

        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            if (messageTypes == null || messageTypes.Count < 1)
            {
                throw new ArgumentException("Need one or more types to be specified", "messageTypes");
            }
            var mainType = messageTypes.First();
            var otherTypes = messageTypes.Skip(1);
            var serializer = new XmlSerializer(mainType, otherTypes.ToArray());
            return new[] {serializer.Deserialize(stream)};
        }

        public string ContentType { get { return ContentTypes.Xml; } }
    }
}
