# Introduction

Implemements a [System.Xml.Serialization](http://msdn.microsoft.com/en-us/library/system.xml.serialization.aspx) based [NServiceBus](http://www.nservicebus.org) message serializer to allow better interoperability with non-NServiceBus peers.

# Usage

In the `Init` method of an `IWantToRunBeforeConfiguration`, call `Configure.Serialization.SystemXml(s=>s.SkipWrappingSingleMessage());`.

# Notes

The [System.Xml.Serialization](http://msdn.microsoft.com/en-us/library/system.xml.serialization.aspx)  is currently known to trigger undesirable behavior in the
64 bit .NET JIT optimizer ([Microsoft Connect issue about the issue](https://connect.microsoft.com/VisualStudio/feedback/details/508748/memory-consumption-alot-higher-on-x64-for-xslcompiledtransform-transform-then-on-x86)).

The memory consuption issue seems to be resolved with the next major release of the .NET JIT [RyuJIT](https://connect.microsoft.com/VisualStudio/feedback/details/508748/memory-consumption-alot-higher-on-x64-for-xslcompiledtransform-transform-then-on-x86).
It can also be avoided by using the current 32-Bit VM (e.g. by using the 32-bit NServiceBus host executable).

## Implementing 64-Bit JIT workaround

If there is memory trouble with serializing messages using this serializer and the 64 bit VM has to be used, do the following.

1. Create [sgen](http://msdn.microsoft.com/en-us/library/bk3w6240%28v=vs.110%29.aspx)ed assembly for the contracts assembly by adding the following
to the msbuild for the contract. In order for sgen to work properly, it's adviced that an assembly be created per contract.

```XML
<Target Name="AfterBuild" DependsOnTargets="AssignTargetPaths;Compile;ResolveKeySource" Inputs="$(MSBuildAllProjects);@(IntermediateAssembly)" Outputs="$(OutputPath)$(_SGenDllName)">
    <Delete Files="$(TargetDir)$(TargetName).XmlSerializers.dll" ContinueOnError="true" />
    <SGen BuildAssemblyName="$(TargetFileName)" BuildAssemblyPath="$(OutputPath)" References="@(ReferencePath)" ShouldGenerateSerializer="true" UseProxyTypes="false" KeyContainer="$(KeyContainerName)" KeyFile="$(KeyOriginatorFile)" DelaySign="$(DelaySign)" ToolPath="$(TargetFrameworkSDKToolsDirectory)" Platform="$(Platform)">
      <Output TaskParameter="SerializationAssembly" ItemName="SerializationAssembly" />
    </SGen>
</Target>
```

2. Disable the JIT optimizer on the generated assembly by creating a [.NET debugging control config file](http://msdn.microsoft.com/en-us/library/9dd8z24x%28v=vs.110%29.aspx)
for the XMLSerializer assembly created above. For example
```INI
[.NET Framework Debugging Control]
GenerateTrackingInfo=1
AllowOptimize=0
```