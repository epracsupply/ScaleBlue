using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Util;
using scale_blu.Bluetooth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(scale_blu.Droid.Dependencies.BluetoohtHelper))]
namespace scale_blu.Droid.Dependencies
{

    public class BluetoohtHelper : IBluetooh
    {


        public async Task<decimal> GetWeightWithConnection(string deviceAddress)
        {
            try
            {

                BluetoothManager mBluetoothManager = (BluetoothManager)MainActivity.ActivityCurrent.GetSystemService(Context.BluetoothService);
                BluetoothAdapter adapter = mBluetoothManager.Adapter;


                BluetoothSocket _socket = null;

                var device = (from bd in adapter.BondedDevices where bd.Address == deviceAddress select bd).FirstOrDefault();

                _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                await _socket.ConnectAsync();


                decimal weight = 0;

                bool listening = true;

                Stream inStream = null;

                inStream = (InputStreamInvoker)_socket.InputStream;



                byte[] uintBuffer = new byte[2048 * 2];
                byte[] textBuffer;


                while (listening)
                {


                    try
                    {
                     

                        await Task.Delay(50);   

                        if (!inStream.CanRead)
                        {
                            weight = 0;
                            break;
                        }

                        await inStream.ReadAsync(uintBuffer, 0, uintBuffer.Length);
                        var readLength = BitConverter.ToInt32(uintBuffer, 0);

                        textBuffer = new byte[readLength];

                        // Here we know for how many bytes we are looking for.
                        await inStream.ReadAsync(textBuffer, 0, (int)readLength);

                        string s = Encoding.UTF8.GetString(textBuffer);



                        var x = s.Split(".");


                        if (x.Length <= 1)
                        {
                            weight = 0;
                            break;

                        }
                        else
                        {
                            try
                            {
                                if (x[1].Substring(0, 10).Trim() != String.Empty)
                                {
                                    weight = decimal.Parse(x[1].Substring(0, 10));
                                }
                                else
                                {
                                    weight = 0;

                                }

                                break;
                            }
                            catch (Exception)
                            {
                                weight = 0;
                                break;
                            }
                        }


                    }
                    catch (Java.IO.IOException error)
                    {

                        throw error;
                    }


                }

                _socket.Close();
                inStream.Close();


                return weight;
            }
            catch (Exception)
            {

                throw;
            }


        }



        public async Task<decimal> GetWeight()
        {


            System.Threading.Thread.Sleep(150);

            decimal weight = 0;

            bool listening = true;

            Stream inStream;

            inStream = (InputStreamInvoker)MainActivity._socket.InputStream;


            byte[] uintBuffer = new byte[2048];
            byte[] textBuffer;


            while (listening)
            {
                try
                {

                    try
                    {
                        if (!inStream.CanRead)
                        {
                            weight = 0;
                            break;
                        }

                        await inStream.ReadAsync(uintBuffer, 0, uintBuffer.Length);
                        uint readLength = BitConverter.ToUInt32(uintBuffer, 0);

                        textBuffer = new byte[readLength];
                        // Here we know for how many bytes we are looking for.
                        await inStream.ReadAsync(textBuffer, 0, (int)readLength);

                        string s = Encoding.UTF8.GetString(textBuffer);



                        var x = s.Split(".");


                        if (x.Length <= 1)
                        {
                            weight = 0;
                            break;
                        }
                        else
                        {
                            weight = decimal.Parse(x[1].Substring(0, 10));
                            break;
                        }


                    }
                    catch (Java.IO.IOException error)
                    {

                        throw error;
                    }


                }
                catch (Java.IO.IOException e)
                {

                    listening = false;
                    break;
                }
            }

            inStream.Close();
            MainActivity._socket.Close();

            return weight;


        }

        public async Task<bool> Connect(String DeviceAddress)
        {


            if (MainActivity._socket == null)
            {
                await ConnectBluetoohtSocket(DeviceAddress);
                return MainActivity._socket.IsConnected;
            }
            else
            {
                MainActivity._socket.Close();
                await ConnectBluetoohtSocket(DeviceAddress);
                return MainActivity._socket.IsConnected;

            }



        }

        private async Task ConnectBluetoohtSocket(string DeviceAddress)
        {
            var device = (from bd in MainActivity.adapter.BondedDevices where bd.Address == DeviceAddress select bd).FirstOrDefault();

            MainActivity._socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));


            await MainActivity._socket.ConnectAsync();


            System.Threading.Thread.Sleep(150);
        }

        public List<BluetoohDeviceModel> GetDevices()
        {

            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;

            var devices = (from bd in adapter.BondedDevices select bd).ToList();

            List<BluetoohDeviceModel> result = new List<BluetoohDeviceModel>();

            foreach (BluetoothDevice item in devices)
            {
                result.Add(new BluetoohDeviceModel { DeviceName = item.Name, DeviceAddress = item.Address });
            }

            return result;
        }


        public bool IsConnected()
        {

            if (MainActivity._socket != null)
            {
                return MainActivity._socket.IsConnected;
            }
            else
            {
                return false;
            }
        }

    }

}