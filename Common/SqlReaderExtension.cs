using System;
using System.IO;
using System.Data.Common;

/// <summary>
/// Summary description for SmartDataReader.
/// SQL does not like unsigned types to we have to double cast in some places!!
/// </summary>
public static class SqlReaderExtension
{
    public static T GetValue<T>(this DbDataReader reader, string column, T defValue = default)
    {
        var ordinal = reader.GetOrdinal(column);
        return reader.IsDBNull(ordinal) ? defValue : reader.GetFieldValue<T>(ordinal);
    }

    public static long GetInt64(this DbDataReader reader, string column, long defValue = 0) =>
        reader.GetValue(column, defValue);

    public static ulong GetUInt64(this DbDataReader reader, string column, ulong defValue = 0) =>
        reader.GetValue(column, defValue);

    public static int GetInt32(this DbDataReader reader, string column, int defValue = 0) =>
        reader.GetValue(column, defValue);

    public static uint GetUInt32(this DbDataReader reader, string column, uint defValue = 0) =>
        reader.GetValue(column, defValue);

    public static short GetInt16(this DbDataReader reader, string column, short defValue = 0) =>
        reader.GetValue(column, defValue);

    public static ushort GetUInt16(this DbDataReader reader, string column, ushort defValue = 0) =>
        reader.GetValue(column, defValue);

    public static byte GetByte(this DbDataReader reader, string column, byte defValue = 0) =>
        reader.GetValue(column, defValue);

    public static sbyte GetSByte(this DbDataReader reader, string column, sbyte defValue = 0) =>
        reader.GetValue(column, defValue);

    public static long GetBytes(this DbDataReader reader, string column, long dataOffset, byte[]? buffer, int bufferOffset, int length)
    {
        var ordinal = reader.GetOrdinal(column);
        if (reader.IsDBNull(ordinal))
            return -1;
        return reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
    }

    public static float GetFloat(this DbDataReader reader, string column, float defValue = 0) =>
        reader.GetValue(column, defValue);

    public static double GetDouble(this DbDataReader reader, string column, double defValue = 0) =>
        reader.GetValue(column, defValue);

    public static bool GetBoolean(this DbDataReader reader, string column, bool defValue = false) =>
        reader.GetValue(column, defValue);

    public static string GetString(this DbDataReader reader, string column, string defValue = "") =>
        reader.GetValue(column, defValue);

    public static decimal GetDecimal(this DbDataReader reader, string column, decimal defValue = 0) =>
        reader.GetValue(column, defValue);

    public static DateTime GetDateTime(this DbDataReader reader, string column, DateTime defValue = default) =>
        reader.GetValue(column, defValue);

    public static long GetStream(this DbDataReader reader, string column, MemoryStream ms)
    {
        var ordinal = reader.GetOrdinal(column);
        if (reader.IsDBNull(ordinal))
            return -1;

        const int size = 4096;
        var buffer = new byte[size];

        long count = 0L, rcount;
        do
        {
            rcount = reader.GetBytes(ordinal, count, buffer, 0, size);
            if (rcount > 0)
            {
                ms.Write(buffer, 0, (int)rcount);
                count += rcount;
            }
        } while (rcount > 0L);
        ms.Position = 0;

        return count;
    }
}
