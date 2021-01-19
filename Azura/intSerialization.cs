// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Provides serialization for signed 32-bit integers.
/// </summary>
public static class intSerialization
{
    /// <summary>
    /// Deserializes a signed 32-bit integer.
    /// </summary>
    /// <param name="stream">Stream to read from.</param>
    /// <returns>Value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Deserialize(Stream stream)
    {
        return SerializationInternals._swap
            ? BinaryPrimitives.ReverseEndianness(
                MemoryMarshal.Read<int>(stream.ReadBase32()))
            : MemoryMarshal.Read<int>(stream.ReadBase32());
    }

    /// <summary>
    /// Serializes a signed 32-bit integer.
    /// </summary>
    /// <param name="self">Value.</param>
    /// <param name="stream">Stream to write to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(this int self, Stream stream)
    {
        if (SerializationInternals._swap) self = BinaryPrimitives.ReverseEndianness(self);
        MemoryMarshal.Write(SerializationInternals.IoBuffer, ref self);
        stream.Write(SerializationInternals.IoBuffer, 0, sizeof(int));
    }

    /// <summary>
    /// Deserializes an array of signed 32-bit integers.
    /// </summary>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="count">Element count.</param>
    /// <returns>Value.</returns>
    public static int[] DeserializeArray(Stream stream, int count)
    {
        int[] res = new int[count];
        stream.ReadSpan<int>(res, count, true);
        return res;
    }

    /// <summary>
    /// Serializes an array of signed 32-bit integers.
    /// </summary>
    /// <param name="self">Value.</param>
    /// <param name="stream">Stream to write to.</param>
    public static void SerializeArray(this ReadOnlySpan<int> self, Stream stream)
    {
        stream.WriteSpan(self, self.Length, true);
    }
}
