﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void usage()
    {
      string execname = System.AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine(execname + " <serial_number> ");
      Console.WriteLine(execname + " <logical_name>");
      Console.WriteLine(execname + "  any ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YDisplay disp;
      YDisplayLayer l0;

      if (args.Length < 1) usage();

      target = args[0].ToUpper();

      // API init
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      // find the display according to command line parameters
      if (target == "ANY") {
        disp = YDisplay.FirstDisplay();
        if (disp == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      } else disp = YDisplay.FindDisplay(target + ".display");


      if (!disp.isOnline()) {
        Console.WriteLine("Module not connected (check identification and USB cable) ");
        Environment.Exit(0);
      }

      disp.resetAll();
      // retreive the display size
      int w = disp.get_displayWidth();
      int h = disp.get_displayHeight();

      // reteive the first layer
      l0 = disp.get_displayLayer(0);
      int bytesPerLines = w / 8;

      byte[] data = new byte[h * bytesPerLines];
      for (int i = 0; i < data.Length; i++) data[i] = 0;

      int max_iteration = 50;
      int iteration;
      double xtemp;
      double centerX = 0;
      double centerY = 0;
      double targetX = 0.834555980181972;
      double targetY = 0.204552998862566;
      double x, y, x0, y0;
      double zoom = 1;
      double distance = 1;

      while (true) {
        for (int i = 0; i < data.Length; i++) data[i] = 0;
        distance = distance * 0.95;
        centerX = targetX * (1 - distance);
        centerY = targetY * (1 - distance);
        max_iteration = (int)Math.Round(max_iteration + Math.Sqrt(zoom));
        if (max_iteration > 1500) max_iteration = 1500;
        for (int j = 0; j < h; j++)
          for (int i = 0; i < w; i++) {
            x0 = (((i - w / 2.0) / (w / 8)) / zoom) - centerX;
            y0 = (((j - h / 2.0) / (w / 8)) / zoom) - centerY;

            x = 0;
            y = 0;

            iteration = 0;

            while ((x * x + y * y < 4) && (iteration < max_iteration)) {
              xtemp = x * x - y * y + x0;
              y = 2 * x * y + y0;
              x = xtemp;
              iteration += 1;
            }

            if (iteration >= max_iteration)
              data[j * bytesPerLines + (i >> 3)] |= (byte)(128 >> (i % 8));

          }

        l0.drawBitmap(0, 0, w, data, 0);
        zoom = zoom / 0.95;
      }
    }
  }
}
