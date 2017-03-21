using System.Threading;
using System.Threading.Tasks;

namespace RwlDeadlock
{
	internal class RwlDeadlock
	{
		private readonly ReaderWriterLock _rwl = new ReaderWriterLock();

		public void Show()
		{
			_rwl.AcquireWriterLock(Timeout.Infinite);

			try
			{
				var t = Task.Factory.StartNew(DoSomeReads, TaskCreationOptions.LongRunning);
				t.Wait();
			}
			finally
			{
				_rwl.ReleaseWriterLock();
			}
		}

		private void DoSomeReads()
		{
			_rwl.AcquireReaderLock(Timeout.Infinite);
			try
			{
				// do some reads
			}
			finally
			{
				_rwl.ReleaseReaderLock();
			}
		}
	}
}