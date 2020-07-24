using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common
{
    public interface ISafeConvert
    {
        T SafeConvertTo<T>(object value);
    }

    public class SafeConvert : ISafeConvert
    {
        public T SafeConvertTo<T>(object value)
        {
            if (value is T modelValue)
            {
                return modelValue;
            }
            //处理序列化
            if (value is JObject theJObject)
            {
                return theJObject.ToObject<T>();
            }
            var json = JsonConvert.SerializeObject(value);
            var argsT = JsonConvert.DeserializeObject<T>(json);
            return argsT;
        }

        #region for extension
        
        private static readonly Lazy<SafeConvert> _lazy = new Lazy<SafeConvert>(() => new SafeConvert());
        public static ISafeConvert Instance => ServiceLocator.Current.GetService<ISafeConvert>() ?? _lazy.Value;

        #endregion
    }
}