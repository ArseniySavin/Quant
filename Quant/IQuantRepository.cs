using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quant.DB.CLI.Models;

namespace Quant
{
    internal enum QuantRepositoryMapType
    {
        history,
        await,
        tran,
        error
    }
    internal interface IQuantRepository<T> : IDisposable
    {
        IQuantRepository<T> Instance { get; }
        long AddTran(T obj);
        long AddTranHistory(long tranId, long payloadId, string clasterId, string app, string chanel, DateTime timeStamp, QuantMessage obj = null);
        long AddPayload(T obj);
        int AddBlobs(T obj);
        void AddBlobsMap(long payLoadId, long id, QuantRepositoryMapType payLoadType);

        void AddTranAwait(string subject, long tranId, long payloadId, T obj);

        bool ExistsTranAwait(string reference, string system);

        T GetTranAwaitTop(int top, string system);

        T GetTranAwait(string Chanel, string status);

        /// <summary>
        /// Get transaction await
        /// </summary>
        /// <param name="id">[Transaction_ID_REF]</param>
        /// <returns>T</returns>
        T GetTranAwaitByTranId(long id);
        /// <summary>
        /// Get transaction await
        /// </summary>
        /// <param name="id">[Correlation_Reference]</param>
        /// <returns>T</returns>
        T GetTranAwaitByReference(string id);

        /// <summary>
        /// After entity member is getting, deleting entity member
        /// </summary>
        /// <param name="id">[Transaction_ID]</param>
        void DltTranAwait(long id);
        /// <summary>
        /// After entity member is getting, deleting entity member
        /// </summary>
        /// <param name="id">[Correlation_Reference]</param>
        void DltTranAwait(string id);
        /// <summary>
        /// After entity member is getting, deleting entity member
        /// </summary>
        /// <param name="id">[Payload_ID]</param>
        void DltPayload(long id);
    }
}
