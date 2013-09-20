namespace NServiceBus
{
    using System;
    using Features;
    using Serializers.SystemXml;
    using Settings;

    public static class SystemXmlSerializerConfigurationExtensions
    {
        public static SerializationSettings SystemXml(this SerializationSettings settings, Action<SystemXmlSerializationSettings> customSettings = null)
        {
            Feature.Enable<SystemXmlSerialization>();

            if (customSettings != null)
                customSettings(new SystemXmlSerializationSettings());

            return settings;
        }
    }
}
