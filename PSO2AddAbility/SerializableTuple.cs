using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PSO2AddAbility
{
    [Serializable]
    [XmlRoot("tuple")]
    public class SerializableTuple<T1, T2> : IXmlSerializable, IEquatable<SerializableTuple<T1, T2>>
    {
        private Tuple<T1, T2> _tuple = null;

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public SerializableTuple() { }
        public SerializableTuple(T1 first, T2 second)
        {
            SetValues(first, second);
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region Item1 プロパティ：
        //-------------------------------------------------------------------------------
        public T1 Item1
        {
            get { return (_tuple == null) ? default(T1) : _tuple.Item1; }
        }
        #endregion (Item1)
        //-------------------------------------------------------------------------------
        #region Item2 プロパティ：
        //-------------------------------------------------------------------------------
        public T2 Item2
        {
            get { return (_tuple == null) ? default(T2) : _tuple.Item2; }
        }
        #endregion (Item2)

        //-------------------------------------------------------------------------------
        #region +SetValues
        //-------------------------------------------------------------------------------
        //
        public void SetValues(T1 first, T2 second)
        {
            _tuple = new Tuple<T1, T2>(first, second);
        }
        #endregion (SetValues)

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer firstSerializer = new XmlSerializer(typeof(T1));
            XmlSerializer secondSerializer = new XmlSerializer(typeof(T2));

            reader.Read();

            reader.ReadStartElement("item1");
            T1 first = (T1)firstSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("item2");
            T2 second = (T2)secondSerializer.Deserialize(reader);
            reader.ReadEndElement();

            SetValues(first, second);

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer firstSerializer = new XmlSerializer(typeof(T1));
            XmlSerializer secondSerializer = new XmlSerializer(typeof(T2));

            writer.WriteStartElement("item1");
            firstSerializer.Serialize(writer, this.Item1);
            writer.WriteEndElement();

            writer.WriteStartElement("item2");
            secondSerializer.Serialize(writer, this.Item2);
            writer.WriteEndElement();
        }

        public static bool operator ==(SerializableTuple<T1, T2> left, SerializableTuple<T1, T2> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SerializableTuple<T1, T2> left, SerializableTuple<T1, T2> right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return (_tuple != null && obj is SerializableTuple<T1, T2>) ? _tuple.Equals(obj as SerializableTuple<T1, T2>) : false;
        }

        public bool Equals(SerializableTuple<T1, T2> other)
        {
            return (_tuple != null && other._tuple != null) ? _tuple.Equals(other._tuple) : false;
        }

        public override int GetHashCode()
        {
            return (_tuple == null) ? 0 : _tuple.GetHashCode();
        }

        public override string ToString()
        {
            return (_tuple == null) ? "" : _tuple.ToString();
        }
    }

    //-------------------------------------------------------------------------------
    #region (Class)SerializableTuple
    //-------------------------------------------------------------------------------
    public static class SerializableTuple
    {
        //-------------------------------------------------------------------------------
        #region Create
        //-------------------------------------------------------------------------------
        //
        public static SerializableTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new SerializableTuple<T1, T2>(item1, item2);
        }
        #endregion (Create)
    }
    //-------------------------------------------------------------------------------
    #endregion ((Class)SerializableTuple)
}
