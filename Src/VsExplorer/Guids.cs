// Guids.cs
// MUST match guids.h
using System;

namespace JaredPar.VsExplorer
{
    static class GuidList
    {
        public const string guidVsExplorerPkgString = "7848ffb2-a704-43a2-9ef7-f0081cfc10d6";
        public const string guidVsExplorerCmdSetString = "0a91773a-2787-42ca-9616-61ecf4f8a502";
        public const string guidToolWindowPersistanceString = "1cd2ad05-d2d2-4151-958b-a1d13c547171";

        public static readonly Guid guidVsExplorerCmdSet = new Guid(guidVsExplorerCmdSetString);
    };
}