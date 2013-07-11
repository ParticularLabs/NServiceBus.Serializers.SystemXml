namespace NServiceBus.Serializers.SystemXml
{
    using Features;
    using MessageInterfaces.MessageMapper.Reflection;
    using Settings;

    public class SystemXmlSerialization : Feature<Features.Categories.Serializers>
    {
        public override void Initialize()
        {
            Configure.Component<MessageMapper>(DependencyLifecycle.SingleInstance);
            Configure.Component<SystemXmlMessageSerializer>(DependencyLifecycle.SingleInstance);
            SettingsHolder.ApplyTo<SystemXmlMessageSerializer>();
        }
    }
}