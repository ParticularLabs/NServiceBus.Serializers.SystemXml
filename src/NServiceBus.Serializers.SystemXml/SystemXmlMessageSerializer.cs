namespace NServiceBus.Serializers.SystemXml
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Serialization;
    using Logging;

    class SystemXmlMessageSerializer : IMessageSerializer
    {
        private readonly string _envelopeNamespace = String.Empty;
        private static readonly Encoding Encoding = Encoding.UTF8;
        public const string EnvelopeName = "Messages";
        private const int TypesToCacheBeforeWarning = 500;

        public void Serialize(object message, Stream stream)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings{Encoding = Encoding}))
            {
                var serializer = new XmlSerializer(message.GetType());
                serializer.Serialize(writer, message);
            }
        }

        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            var xdoc = new XmlDocument();
            xdoc.Load(stream);

            var results = new List<object>();
            var documentElement = xdoc.DocumentElement;
            if (documentElement.LocalName == EnvelopeName && documentElement.NamespaceURI == _envelopeNamespace)
            {
                results.AddRange(documentElement.ChildNodes.Cast<object>().Select(element => GetObjectFromNode((XmlElement)element, messageTypes)));
            }
            else
            {
                results.Add(GetObjectFromNode(documentElement, messageTypes));
            }
            return results.ToArray();
        }

        public string ContentType { get { return ContentTypes.Xml; } }

        private static Type GetTypeFromElementName(XmlNode element)
        {
            var xmlNs = element.NamespaceURI;
            var defaultNameSpace = xmlNs.Substring(xmlNs.LastIndexOf("/") + 1);
            var className = element.LocalName;
            var fullTypeName = defaultNameSpace + "." + className;
            var returnValue = typesCache.GetOrAdd(fullTypeName, key => Type.GetType(key) ?? AppDomain.CurrentDomain.GetAssemblies()
                                                                                     .Select(a => a.GetType(key))
                                                                                     .FirstOrDefault(t => t != null));
            if (typesCache.Count > TypesToCacheBeforeWarning)
            {
                logger.WarnFormat("Number of cached types exceeded {0}. There's likely something wrong", TypesToCacheBeforeWarning);
            }
            return returnValue;
        }


        private static object GetObjectFromNode(XmlNode element, IEnumerable<Type> messageTypes)
        {
            var types = GetTypesForElement(element, messageTypes);

            using (var tmpStream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(tmpStream, new XmlWriterSettings{Encoding = Encoding}))
                {
                    element.WriteTo(writer);
                }
                tmpStream.Seek(0, SeekOrigin.Begin);
                var serializer = new XmlSerializer(types.First());
                return serializer.Deserialize(tmpStream);
            }
        }

        private static IEnumerable<Type> GetTypesForElement(XmlNode element, IEnumerable<Type> messageTypes)
        {
            var types = messageTypes;
            if (messageTypes == null || !messageTypes.Any())
            {
                var mainType = GetTypeFromElementName(element);
                if (mainType == null)
                {
                    throw new ArgumentException("Need one or more types to be specified", "messageTypes");
                }
                types = new[]{mainType};
            }
            return types;
        }

         static readonly ILog logger = LogManager.GetLogger(typeof(SystemXmlMessageSerializer));
        static readonly ConcurrentDictionary<string,Type> typesCache = new ConcurrentDictionary<string, Type>();
    }
}
