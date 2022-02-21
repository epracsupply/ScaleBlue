using Android.App;
using Android.Bluetooth;
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
    [BroadcastReceiver]
    [IntentFilter(new[] { BluetoothAdapter.ActionStateChanged })]
    public class BluetoothStateChangedReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Extras == null)
                return;

            var state = intent.GetIntExtra(BluetoothAdapter.ExtraState, -1);

            switch (state)
            {
                case (int)State.On:
                    // bluetooth is on
                    break;
                case (int)State.Off:
                    // bluetooth is off
                    break;
            }
        }
    }
}