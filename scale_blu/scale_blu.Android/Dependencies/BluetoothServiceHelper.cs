using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;
using scale_blu.Bluetooth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(scale_blu.Droid.Dependencies.BluetoothServiceHelper))]
namespace scale_blu.Droid.Dependencies
{
    public class BluetoothServiceHelper : IBluetoothService
    {

        private readonly UUID SppRecordUUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");


        private string _bluetoothDeviceAddress;
        private BluetoothSocket _socket;


        public async Task<decimal> GetWeight(string deviceAddress)
        {
            try
            {
                _bluetoothDeviceAddress = deviceAddress;

                var device = GetRemoteDevice(deviceAddress);

                await CreateSocketAndConnectAsync(device);



                byte[] buffer = new byte[16];
                var data = await ReciveAsync(buffer, 0, buffer.Length);

                string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);


                if (string.IsNullOrEmpty(s))
                {
                    return 0;
                }

                if (s.Trim().Contains("S/N      WT-g"))
                {
                    return 0;
                }


                var x = s.Split(".");

                if (x[1].Substring(0, 10).Trim() != String.Empty)
                {
                    return decimal.Parse(x[1].Substring(0, 10));
                }
                else
                {
                    return 0;

                }
            }
            catch (Exception)
            {
                Dispose();

                throw;
            }
            finally
            {
                Dispose();
            }


        }


        private BluetoothDevice GetRemoteDevice(string deviceAddress)
        {

            //public static Activity ActivityCurrent { get; private set; }

            var appContext = Android.App.Application.Context;

            BluetoothManager _manager = (BluetoothManager)appContext.GetSystemService("bluetooth");
            BluetoothAdapter _adapter = _manager.Adapter;

            //var result = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(deviceAddress);
            var result = _adapter.GetRemoteDevice(deviceAddress);

            if (result == null)
            {
                throw new BluetoothConnectionException(
                    $"Can not get remote bluetooth device with address: \"{_bluetoothDeviceAddress}\".");
            }

            return result;
        }


        public List<BluetoohDevice> GetDevices()
        {

            var appContext = Android.App.Application.Context;

            BluetoothManager _manager = (BluetoothManager)appContext.GetSystemService("bluetooth");
            BluetoothAdapter _adapter = _manager.Adapter;


            var devices = (from bd in _adapter.BondedDevices select bd).ToList();

            List<BluetoohDevice> devicesResult = new List<BluetoohDevice>();

            foreach (var item in devices)
            {
                devicesResult.Add(new BluetoohDevice { Name = item.Name, Address = item.Address });
            }

            return devicesResult;
        }

        private async Task CreateSocketAndConnectAsync(BluetoothDevice device)
        {
            var socket = device.CreateRfcommSocketToServiceRecord(SppRecordUUID);

            if (socket == null)
            {
                throw new BluetoothConnectionException(
                    $"Can not connect to the remote bluetooth device with address: \"{_bluetoothDeviceAddress}\". Can not create RFCOMM socket.");
            }

            try
            {
                await socket.ConnectAsync();
                _socket = socket;
            }
            catch
            {
                throw new BluetoothConnectionException(
                    $"Can not connect to the remote bluetooth device with address: \"{_bluetoothDeviceAddress}\". Can not connect to the RFCOMM socket.");
            }
        }

        private void ValidateSocket()
        {
            if (_socket == null)
            {
                throw new BluetoothConnectionException("Can not transmit/recive data because connection is not opened. Plase, use \"Task ConnectAsync()\" method before.");
            }
        }


        public bool DataAvailable
        {
            get
            {
                ValidateSocket();
                try
                {
                    return _socket.InputStream.IsDataAvailable();
                }
                catch (Exception exception)
                {
                    throw new BluetoothReciveException(
                        $"Can not recive is data available for the device with address: \"{_bluetoothDeviceAddress}\"",
                        exception);
                }
            }
        }


        public async Task<int> ReciveAsync(Memory<byte> buffer,
           CancellationToken cancellationToken = default)
        {
            ValidateSocket();
            try
            {
                return await _socket.InputStream.ReadAsync(buffer, cancellationToken);
            }
            catch (Exception exception)
            {
                throw new BluetoothReciveException(
                    $"Can not recive data from the device with address: \"{_bluetoothDeviceAddress}\"",
                    exception);
            }
        }

        public async Task<int> ReciveAsync(byte[] buffer, int offset, int count)
        {
            ValidateSocket();
            try
            {
                return await _socket.InputStream.ReadAsync(buffer, offset, count);
            }
            catch (Exception exception)
            {
                throw new BluetoothReciveException(
                    $"Can not recive data from the device with address: \"{_bluetoothDeviceAddress}\"",
                    exception);
            }
        }

        public async Task<int> ReciveAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ValidateSocket();
            try
            {
                return await _socket.InputStream.ReadAsync(buffer, offset, count, cancellationToken);
            }
            catch (Exception exception)
            {
                throw new BluetoothReciveException(
                    $"Can not recive data from the device with address: \"{_bluetoothDeviceAddress}\"",
                    exception);
            }
        }

        public void Dispose()
        {
            try
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Dispose::Exception", exception.Message);
            }
        }




    }
}