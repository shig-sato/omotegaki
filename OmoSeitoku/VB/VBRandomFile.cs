using System;
using System.IO;
using System.Text;

namespace OmoSeitoku.VB
{
    public static class VBRandomFile
    {
        public const long SIZE_INT = 2;
        public const long SIZE_LONG = 4;
        public const long SIZE_BOOL = 2;
        public const long SIZE_DATE = 4;
        public const long SIZE_DATETIME = 8;

        public static readonly DateTime BASE_DATETIME = new DateTime(1899, 12, 30, 0, 0, 0);

        private const long ONE_DAY_TICKS = 24L * 60L * 60L * 10000000L;


        public static int ReadInt(BinaryReader bin)
        {
            return bin.ReadInt16();
        }

        public static long ReadLong(BinaryReader bin)
        {
            return bin.ReadInt32();
        }

        public static string ReadString(BinaryReader bin)
        {
            int len = bin.ReadInt16();
            byte[] data = bin.ReadBytes(len);
            return Encoding.Default.GetString(data);
        }

        public static string ReadString(BinaryReader bin, Encoding encode)
        {
            int len = bin.ReadInt16();
            byte[] data = bin.ReadBytes(len);
            return encode.GetString(data);
        }

        public static string ReadFixedLengthString(BinaryReader bin, int length)
        {
            byte[] data = bin.ReadBytes(length);
            return Encoding.Default.GetString(data);
        }

        public static string ReadFixedLengthString(BinaryReader bin, int length, Encoding encode)
        {
            byte[] data = bin.ReadBytes(length);
            return encode.GetString(data);
        }

        public static bool ReadBool(BinaryReader bin)
        {
            int data = bin.ReadInt16();
            if (data == 0) return false;
            return true;
        }

        public static DateTime? ReadDate(BinaryReader bin)
        {
            int data = bin.ReadInt32();
            if (data == 0) return null;

            var span = new TimeSpan(data, 0, 0, 0);
            long resultTicks = BASE_DATETIME.Ticks + span.Ticks;
            if (resultTicks < DateTime.MinValue.Ticks || resultTicks > DateTime.MaxValue.Ticks)
                return null;

            return BASE_DATETIME + new TimeSpan(data, 0, 0, 0);
        }

        public static DateTime? ReadDateTime(BinaryReader bin)
        {
            double data = bin.ReadDouble();
            if (data == 0) return null;
            int intData = (int)data;
            return BASE_DATETIME + new TimeSpan(intData, 0, 0, 0)
                             + new TimeSpan((long)(ONE_DAY_TICKS * (data - intData)));
        }
    }
}
