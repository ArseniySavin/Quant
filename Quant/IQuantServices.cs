using System;
using System.Collections.Generic;
using System.Text;
using Quant.DB.CLI.Models;

namespace Quant
{
    /// <summary>
    /// Interface is representing the methods for background services 
    /// </summary>
    public interface IQuantServices
    {
        /// <summary>
        /// Represents instance for waiting correlation
        /// </summary>
        void AwaitServices();

        /// <summary>
        /// Represents instance for resolving an problem
        /// </summary>
        void ErrorServices();
        /// <summary>
        /// Represents main stream for publisher or subscribers
        /// </summary>
        void TripServices();
    }
}
