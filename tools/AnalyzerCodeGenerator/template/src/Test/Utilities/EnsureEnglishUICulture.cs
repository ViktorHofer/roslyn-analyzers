﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Roslyn.Test.Utilities
{
    public class EnsureEnglishUICulture : IDisposable
    {
        public static CultureInfo PreferredOrNull
        {
            get
            {
                var currentUICultureName = Thread.CurrentThread.CurrentUICulture.Name;
                if (currentUICultureName.Length == 0 || currentUICultureName.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                return CultureInfo.InvariantCulture;
            }
        }

        private bool _needToRestore;
        private readonly CultureInfo _threadUICulture;
        private readonly int _threadId;

        public EnsureEnglishUICulture()
        {
            _threadId = Thread.CurrentThread.ManagedThreadId;
            var preferred = PreferredOrNull;

            if (preferred != null)
            {
                _threadUICulture = Thread.CurrentThread.CurrentUICulture;
                _needToRestore = true;
                Thread.CurrentThread.CurrentUICulture = preferred;
            }
        }

        public void Dispose()
        {
            Debug.Assert(_threadId == Thread.CurrentThread.ManagedThreadId);

            if (_needToRestore && _threadId == Thread.CurrentThread.ManagedThreadId)
            {
                _needToRestore = false;
                Thread.CurrentThread.CurrentUICulture = _threadUICulture;
            }
        }
    }
}
