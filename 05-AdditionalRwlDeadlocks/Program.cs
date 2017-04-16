using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdditionalRwlDeadlocks
{
	class Program
	{
		private const string Info = @"Deadlock example on ReaderWriterLock and ReaderWriterLockSlim.
1. Deadlock on ReaderWriterLock
2. Deadlock on ReaderWriterLockSlim
Press Ctrl+C for exit.{0}";

		private static readonly string[] ExampleNames =
		{
			"Deadlock on ReaderWriterLock",
			"Deadlock on ReaderWriterLockSlim"
		};


		static void Main(string[] args)
		{
			Console.WriteLine(Info, Environment.NewLine);
			Console.WriteLine($"ProcessId = {Process.GetCurrentProcess().Id}, {ProcessorArchticture()}");

			int exampleNumber = GetExampleNumber();
			Console.WriteLine($"{ExampleNames[exampleNumber - 1]} has been chosen.");

			RunExample(exampleNumber);
		}

		private static void RunExample(int exampleNumber)
		{
			switch (exampleNumber)
			{
				case 1:
					new RwlDeadlock().Show();
					break;
				case 2:
					new RwlSlimDeadlock().Show();
					break;
				default:
					return;
			}
		}

		private static int GetExampleNumber()
		{
			while (true)
			{
				Console.Write("Choose example (1-2) or q to exit: ");
				var input = Console.ReadLine();
				if (string.IsNullOrEmpty(input))
					continue;

				int exampleNumber;
				if (int.TryParse(input, out exampleNumber) && (exampleNumber >= 1 && exampleNumber <= 2))
					return exampleNumber;

				if (input.ToLowerInvariant() == "q")
					Environment.Exit(0);
			}
		}

		private static string ProcessorArchticture()
		{
			return IntPtr.Size == 8 ? "x64" : "x86";
		}
	}
}
