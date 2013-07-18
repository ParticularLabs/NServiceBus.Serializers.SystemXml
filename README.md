# Introduction

Implemements a [System.Xml.Serialization](http://msdn.microsoft.com/en-us/library/system.xml.serialization.aspx) based [NServiceBus](http://www.nservicebus.org) message serializer to allow better interoperability with non-NServiceBus peers.

# Usage

In the `Init` method of an `IWantToRunBeforeConfiguration`, call `Configure.Serialization.SystemXml();`.