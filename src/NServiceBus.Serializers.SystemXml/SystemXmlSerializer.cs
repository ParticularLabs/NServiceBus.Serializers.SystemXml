namespace NServiceBus
{
    using System;
    using MessageInterfaces;
    using Serialization;
    using Serializers.SystemXml;
    using Settings;


    /// <summary>
    /// Defines the capabilities of the XML serializer.
    /// </summary>
    public class SystemXmlSerializer : SerializationDefinition
    {
        /// <summary>
        /// Provides a factory method for building a message serializer.
        /// </summary>
        public override Func<IMessageMapper, IMessageSerializer> Configure(ReadOnlySettings settings)
        {
            return mapper => new SystemXmlMessageSerializer();
        }
    }
}
