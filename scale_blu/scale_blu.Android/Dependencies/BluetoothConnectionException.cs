using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scale_blu.Droid.Dependencies
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BluetoothConnectionException : Exception
    {
        public BluetoothConnectionException()
        {
        }

        public BluetoothConnectionException(string message) : base(message)
        {
        }

        public BluetoothConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BluetoothConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

      
}