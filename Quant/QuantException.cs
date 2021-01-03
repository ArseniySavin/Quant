using System;
using System.Collections.Generic;
using System.Text;

namespace Quant
{
    public class QuantBaseException : Exception
    {
        public QuantBaseException(string message) : base(message)
        {
        }
    }

    public class QuantEnviromentNotFoundException : QuantBaseException
    {
        public QuantEnviromentNotFoundException(string message) : base(message)
        {
        }
    }

    public class QuantAppsettingsNotFoundException : QuantBaseException
    {
        public QuantAppsettingsNotFoundException(string message) : base(message)
        {
        }
    }

    public class QuantNotSetAppsettingParException : QuantBaseException
    {
        public QuantNotSetAppsettingParException(string message) : base(message)
        {
        }
    }

    public class QuantRequestReplyEqualsException : QuantBaseException
    {
        public QuantRequestReplyEqualsException(string message) : base(message)
        {
        }
    }

    public class QuantQueueGroupNotEqualsException : QuantBaseException
    {
        public QuantQueueGroupNotEqualsException(string message) : base(message)
        {
        }
    }

    public class QuantMessageRouteInfoException : QuantBaseException
    {
        public QuantMessageRouteInfoException(string message) : base(message)
        {
        }
    }

    public class QuantCorrelationNullException : QuantBaseException
    {
        public QuantCorrelationNullException(string message) : base(message)
        {
        }
    }

    public class QuantCorrelationDublicateException : QuantBaseException
    {
        public QuantCorrelationDublicateException(string message) : base(message)
        {
        }
    }

    public class QuantCallRetryCountException : QuantBaseException
    {
        public QuantCallRetryCountException(string message) : base(message)
        {
        }
    }

}
