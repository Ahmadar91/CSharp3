using System.IO;
using System.Xml.Serialization;

namespace Utilities;

public static class XMLSerializerUtility
{

	/// <summary>Serializes the specified object.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static bool Serialize<T>(T obj, string fileName)
    {
        using (var writer = new StreamWriter(fileName))
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, obj);
            writer.Flush();
        }
        return true;
    }
    /// <summary>Deserializes the specified file path.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath">The file path.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static T Deserialize<T>(string filePath)
    {
        object result = null;
        using (var reader = new StreamReader(filePath))
        {
            var serializer = new XmlSerializer(typeof(T));
            result = (T)serializer.Deserialize(reader);
        }
        return (T)result;
    }
}