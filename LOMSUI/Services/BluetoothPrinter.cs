using Android.Bluetooth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Services
{
    public class BluetoothPrinter
    {
        private BluetoothSocket _socket;

        public async Task ConnectAsync(BluetoothDevice device)
        {
            var uuid = device.GetUuids()?.FirstOrDefault()?.Uuid ?? UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"); // SPP UUID
            _socket = device.CreateRfcommSocketToServiceRecord(uuid);
            await _socket.ConnectAsync();
        }

        public void PrintText(string text)
        {
            if (_socket != null && _socket.IsConnected)
            {
                var stream = _socket.OutputStream;
                byte[] bytes = Encoding.GetEncoding("GB18030").GetBytes(text); // Dùng GB18030 để hỗ trợ tiếng Việt cơ bản
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Disconnect()
        {
            _socket?.Close();
            _socket = null;
        }
    }

}
