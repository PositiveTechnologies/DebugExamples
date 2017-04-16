using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorDeadlock
{
	class Program
	{
		private static readonly object Lock1 = new object();
		private static readonly object Lock2 = new object();

		private static readonly Barrier Barrier = new Barrier(2); // To enforce deadlock
		
		static void Main(string[] args)
		{
			Console.WriteLine("Simple deadlock example on lock construction.");
			Console.WriteLine("Press Ctrl+C for exit.\n");
			Console.WriteLine($"ProcessId = {Process.GetCurrentProcess().Id}, {ProcessorArchticture()}");

			var t1 = Task.Factory.StartNew(Thread1, TaskCreationOptions.LongRunning);
			var t2 = Task.Factory.StartNew(Thread2, TaskCreationOptions.LongRunning);

			Task.WaitAll(t1, t2);
		}

		private static void Thread1()
		{
			lock (Lock1)
			{
				Barrier.SignalAndWait(); // Forcing deadlock

				lock (Lock2)
				{
				}
			}
		}

		private static void Thread2()
		{
			lock (Lock2)
			{
				Barrier.SignalAndWait(); // Forcing deadlock

				lock (Lock1)
				{
				}
			}
		}

		private static string ProcessorArchticture()
		{
			return IntPtr.Size == 8 ? "x64" : "x86";
		}
	}
}
