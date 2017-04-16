using System;
using System.Diagnostics;
using System.Threading;

namespace MemoryLeak
{
	class Program
	{
		private static readonly GlobalNotifier Notifier = new GlobalNotifier();

		static void Main(string[] args)
		{
			Console.WriteLine("Simple example of managed memory leak");
			Console.WriteLine("Press Ctrl+C for exit.\n");
			Console.WriteLine($"ProcessId = {Process.GetCurrentProcess().Id}, {ProcessorArchticture()}");

			while (true)
			{
				var obj = new Worker();
				Notifier.SomethingHappened += obj.SomethingHappened;
				obj.Work();

				new Reporter().Do();
				
				Thread.Sleep(50);
			}
		}

		private static string ProcessorArchticture()
		{
			return IntPtr.Size == 8 ? "x64" : "x86";
		}
	}

	internal sealed class GlobalNotifier
	{
		public event EventHandler<EventArgs> SomethingHappened;

		public void Notify()
		{
			var evt = SomethingHappened;
			evt?.Invoke(this, EventArgs.Empty);
		}
	}

	internal sealed class Worker
	{
		private readonly long[] _someData = new long[10 * 1000];

		public void SomethingHappened(object sender, EventArgs arg)
		{
		}

		public void Work()
		{
			for (int i = 0; i < _someData.Length; ++i)
			{
				_someData[i] = i;
			}
		}

	}

	internal sealed class Reporter
	{
		private readonly long[] _someData = new long[10 * 1000];
		public void Do()
		{
			for (int i = 0; i < _someData.Length; ++i)
			{
				_someData[i] = i;
			}
		}

	}


}
