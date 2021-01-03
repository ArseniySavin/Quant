using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quant.Extensions;
using Quant.DB.CLI.Models;

namespace Quant
{
    internal class QuantRepository : IQuantRepository<QuantMessage>
    {
        QuantContext _context;

        public QuantRepository(QuantContext context)
        {
            _context = context;
        }

        public IQuantRepository<QuantMessage> Instance
        {
            get
            {
                return this;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int AddBlobs(QuantMessage obj)
        {
            throw new NotImplementedException();
        }

        public void AddBlobsMap(long payLoadId, long id, QuantRepositoryMapType payLoadType)
        {
            throw new NotImplementedException();
        }

        public long AddPayload(QuantMessage obj)
        {
            var res = _context.Add<Payload>(
                new Payload
                {
                    Content = obj.ToString()
                }).Entity;

            _context.SaveChanges();

            return res.PayloadId;
        }

        public long AddTran(QuantMessage obj)
        {
            if (_context.Transactions.Any(m => m.InstanceId == obj.TransactionInfo.InstanceId 
                                            || m.TransactionId == obj.TransactionInfo.TransactionId))
                return _context.Transactions.First(m => m.InstanceId == obj.TransactionInfo.InstanceId).TransactionId;

            var res =_context.Add<Transactions>(
                new Transactions
                {
                    TransactionCode = obj.TransactionInfo.TransactionCode,
                    InstanceId = obj.TransactionInfo.InstanceId,
                    ParentIdRef = obj.TransactionInfo.ParentId,
                    Parent = obj.TransactionInfo.IsParent,
                    SystemReference = obj.TransactionInfo.SystemReference,
                    SystemCode = obj.TransactionInfo.SystemCode
                });

            _context.SaveChanges();

            return res.Entity.TransactionId;
        }

        public void AddTranAwait(string subject, long tranId, long payloadId, QuantMessage obj)
        {
            var corellation = obj.Correlations.LastOrDefault(m => !m.WasExecut);

            if (corellation is null)
                throw new QuantCorrelationNullException("QuantMessage.Correlations cannot be null. If you are setting this instance as self waiting. You must add current instance as self");

            if (ExistsTranAwait(corellation.Reference, corellation.System))
                return;

            var res = _context.Add<TransactionAwait>(
                new TransactionAwait
                {
                    TransactionIdRef = tranId,
                    CorrelationReference = corellation.Reference,
                    CorrelationSystem = corellation.System,
                    NextCall = obj.Call.NextCall,
                    RetryCount = obj.Call.RetryCount,
                    Status = obj.Call.Status,
                    PayloadIdRef = payloadId,
                    Chanel = subject
                });

            _context.SaveChanges();
        }

        public bool ExistsTranAwait(string reference, string system)
        {
            return _context.TransactionAwait.Any(m => m.CorrelationReference == reference && m.CorrelationSystem == system);
        }

        public long AddTranHistory(long tranId, long payloadId, string clasterId, string app, string chanel, DateTime timeStamp, QuantMessage obj = null)
        {
            var res = _context.Add<TransactionsHistory>(
                new TransactionsHistory
                {
                    TransactionIdRef = tranId,
                    App = app,
                    Chanel = chanel,
                    ClasterId = clasterId,
                    TimeStamp = timeStamp,
                    PayloadIdRef = payloadId
                });

            _context.SaveChanges();

            return res.Entity.TransactionsHistoryId;
        }

        public void DltTranAwait(long id)
        {
            var res = _context.TransactionAwait.FirstOrDefault(m => m.TransactionAwaitId == id);

            if (res == null)
                return;

            _context.Remove(res);
            _context.SaveChanges();
        }

        public void DltTranAwait(string id)
        {
            var res = _context.TransactionAwait.FirstOrDefault(m => m.CorrelationReference == id);

            if (res == null)
                return;

            _context.Remove(res);
            _context.SaveChanges();
        }

        public QuantMessage GetTranAwaitByTranId(long id)
        {
            throw new NotImplementedException();
        }

        public QuantMessage GetTranAwaitByReference(string id)
        {
            throw new NotImplementedException();
        }

        public QuantMessage GetTranAwait(string chanel, string status)
        {
            var tranAwait = _context.TransactionAwait.FirstOrDefault(m => m.Chanel == chanel && m.Status == status);

            if (tranAwait == null)
                return null;

            var payload = _context.Payload.FirstOrDefault(m => m.PayloadId == tranAwait.PayloadIdRef);

            if (payload == null)
                return null;

            var res = payload.Content.DeserializeMsg<QuantMessage>();

            DltTranAwait(tranAwait.TransactionAwaitId);
            DltPayload(tranAwait.PayloadIdRef);

            return res;
        }

        public QuantMessage GetTranAwaitTop(int top, string system)
        {
            throw new NotImplementedException();
        }

        public void DltPayload(long id)
        {
            var res = _context.Payload.FirstOrDefault(m => m.PayloadId == id);

            if (res == null)
                return;

            _context.Remove(res);
            _context.SaveChanges();
        }
    }
}
