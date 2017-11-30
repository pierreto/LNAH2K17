using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Utilities
{
    public class JsonParser
    {
        public static string ParseObjectToString(object element)
        {
            try
            {
                return JsonConvert.SerializeObject(element);
            }
            catch(JsonSerializationException)
            {
                return null;
            }
        }

        public static T ParseStringToObject<T>(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}