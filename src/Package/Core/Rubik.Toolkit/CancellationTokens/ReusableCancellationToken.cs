using System;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Rubik.Toolkit.CancellationTokens
{
    public class ReusableCancellationToken : IChangeToken
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public bool ActiveChangeCallbacks => true;

        public bool HasChanged => _cts.IsCancellationRequested;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => _cts.Token.Register(callback, state);

        public void Cancel() => _cts.Cancel();

        public CancellationToken Token => _cts.Token;
    }
}
