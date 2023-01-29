using System.Threading;

namespace Rubik.Toolkit.CancellationTokens
{
    public class ReusableCancellationTokenProvider
    {
        private ReusableCancellationToken _changeToken = new ReusableCancellationToken();
        private ReusableCancellationToken _previousToken = null;

        public void Cancel()
        {
            //Exchange a new ReusableCancellationToken
            _previousToken = Interlocked.Exchange(ref _changeToken, new ReusableCancellationToken());

            //Cancel the last ReusableCancellationToken to notify
            _previousToken.Cancel();
        }

        //Only check _previousToken instead of _changeToken which is reset after canceled
        public bool IsCancellationRequested => _previousToken == null ? _changeToken.HasChanged : _previousToken.HasChanged;
        public ReusableCancellationToken CancellationToken => _changeToken;

        public void Reset()
        {
            _previousToken = null;
        }
    }
}
