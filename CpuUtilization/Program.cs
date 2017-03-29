using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpuUtilization
{
	class Program
	{
		private static ConcurrentDictionary<Guid, Asset> _cache = new ConcurrentDictionary<Guid, Asset>();


		static void Main(string[] args)
		{
			Console.WriteLine("Example of a heavy cpu utilization application.");
			Console.WriteLine("Press Ctrl+C for exit.\n");
			Console.WriteLine($"ProcessId = {Process.GetCurrentProcess().Id}, {ProcessorArchticture()}");

			var processorCount = Environment.ProcessorCount;
			var tasks = new Task[processorCount];

			for (int i = 0; i < processorCount; i++)
			{
				var task = Task.Factory.StartNew(DoSomeHeavyWork, TaskCreationOptions.LongRunning);
				tasks[i] = task;
			}
			
			Task.WaitAll(tasks);
		}

		private static void DoSomeHeavyWork()
		{
			while (true)
			{
				ResolveAsset(Guid.NewGuid());
			}
		}

		private static string ProcessorArchticture()
		{
			return IntPtr.Size == 8 ? "x64" : "x86";
		}

		private static Asset ResolveAsset(Guid assetId)
		{
			try
			{
				return _cache[assetId];
			}
			catch (KeyNotFoundException)
			{
				Init(assetId);
			}

			return _cache[assetId];
		}

		private static void Init(Guid assetId)
		{
			_cache.TryAdd(assetId, new Asset());
		}
	
	}

	class Asset { }
}
