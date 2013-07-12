using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBus.Serializers.SystemXml
{
    using Settings;

    public class SystemXmlSerializationSettings
    {
        public SystemXmlSerializationSettings SkipWrappingSingleMessage()
        {
            SettingsHolder.SetProperty<SystemXmlMessageSerializer>(s => s.SkipWrappingElementForSingleMessages, true);
            return this;
        }
        public SystemXmlSerializationSettings WrapSingleMessage()
        {
            SettingsHolder.SetProperty<SystemXmlMessageSerializer>(s => s.SkipWrappingElementForSingleMessages, false);
            return this;
        }

    }
}
