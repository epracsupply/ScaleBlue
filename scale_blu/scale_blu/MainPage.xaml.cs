using System;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using scale_blu.Bluetooth;
using System.Collections.Generic;

namespace scale_blu
{
    public partial class MainPage : ContentPage
    {

        IEnumerable<BluetoohDevice> devices;

        List<Order> orders;

        private Order currentOrder;
        private int index = -1;

        public MainPage()
        {
            InitializeComponent();

            devices = DependencyService.Get<IBluetoothService>().GetDevices();

            //DevicesList.ItemsSource = devices;

            orders = new List<Order>();

            LoadData();


            NextRec();

        }
        void NextRec()
        {
            index++;
            currentOrder = orders[index];

            ItemCode.Text = currentOrder.ItemCode;
            ItemName.Text = currentOrder.ItemName;
            Quantity.Text = currentOrder.Quantity.ToString();
            TotalWeight.Text = (currentOrder.Quantity * currentOrder.UnitWeight).ToString();
            PickingWeight.Text = "0";
            PickingQuantity.Text = "0";

        }

        void LoadData()
        {
            orders.Clear();

            orders.Add(new Order { ItemCode = "0002", ItemName = "TORNILLOTE", Quantity = 30, UnitWeight = 7.59259 });
            orders.Add(new Order { ItemCode = "0001", ItemName = "ARANDELA DE ROSCA", Quantity = 50, UnitWeight = 3 });
            orders.Add(new Order { ItemCode = "0002", ItemName = "ARANDELA DE LISA", Quantity = 20, UnitWeight = 3 });
            
            orders.Add(new Order { ItemCode = "0002", ItemName = "TORNILLO CON PUNTA", Quantity = 30, UnitWeight = 1.3157 });
            orders.Add(new Order { ItemCode = "0002", ItemName = "TORNILLITO", Quantity = 60, UnitWeight = 0.461538 });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

        }

        private async void DevicesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {




        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var obj = DependencyService.Get<IBluetoothService>();

            try
            {

                //var r = await obj.GetWeight("98:D3:32:11:57:E4");
                var r = await obj.GetWeight("98:D3:32:21:4C:2A");
                //PesoLabel.Text = r.ToString();

                obj.Dispose();


                CalcPicking(double.Parse(r.ToString()));
                //CalcPicking(r);
            }
            catch (Exception ex)
            {


                obj.Dispose();

            }


        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var obj = DependencyService.Get<IBluetoothService>();

            try
            {

                var r = await obj.GetWeight("98:D3:32:11:57:E4");
                //var r = await obj.GetWeight("98:D3:32:21:4C:2A");
                //PesoLabel.Text = r.ToString();


                obj.Dispose();


                CalcPicking(double.Parse(r.ToString()));
            }
            catch (Exception ex)
            {


                obj.Dispose();

            }

        }

        void CalcPicking(double weight)
        {
            //PesoLabel.Text = weight.ToString();

            var dif = 100 - ((100 * (orders[index].Quantity * orders[index].UnitWeight)) / weight); //(100 * (orders[index].Quantity * orders[index].UnitWeight) ) / weight ;

            if (dif > 2)
            {
                PickingWeight.Text = weight.ToString();
                PickingQuantity.Text = "0";
                NextPick.IsEnabled = false;
                PickingQuantity.Text = (weight / currentOrder.UnitWeight).ToString();
                DisplayAlert("Atención", "La cantidad supera el margen de alisto de más", "Ok");
            }
            else
            {
                PickingWeight.Text = weight.ToString();
                PickingQuantity.Text = (weight / currentOrder.UnitWeight).ToString();
                NextPick.IsEnabled = true;
            }



        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            NextRec();
            NextPick.IsEnabled = true;
        }
    }
}
