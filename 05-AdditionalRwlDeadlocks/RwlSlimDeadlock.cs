using System.Threading;
using System.Threading.Tasks;

namespace AdditionalRwlDeadlocks
{
	internal class RwlSlimDeadlock
	{
		private readonly ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim();

		public void Show()
		{
			_rwl.EnterWriteLock();

			try
			{
				var t = Task.Factory.StartNew(DoSomeReads, TaskCreationOptions.LongRunning);
				t.Wait();
			}
			finally
			{
				_rwl.ExitWriteLock();
			}
		}

		private void DoSomeReads()
		{
			_rwl.EnterReadLock();
			try
			{
				// do some reads
			}
			finally
			{
				_rwl.ExitReadLock();
			}
		}
	}
}