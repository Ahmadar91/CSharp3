using System.Collections.Generic;

namespace QuizManager
{
    public interface IListManager<T>
    {
        bool Add(T value);
        bool ReplaceAt(int index, T value);
        bool CheckIndex(int index);
        void RemoveAll(T value);
        bool RemoveAt(int index);
        T GetAt(int index);
        string[] ToStringArray();
        List<string> ToStringList();
        bool BinarySerializer(string fileName);
        bool BinaryDeSerializer(string fileName);
        bool XmlSerialize(string fileName);
        bool XmlDeserialize(string fileName);
    }
}