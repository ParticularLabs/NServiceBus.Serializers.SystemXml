namespace NServiceBus.Serializers.SystemXml.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Xunit;

    public class SystemXmlMessageSerializerTests
    {
        [Fact]
        public void ItShouldSerializeSingleObject()
        {
            var ser = new SystemXmlMessageSerializer();
            using (var stream = new MemoryStream())
            {
                ser.Serialize(new object[] { DateTime.MinValue }, stream);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(stream))
                {
                    var s = streamReader.ReadToEnd();
                    Console.WriteLine(s);
                    Assert.True(s.Contains(@"<dateTime>0001-01-01T00:00:00</dateTime>"));
                }
                
            }
        }

        [Fact]
        public void ItShouldRaiseExceptionOnSerializeMultipleObjects()
        {
            var ser = new SystemXmlMessageSerializer();
            using (var stream = new MemoryStream())
            {
                Assert.Throws<ArgumentException>(() =>ser.Serialize(new object[] { DateTime.MinValue, DateTime.MaxValue }, stream));
            }
        }

        [Fact]
        public void ItShouldHaveAProperContentType()
        {
            Assert.Equal("text/xml", new SystemXmlMessageSerializer().ContentType);
        }
    }
}
