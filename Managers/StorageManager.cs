using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Windows.Storage;

namespace Site_Manager
{
    class StorageManager
    {
        public static void SaveToFile(object obj, string filename)
        {
            try
            {
                DataContractSerializer ser = new DataContractSerializer(obj.GetType());
                FileStream stream = new FileStream(ApplicationData.Current.LocalFolder.Path + "\\" + filename, FileMode.Create);
                ser.WriteObject(stream, obj);
                stream.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.Source);
            }
        }

        public static object LoadFromFile(Type type, string filename)
        {
            object r = null;

            try
            {
                DataContractSerializer ser = new DataContractSerializer(type);
                FileStream stream = new FileStream(ApplicationData.Current.LocalFolder.Path + "\\" + filename, FileMode.Open);
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                r = ser.ReadObject(reader, true);
                reader.Dispose();
                stream.Dispose();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.Source);
            }

            return r;
        }

    }
}
