

using System;
using System.Runtime.Serialization;

namespace scale_blu.Droid.Dependencies
{
    [Serializable]
    public class BluetoothDataTransferUnitException : Exception
    {
        public BluetoothDataTransferUnitException()
        {
        }

        public BluetoothDataTransferUnitException(string message) : base(message)
        {
        }

        public BluetoothDataTransferUnitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BluetoothDataTransferUnitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}