using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ivoryservices.Helper
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            //return value == null ? default(T) : (T)value

            //return IList<value>  == null ? default(T) : JsonConvert.DeserializeObject<List<T>>(value);
        }
    }
}
