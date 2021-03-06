﻿using System;

namespace Messy
{
    /// <summary>
    ///     Sequential guids generator based on native Windows uuid generation methods. 
    ///     Falls back to the NHibernate's method of COMBs generation.
    /// </summary>
    public static class IdGenerator
    {
        private const int RPC_S_OK = 0;

        public static Guid NewSequentialGuid()
        {
            Guid guid;
            var result = NativeMethods.UuidCreateSequential(out guid);
            if (result == RPC_S_OK)
            {
                return guid;
            }

            var guidArray = Guid.NewGuid().ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            var now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            var msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long) (msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }
    }
}