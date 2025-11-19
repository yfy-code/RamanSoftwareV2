using System;
using System.Windows.Threading;

namespace RamanSoftwareV2.Utils
{
    public static class DispatcherHelper
    {
        public static void Invoke(Action action) =>
            Dispatcher.CurrentDispatcher.Invoke(action);
    }
}
