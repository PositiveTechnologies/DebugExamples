using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RwlDeadlock
{
	internal sealed class UpgradableRwlDeadlock
	{
		private readonly Dictionary<string, ReallyExpensiveObject> expensiveObjectCache = new Dictionary<string, ReallyExpensiveObject>();

		private readonly ReaderWriterLock _rwl = new ReaderWriterLock();

		public void Show()
		{
			Parallel.For(0, int.MaxValue - 1, i => GetExpensiveObject(i.ToString()));
		}

		private ReallyExpensiveObject GetExpensiveObject(string key)
		{
			_rwl.AcquireReaderLock(Timeout.Infinite);

			try
			{
				ReallyExpensiveObject expensiveObject;
				if (this.expensiveObjectCache.TryGetValue(key, out expensiveObject))
				{
					// Cache hit
					return expensiveObject;
				}

				// Cache miss
				LockCookie cookie;

				cookie = _rwl.UpgradeToWriterLock(Timeout.Infinite);
				try
				{
					expensiveObject = new ReallyExpensiveObject();
					expensiveObjectCache[key] = expensiveObject;
					return expensiveObject;
				}

				finally
				{
					_rwl.DowngradeFromWriterLock(ref cookie);
				}
			}
			finally
			{
				_rwl.ReleaseReaderLock();
			}
		}

		class ReallyExpensiveObject { }
	}
}