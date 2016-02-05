using System;
using System.Runtime.InteropServices;

namespace Messy
{
    public class NativeMethods
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreateSequential(out Guid guid);
    }
}