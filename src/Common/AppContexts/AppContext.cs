using System;
using System.Collections.Generic;

namespace Common.AppContexts
{
    public class MyAppContext : IShouldHaveBags
    {
        public IDictionary<string, object> Items { get; set; } = BagsHelper.Create();

        #region for di extensions

        //don't delete this method!
        public string GetBagsPropertyName()
        {
            return nameof(Items);
        }

        public static MyAppContext Current => Resolve();

        private static readonly Lazy<MyAppContext> _lazy = new Lazy<MyAppContext>(() => new MyAppContext());
        public static Func<MyAppContext> Resolve { get; set; } = () => ServiceLocator.Current.GetService<MyAppContext>() ?? _lazy.Value;

        #endregion
    }
}
