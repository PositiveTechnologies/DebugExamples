using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RwlDeadlock
{
	class Program
	{
		private const string Info = @"Deadlock example on upgrade ReaderWriterLock{0}
Press Ctrl+C for exit.{0}";
		
		static void Main(string[] args)
		{
			Console.WriteLine(Info, Environment.NewLine);
			Console.WriteLine($"ProcessId = {Process.GetCurrentProcess().Id}, {ProcessorArchticture()}");
			new UpgradableRwlDeadlock().Show();
		}

		private static string ProcessorArchticture()
		{
			return IntPtr.Size == 8 ? "x64" : "x86";
		}
	}
}
