// -----------------------------------------------------------------------
// <copyright file="IsExternalInit.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

#if NETSTANDARD2_0
namespace System.Runtime.CompilerServices;

// Required for init accessors emitted by the shared quantity generator.
internal static class IsExternalInit
{
}
#endif
