namespace NServiceBus
{
    using System;
    using Features;
    using Serializers.SystemXml;
    using Serializers.XML.Config;
    using Settings;

    public static class SystemXmlSerializerConfigurationExtensions
    {
        public static SerializationSettings SystemXml(this SerializationSettings settings, Action<XmlSerializationSettings> customSettings = null)
        {
            Feature.Enable<SystemXmlSerialization>();
            return settings;
        }
    }
}
