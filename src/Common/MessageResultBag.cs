using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class MessageResultBag : MessageResult
    {
        public MessageResultBag Set(string key, object value)
        {
            var bag = GetBag();
            bag[key] = value;
            return this;
        }

        public object Get(string key, object defaultValue)
        {
            var bag = GetBag();
            if (bag.TryGetValue(key, out var result))
            {
                return result;
            }
            return defaultValue;
        }

        public MessageResultBag SetAs<T>(string key, T value)
        {
            Set(key, value);
            return this;
        }

        public T GetAs<T>(string key, T defaultValue)
        {
            return (T)Get(key, defaultValue);
        }

        protected internal IDictionary<string, object> GetBag()
        {
            var dic = this.Data as IDictionary<string, object>;
            if (dic != null)
            {
                return dic;
            }

            dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.Data = dic;
            return dic;
        }

        public static MessageResultBag Create(MessageResult messageResult)
        {
            var bag = new MessageResultBag() { Success = messageResult.Success, Message = messageResult.Message, Data = messageResult.Data };
            return bag;
        }
    }

    public class MessageResult<TArgs, TResult> : MessageResultBag
    {
        public TArgs Args
        {
            get => GetAs(nameof(Args), default(TArgs));
            set => SetAs(nameof(Args), value);
        }

        public TResult Result
        {
            get => GetAs(nameof(Result), default(TResult));
            set => SetAs(nameof(Result), value);
        }
    }

    public static class MessageResultBagExtensions
    {
        public static MessageResultBag Slim(this MessageResultBag messageResult, string select = "*")
        {
            if (messageResult == null)
            {
                throw new ArgumentNullException(nameof(messageResult));
            }

            var slimResult = MessageResultBag.Create(messageResult);
            return slimResult.DataSlim(select);
        }

        private static MessageResultBag DataSlim(this MessageResultBag messageResult, string select = "*")
        {
            if (messageResult == null)
            {
                throw new ArgumentNullException(nameof(messageResult));
            }

            if (select.Contains("*"))
            {
                return messageResult;
            }

            var bag = messageResult.GetBag();
            var slimBag = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(select))
            {
                messageResult.Data = slimBag;
                return messageResult;
            }

            var selectKeys = select.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
            foreach (var selectKey in selectKeys)
            {
                if (bag.TryGetValue(selectKey, out var theItem))
                {
                    slimBag[selectKey] = theItem;
                }
            }

            return new MessageResultBag() { Success = messageResult.Success, Message = messageResult.Message, Data = slimBag };
        }


        public static MessageResultBag AsMessageResultBag(this MessageResult messageResult, string originalDataKey = "_")
        {
            if (messageResult == null)
            {
                throw new ArgumentNullException(nameof(messageResult));
            }

            var messageResultBag = messageResult as MessageResultBag;
            if (messageResultBag != null)
            {
                return messageResultBag;
            }

            var bag = MessageResultBag.Create(messageResult);
            if (messageResult.Data != null)
            {
                bag.Set(originalDataKey, messageResult.Data);
            }
            return bag;
        }

        public static MessageResult<TArgs, TResult> As<TArgs, TResult>(this MessageResultBag messageResultBag, TArgs args, TResult result)
        {
            if (messageResultBag == null)
            {
                throw new ArgumentNullException(nameof(messageResultBag));
            }

            var newResult = new MessageResult<TArgs, TResult>();

            newResult.Data = messageResultBag.Data;
            newResult.Args = args;
            newResult.Result = result;

            return newResult;
        }
    }
}