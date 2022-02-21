using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace scale_blu.Bluetooth
{
    public interface IBluetoothService
    {
        List<BluetoohDevice> GetDevices();
        Task<decimal> GetWeight(string deviceAddress);
        void Dispose();
    }
}
