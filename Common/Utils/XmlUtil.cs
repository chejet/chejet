using System ;
using System.Globalization ;
using System.Xml ;
using System.IO;

namespace XkCms.Common.Utils
{
	public sealed class XmlUtil
	{
		public XmlUtil()
		{}

		public static XmlNode AppendElement( XmlNode node, string newElementName )
		{
			return AppendElement( node, newElementName, null ) ;
		}

		public static XmlNode AppendElement( XmlNode node, string newElementName, string innerValue )
		{
			XmlNode oNode ;

			if ( node is XmlDocument )
                oNode = node.AppendChild( ((XmlDocument)node).CreateElement( newElementName ) ) ;
			else
				oNode = node.AppendChild( node.OwnerDocument.CreateElement( newElementName ) ) ;

			if ( innerValue != null )
				oNode.AppendChild( node.OwnerDocument.CreateTextNode( innerValue ) ) ;

			return oNode ;
		}

		public static XmlAttribute CreateAttribute( XmlDocument xmlDocument, string name, string value )
		{
			XmlAttribute oAtt = xmlDocument.CreateAttribute( name ) ;
			oAtt.Value = value ;
			return oAtt ;
		}

		public static void SetAttribute( XmlNode node, string attributeName, string attributeValue )
		{
			if ( node.Attributes[ attributeName ] != null )
				node.Attributes[ attributeName ].Value = attributeValue ;
			else
				node.Attributes.Append( CreateAttribute( node.OwnerDocument, attributeName, attributeValue ) ) ;
		}

        public static string FormatXml(XmlDocument xml)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XmlTextWriter xw = new XmlTextWriter(new StringWriter(sb));
            xw.Formatting = Formatting.Indented;
            xw.Indentation = 1;
            xw.IndentChar = '\t';
            xml.WriteTo(xw);
            return sb.ToString();
        }
	}
}
