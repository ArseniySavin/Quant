using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Quant
{
    public class QuantMessage
    {
        public QuantMessage()
        {
            Data = new Dictionary<string, string>();
        }

        public TransactionInfo TransactionInfo { get; set; }
        public List<Correlation> Correlations { get; set; }
        public Call Call { get; set; }
        public Error Error { get; set; }

        public Blob Blob { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public string this[string typeName]
        {
            get
            {
                if (Data.Count == 0) return string.Empty;
                if (!Data.Any(m => m.Key == typeName)) return string.Empty;
                return Data[typeName]; }
            set { Data[typeName] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <exception cref="DuplicateWaitObjectException"></exception>
        public void AddData<T>(T obj)
        {
            Data.Add(typeof(T).Name, JsonSerializer.Serialize<T>(obj));
        }

        public void AddOrUpdateData<T>(T obj)
        {
            var typeName = typeof(T).Name;

            if (string.IsNullOrEmpty(this[typeName]))
            {
                Data.Add(typeName, JsonSerializer.Serialize<T>(obj));
                return;
            }

            this[typeName] = JsonSerializer.Serialize<T>(obj);
        }

        public void AddOrUpdateCorrelation(string reference, string system)
        {
            if (this.Correlations == null)
                throw new QuantCorrelationNullException("QuantMessage.Correlations cannot be null.");

            if (this.Correlations != null && this.Correlations.Any(m => m.Reference == reference && m.System == system))
                throw new QuantCorrelationDublicateException($"The correlation is have the duplicate by {reference} and {system}");

            this.Correlations.Add(new Correlation
            {
                Reference = reference,
                System = system,
                WasExecut = false
            });

        }

        public T GetData<T>()
        {
            return GetData<T>(typeof(T).Name);
        }

        public T GetData<T>(string typeName)
        {
            return JsonSerializer.Deserialize<T>(this[typeName]);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize<QuantMessage>(this);
        }

    }

    [Serializable]
    public class TransactionInfo
    {
        public long TransactionId { get; set; }
        public Guid InstanceId { get; set; }
        public Guid ParentId { get; set; }

        public bool IsParent { get { return ParentId != Guid.Empty;  } }
        public string TransactionCode { get; set; }
        public string SystemCode { get; set; }
        public string SystemReference { get; set; }
    }

    [Serializable]
    public class Correlation
    {
        /// <summary>
        /// Collector, RRN, etc
        /// </summary>
        public string Reference { get; set; }
        public string System { get; set; }
        public bool WasExecut { get; set; }
    }

    [Serializable]
    public class Call
    {
        public string Status { get; set; }
        public DateTime NextCall { get; set; }
        public bool IsWaitNextCall { get { return NextCall > DateTime.Now; } }
        public int RetryCount { get; set; }
    }

    [Serializable]
    public class Error
    {
        public string Code { get; set; }
        public string HelpLink { get; set; }
        public string UserMessage { get; set; }
        public string TechnicalMessage { get; set; }
    }

    [Serializable]
    public class Blob
    {
        public long BlobId { get; set; }
        public string FileName { get; set; }

    }
}
