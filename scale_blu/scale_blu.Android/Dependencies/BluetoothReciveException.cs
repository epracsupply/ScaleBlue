using System;
using System.Runtime.Serialization;

namespace scale_blu.Droid.Dependencies
{
    [Serializable]
    public class BluetoothReciveException : BluetoothDataTransferUnitException
    {
        public BluetoothReciveException()
        {
        }

        public BluetoothReciveException(string message) : base(message)
        {
        }

        public BluetoothReciveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BluetoothReciveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}