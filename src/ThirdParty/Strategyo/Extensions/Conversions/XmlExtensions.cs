using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Strategyo.Extensions.Conversions;

public static class XmlExtensions
{
    public static T? DeserializeXmlFrom<T>(this Stream entry)
    {
        var serializer = new XmlSerializer(typeof(T));
        
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore,
            XmlResolver = null
        };
        
        using var reader = XmlReader.Create(entry, settings);
        var data = (T?)serializer.Deserialize(reader);

        return data;
    }

    #region Parse

    public static async Task<Dictionary<string, object>> ParseXmlToDictionaryAsync(this string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new ArgumentException("XML string cannot be null or whitespace.", nameof(xml));
        }
        
        await using Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        var dict = await ParseXmlToDictionary(xmlStream).ConfigureAwait(false);
        return dict;
    }
    
    public static async Task<Dictionary<string, object>> ParseXmlFileToDictionaryAsync(this string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));
        }
        
        await using Stream xmlStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return await ParseXmlToDictionary(xmlStream).ConfigureAwait(false);
    }
    
    public static async Task<Dictionary<string, object>> ParseXmlStreamToDictionaryAsync(this Stream xmlStream)
    {
        if (xmlStream == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(xmlStream));
        }
        
        return await ParseXmlToDictionary(xmlStream).ConfigureAwait(false);
    }

    #endregion
    
    private static async Task<Dictionary<string, object>> ParseXmlToDictionary(Stream xmlStream)
    {
        var result = new Dictionary<string, object>();
        try
        {
            var xmlDocument = new XmlDocument();
            using var reader = new StreamReader(xmlStream);
            var xmlContent = await reader.ReadToEndAsync().ConfigureAwait(false);
            xmlDocument.LoadXml(xmlContent);

            if (xmlDocument.DocumentElement == null)
            {
                return result;
            }
            
            foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
            {
                AddNodeToDictionary(result, node);
            }
        }
        catch
        {
            return result;
        }

        return result;
    }

    private static void AddNodeToDictionary(Dictionary<string, object> dictionary, XmlNode node)
    {
        if (dictionary.TryGetValue(node.Name, out var existingValue))
        {
            if (existingValue is not List<object> existingList)
            {
                existingList = [existingValue];
                dictionary[node.Name] = existingList;
            }

            existingList.Add(ParseXmlNode(node));
            return;
        }
        
        dictionary[node.Name] = ParseXmlNode(node);
    }

    private static object ParseXmlNode(XmlNode node)
    {
        if (node is { HasChildNodes: true, ChildNodes.Count: 1, FirstChild: XmlText })
        {
            return node.InnerText;
        }
        
        var nodeDict = new Dictionary<string, object>();
        foreach (XmlNode child in node.ChildNodes)
        {
            AddNodeToDictionary(nodeDict, child);
        }

        return nodeDict;
    }
}