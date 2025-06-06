/*********************************************************************
 *
 *  $Id: main.cs 58233 2023-12-04 10:57:58Z seb $
 *
 *  An example that shows how to use a  Yocto-RS232
 *
 *  You can find more information on our web site:
 *   Yocto-RS232 documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-rs232/doc.html
 *   C# API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-cs-EN.html
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YSerialPort serialPort;

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (args.Length > 0 && args[0] != "any") {
        target = args[0];
        serialPort = YSerialPort.FindSerialPort(target + ".serialPort");
        if (!serialPort.isOnline()) {
          Console.WriteLine("No module connected (check cable)");
          Environment.Exit(0);
        }
      } else {
        serialPort = YSerialPort.FirstSerialPort();
        if (serialPort == null) {
          Console.WriteLine("No module connected (check USB cable)");
          Environment.Exit(0);
        }
      }

      serialPort.set_serialMode("9600,8N1");
      serialPort.set_protocol("Line");
      serialPort.reset();

      string line;
      do {
        YAPI.Sleep(500, ref errmsg);
        do {
          line = serialPort.readLine();
          if (line != "") {
            Console.WriteLine("Received: " + line);
          }
        } while (line != "");
        Console.WriteLine("Type line to send, or Ctrl-C to exit: ");
        line = Console.ReadLine();
        serialPort.writeLine(line);
      } while (line != "");
      YAPI.FreeAPI();
    }
  }
}
