﻿using System;
using System.Windows.Forms;

namespace Stinkhorn.Bureau
{
	internal sealed class Program
	{
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}