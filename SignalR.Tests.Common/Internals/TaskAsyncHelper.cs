// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#if !SERVER
#endif

namespace SignalR.Tests.Common
{
    public static class TaskAsyncHelper
    {
        private static readonly Task _emptyTask = MakeTask<object>(null);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This is a shared file")]
        public static Task Empty
        {
            get { return _emptyTask; }
        }

        private static Task<T> MakeTask<T>(T value)
        {
            return FromResult(value);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This is a shared file")]
        private static Task<T> FromResult<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }
    }
}