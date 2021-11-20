using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NativeWifi;
using Windows.Devices.WiFi;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private WiFiAdapter nwAdapter;

        public Form1()
        {
            InitializeComponent();
        }
        void SSID()
        {
            label1.Text = "";
            var strength_arr = new List<int>();
            WlanClient client = new WlanClient();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    int strength = (int)network.wlanSignalQuality;
                    strength_arr.Add(strength);
                    label1.Text += "Found network with SSID "+GetStringForSSID(network.dot11Ssid)+" Signal Quality: "+strength+" \n";
                    
                }
            }
            int max_strength = strength_arr.Max();
            label2.Text = "Nearest POM device's WIFI strength: "+max_strength+ " \n";
        }

        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }
        
        async void Wifiadaptor()
        {
            var access = await WiFiAdapter.RequestAccessAsync();
            var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
            if (result.Count >= 1)
            {
                // take first adapter
                nwAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                // scan for networks
                await nwAdapter.ScanAsync();
                // find network with the correct SSID
                var nw = nwAdapter.NetworkReport.AvailableNetworks.Where(y => y.Ssid.ToLower() == "MyNetworkSSID").FirstOrDefault();
                // connect 
                await nwAdapter.ConnectAsync(nw, WiFiReconnectionKind.Automatic);
            }
        
        }
        
    private void Form1_Load(object sender, EventArgs e)
        {
            SSID();
            //Wifiadaptor();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}





