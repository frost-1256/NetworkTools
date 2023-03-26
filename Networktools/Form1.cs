using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Networktools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static IPAddress GetSubnetMask()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .SelectMany(iface => iface.GetIPProperties().UnicastAddresses)
                .Where(address => address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(address => address.IPv4Mask)
                .FirstOrDefault();
        }
        public static IPAddress GetDefaultGateway()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface iface in interfaces)
            {
                IPInterfaceProperties properties = iface.GetIPProperties();
                GatewayIPAddressInformationCollection gatewayAddresses = properties.GatewayAddresses;
                if (gatewayAddresses.Count > 0)
                {
                    foreach (GatewayIPAddressInformation gatewayAddress in gatewayAddresses)
                    {
                        if (gatewayAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return gatewayAddress.Address;
                        }
                    }
                }
            }

            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = GetSubnetMask().ToString();
            textBox4.Text = string.IsNullOrEmpty(textBox4.Text) ? "Err/Null" : textBox4.Text;
            textBox4.Text = GetDefaultGateway().ToString();
            // ChatGPTに生成してもらったコード+
            string localIP;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string domainName = textBox1.Text;
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    textBox1.Text = localIP;
                    break;
                }
            }
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                foreach (IPAddress dns in properties.DnsAddresses)
                {
                    if (dns.AddressFamily == AddressFamily.InterNetwork)
                    {
                        textBox2.Text = dns.ToString();
                    }
                }
            }
        }
    }
}
