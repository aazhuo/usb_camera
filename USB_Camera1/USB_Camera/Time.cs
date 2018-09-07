using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace USB_Camera
{
    class Time
    {
         public string  GetTime()
            {
            return DateTime.Now.Hour.ToString() +"-"+ DateTime.Now.Minute.ToString() +"-"+ DateTime.Now.Second.ToString();  
             }


    }
}
