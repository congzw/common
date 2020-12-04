using System;
using System.Collections.Generic;

namespace Common.AppContexts
{
    public class MyAppContext : IShouldHaveBags
    {
        public IDictionary<string, object> Items { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        
        public string GetBagsPropertyName()
        {
            return nameof(Items);
        }

        #region for di extensions

        public static MyAppContext Current => Resolve();

        private static readonly Lazy<MyAppContext> Lazy = new Lazy<MyAppContext>(() => new MyAppContext());

        public static Func<MyAppContext> Resolve { get; set; } = () => ServiceLocator.Current.GetService(() => Lazy.Value);

        #endregion
    }


}
