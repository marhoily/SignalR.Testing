// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace SignalR.Tests.Common
{
    public class DisposableAction : IDisposable
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields",
            Justification = "The client projects use this.")] public static readonly DisposableAction Empty =
                new DisposableAction(() => { });

        private readonly object _state;

        private Action<object> _action;

        public DisposableAction(Action action)
            : this(state => ((Action) state).Invoke(), action)
        {
        }

        public DisposableAction(Action<object> action, object state)
        {
            _action = action;
            _state = state;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Interlocked.Exchange(ref _action, state => { }).Invoke(_state);
            }
        }
    }
}