using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace XrmVision.Extensions.Extensions
{
    /// <summary>
    /// Methods used for serializing and deserializing JSON using DataContractJsonSerializer (without using Newtonsoft so no need to ILMerge when used in plugin or custom wf activities) 
    /// </summary>
    public static class JsonHelper
    {
        public static string SerializeJSon<T>(this T model)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));

            serializer.WriteObject(stream, model);
            var jsonString = Encoding.UTF8.GetString(stream.ToArray());
            stream.Close();
            return jsonString;
        }

        public static T DeserializeJson<T>(this string json)
        {
            var instance = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }


        //EX , new DataContractJsonSerializerSettings
        //{
        //    DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ss")
        //}
        public static T DeserializeJson<T>(this string json, DataContractJsonSerializerSettings settings)
        {
            var instance = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(instance.GetType(), settings);
                return (T)serializer.ReadObject(ms);
            }
        }

        

    }
}
