using System;
using System.Collections.Generic;

namespace PixelCrew.Utils.Disposables
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public void Retain(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }
    }
}
