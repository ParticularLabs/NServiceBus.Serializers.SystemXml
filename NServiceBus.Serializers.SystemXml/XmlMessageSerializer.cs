namespace NServiceBus.Serializers.SystemXml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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

        public object[] Deserialize(Stream stream, IList<Type> messageTypes = null)
        {
            throw new NotImplementedException();
        }

        public string ContentType { get { return ContentTypes.Xml; } }
    }
}
