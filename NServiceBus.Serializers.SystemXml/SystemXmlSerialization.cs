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
            Configure.Component<SystemXmlMessageSerializer>(DependencyLifecycle.SingleInstance)
                                .ConfigureProperty(p => p.SkipWrappingElementForSingleMessages, !SettingsHolder.GetOrDefault<bool>("SerializationSettings.WrapSingleMessages"));
            SettingsHolder.ApplyTo<SystemXmlMessageSerializer>();
        }
    }
}