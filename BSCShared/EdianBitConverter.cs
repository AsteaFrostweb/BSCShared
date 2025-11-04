using System;
using System.Text;

namespace BSCShared
{
    public static class EndianBitConverter
    {
        // ====== INTEGER TYPES ======

        public static byte[] GetBytesBigEndian(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesBigEndian(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesBigEndian(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesBigEndian(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesBigEndian(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesBigEndian(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesBigEndian(char value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        // ====== FLOATING POINT TYPES ======

        public static byte[] GetBytesBigEndian(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetBytesBigEndian(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        // ====== READING FROM BIG-ENDIAN BYTES ======

        public static short ToInt16BigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[2];
            Array.Copy(bytes, startIndex, tmp, 0, 2);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToInt16(tmp, 0);
        }

        public static ushort ToUInt16BigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[2];
            Array.Copy(bytes, startIndex, tmp, 0, 2);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToUInt16(tmp, 0);
        }

        public static int ToInt32BigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[4];
            Array.Copy(bytes, startIndex, tmp, 0, 4);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToInt32(tmp, 0);
        }

        public static uint ToUInt32BigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[4];
            Array.Copy(bytes, startIndex, tmp, 0, 4);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToUInt32(tmp, 0);
        }

        public static long ToInt64BigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[8];
            Array.Copy(bytes, startIndex, tmp, 0, 8);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToInt64(tmp, 0);
        }

        public static ulong ToUInt64BigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[8];
            Array.Copy(bytes, startIndex, tmp, 0, 8);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToUInt64(tmp, 0);
        }

        public static char ToCharBigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[2];
            Array.Copy(bytes, startIndex, tmp, 0, 2);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToChar(tmp, 0);
        }

        public static float ToSingleBigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[4];
            Array.Copy(bytes, startIndex, tmp, 0, 4);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToSingle(tmp, 0);
        }

        public static double ToDoubleBigEndian(byte[] bytes, int startIndex = 0)
        {
            byte[] tmp = new byte[8];
            Array.Copy(bytes, startIndex, tmp, 0, 8);
            if (BitConverter.IsLittleEndian) Array.Reverse(tmp);
            return BitConverter.ToDouble(tmp, 0);
        }

        // ====== STRING TYPES ======

        /// <summary>
        /// Converts a string to bytes using UTF-8 encoding, prefixed with its length (big-endian ushort).
        /// </summary>
        public static byte[] GetStringBytesBigEndian(string value)
        {
            if (value == null) return GetBytesBigEndian((ushort)0);

            byte[] stringBytes = Encoding.UTF8.GetBytes(value);
            ushort length = (ushort)stringBytes.Length;

            byte[] lengthBytes = GetBytesBigEndian(length);
            byte[] combined = new byte[2 + stringBytes.Length];

            lengthBytes.CopyTo(combined, 0);
            stringBytes.CopyTo(combined, 2);

            return combined;
        }

        /// <summary>
        /// Reads a string from bytes that were encoded with GetStringBytesBigEndian.
        /// Returns both the string and the number of bytes consumed.
        /// </summary>
        public static string ToStringBigEndian(byte[] bytes, int startIndex, out int bytesRead)
        {
            ushort length = ToUInt16BigEndian(bytes, startIndex);
            bytesRead = 2 + length;

            if (length == 0) return string.Empty;

            byte[] stringBytes = new byte[length];
            Array.Copy(bytes, startIndex + 2, stringBytes, 0, length);

            return Encoding.UTF8.GetString(stringBytes);
        }
    }
}
