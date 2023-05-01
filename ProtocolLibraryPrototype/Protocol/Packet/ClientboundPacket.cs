using System.Runtime.CompilerServices;
using System.Text;

namespace ProtocolLibraryPrototype.Protocol.Packet;

public abstract class ClientboundPacket : BasePacket
{
    private Queue<byte> _packetData;

    public abstract void Read();

    public void SetPacketData(byte[] data)
    {
        _packetData = new Queue<byte>(data);
    }

    /// <summary>
    /// Read some data from a cache of bytes and remove it from the cache
    /// </summary>
    /// <param name="offset">Amount of bytes to read</param>
    /// <returns>The data read from the cache as an array</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected byte[] ReadData(int offset)
    {
        byte[] result = new byte[offset];
        for (int i = 0; i < offset; i++)
            result[i] = _packetData.Dequeue();
        return result;
    }

    /// <summary>
    /// Read some data from a cache of bytes and remove it from the cache
    /// </summary>
    /// <param name="dest">Storage results</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected void ReadDataReverse(Span<byte> dest)
    {
        for (int i = (dest.Length - 1); i >= 0; --i)
            dest[i] = _packetData.Dequeue();
    }

    /// <summary>
    /// Remove some data from the cache
    /// </summary>
    /// <param name="offset">Amount of bytes to drop</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected void DropData(int offset)
    {
        while (offset-- > 0)
            _packetData.Dequeue();
    }

    /// <summary>
    /// Read a string from a cache of bytes and remove it from the cache
    /// </summary>
    /// <param name="cache">Cache of bytes to read from</param>
    /// <returns>The string</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected string ReadString()
    {
        int length = ReadVarInt();
        return length > 0 ? Encoding.UTF8.GetString(ReadData(length)) : "";
    }

    /// <summary>
    /// Skip a string from a cache of bytes and remove it from the cache
    /// </summary>
    /// <param name="cache">Cache of bytes to read from</param>
    protected void SkipString()
    {
        int length = ReadVarInt();
        DropData(length);
    }

    /// <summary>
    /// Read a boolean from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The boolean value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected bool ReadBool()
    {
        return ReadByte() != 0x00;
    }

    /// <summary>
    /// Read a short integer from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The short integer value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected short ReadShort()
    {
        Span<byte> rawValue = stackalloc byte[2];
        for (int i = (2 - 1); i >= 0; --i) //Endianness
            rawValue[i] = _packetData.Dequeue();
        return BitConverter.ToInt16(rawValue);
    }

    /// <summary>
    /// Read an integer from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The integer value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected int ReadInt()
    {
        Span<byte> rawValue = stackalloc byte[4];
        for (int i = (4 - 1); i >= 0; --i) //Endianness
            rawValue[i] = _packetData.Dequeue();
        return BitConverter.ToInt32(rawValue);
    }

    /// <summary>
    /// Read a long integer from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The unsigned long integer value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected long ReadLong()
    {
        Span<byte> rawValue = stackalloc byte[8];
        for (int i = (8 - 1); i >= 0; --i) //Endianness
            rawValue[i] = _packetData.Dequeue();
        return BitConverter.ToInt64(rawValue);
    }

    /// <summary>
    /// Read an unsigned short integer from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The unsigned short integer value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected ushort ReadUShort()
    {
        Span<byte> rawValue = stackalloc byte[2];
        for (int i = (2 - 1); i >= 0; --i) //Endianness
            rawValue[i] = _packetData.Dequeue();
        return BitConverter.ToUInt16(rawValue);
    }

    /// <summary>
    /// Read an unsigned long integer from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The unsigned long integer value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected ulong ReadULong()
    {
        Span<byte> rawValue = stackalloc byte[8];
        for (int i = (8 - 1); i >= 0; --i) //Endianness
            rawValue[i] = _packetData.Dequeue();
        return BitConverter.ToUInt64(rawValue);
    }

    /// <summary>
    /// Read several little endian unsigned short integers at once from a cache of bytes and remove them from the cache
    /// </summary>
    /// <returns>The unsigned short integer value</returns>
    protected ushort[] ReadUShortsLittleEndian(int amount)
    {
        byte[] rawValues = ReadData(2 * amount);
        ushort[] result = new ushort[amount];
        for (int i = 0; i < amount; i++)
            result[i] = BitConverter.ToUInt16(rawValues, i * 2);
        return result;
    }

    /// <summary>
    /// Read a byte array from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The byte array</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected byte[] ReadByteArray()
    {
        return ReadData(ReadVarInt());
    }

    /// <summary>
    /// Read a byte array with given length from a cache of bytes and remove it from the cache
    /// </summary>
    /// <param name="length">Length of the bytes array</param>
    /// <returns>The byte array</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected byte[] ReadByteArray(int length)
    {
        return ReadData(length);
    }

    /// <summary>
    /// Reads a length-prefixed array of unsigned long integers and removes it from the cache
    /// </summary>
    /// <returns>The unsigned long integer values</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected ulong[] ReadULongArray()
    {
        int len = ReadVarInt();
        ulong[] result = new ulong[len];
        for (int i = 0; i < len; i++)
            result[i] = ReadULong();
        return result;
    }

    /// <summary>
    /// Read a double from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The double value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected double ReadDouble()
    {
        Span<byte> rawValue = stackalloc byte[8];
        for (int i = (8 - 1); i >= 0; --i) //Endianness
            rawValue[i] = _packetData.Dequeue();
        return BitConverter.ToDouble(rawValue);
    }

    /// <summary>
    /// Read a float from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The float value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected float ReadFloat()
    {
        Span<byte> rawValue = stackalloc byte[4];
        for (int i = (4 - 1); i >= 0; --i) //Endianness
            rawValue[i] = _packetData.Dequeue();
        return BitConverter.ToSingle(rawValue);
    }

    /// <summary>
    /// Read an integer from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The integer</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected int ReadVarInt()
    {
        int i = 0;
        int j = 0;
        byte b;

        do
        {
            b = _packetData.Dequeue();
            i |= (b & 0x7F) << j++ * 7;
            if (j > 5) throw new OverflowException("VarInt too big");
        } while ((b & 0x80) == 128);

        return i;
    }

    /// <summary>
    /// Skip a VarInt from a cache of bytes with better performance
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected void SkipVarInt()
    {
        while (true)
            if ((ReadByte() & 0x80) != 128)
                break;
    }

    /// <summary>
    /// Read an "extended short", which is actually an int of some kind, from the cache of bytes.
    /// This is only done with forge.  It looks like it's a normal short, except that if the high
    /// bit is set, it has an extra byte.
    /// </summary>
    /// <returns>The int</returns>
    protected int ReadVarShort()
    {
        ushort low = ReadUShort();
        byte high = 0;
        if ((low & 0x8000) != 0)
        {
            low &= 0x7FFF;
            high = ReadByte();
        }

        return ((high & 0xFF) << 15) | low;
    }

    /// <summary>
    /// Read a long from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The long value</returns>
    protected long ReadVarLong()
    {
        int numRead = 0;
        long result = 0;
        byte read;
        do
        {
            read = ReadByte();
            long value = (read & 0x7F);
            result |= (value << (7 * numRead));

            numRead++;
            if (numRead > 10)
            {
                throw new OverflowException("VarLong is too big");
            }
        } while ((read & 0x80) != 0);

        return result;
    }

    /// <summary>
    /// Read a single byte from a cache of bytes and remove it from the cache
    /// </summary>
    /// <returns>The byte that was read</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    protected byte ReadByte()
    {
        return _packetData.Dequeue();
    }

    // TODO: Add missing methods
}