using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quant;

namespace Quant.Extensions
{
    public static class ConverterExtensions
    {
        /// <summary>
        /// String to Int
        /// </summary>
        /// <param name="s">Any string</param>
        /// <param name="d">Default value</param>
        public static int ToInt(this string s, int d)
        { 
            int r;

            if (int.TryParse(s, out r))
                return r;

            return d;
        }

        /// <summary>
        /// String to Long
        /// </summary>
        /// <param name="s">Any string</param>
        /// <param name="d">Default value</param>
        public static long ToLong(this string s, long d)
        {
            long r;

            if (long.TryParse(s, out r))
                return r;

            return d;
        }

        /// <summary>
        /// String to bool
        /// </summary>
        /// <param name="s">Any string</param>
        /// <param name="d">Default value</param>
        public static bool ToBool(this string s, bool d)
        {
            bool r;

            if (bool.TryParse(s, out r))
                return r;

            return d;
        }
    }

    public static class SerializerMessageExtension
    {
        public static T DeserializeMsg<T>(this byte[] data)
        {
            return JsonSerializer.Deserialize<T>(System.Text.Encoding.UTF8.GetString(data));
        }

        public static T DeserializeMsg<T>(this string data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }

        public static byte[] SerializeMsg<T>(this T data)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(data));
        }
    }

    public static class QuantMessageExtension
    {
        public static QuantMessage Init(this QuantMessage message, Guid instanceId, string transactionCode, string systemCode, string systemReference)
        {
            if (message == null)
                message = new QuantMessage();

            if (message.TransactionInfo != null)
                throw new QuantMessageRouteInfoException("TransactionInfo is not null. It was initialized before");

            message.Call = new Call
            {
                Status = "INIT",
            };

            message.TransactionInfo = new TransactionInfo
            {
                InstanceId = instanceId,
                TransactionCode = transactionCode,
                SystemCode = systemCode,
                SystemReference = systemReference
            };

            message.Correlations = new List<Correlation>();

            return message;
        }
    }
}