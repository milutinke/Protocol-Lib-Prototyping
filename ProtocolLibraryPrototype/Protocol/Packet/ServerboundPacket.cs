using System.Text;

namespace ProtocolLibraryPrototype.Packets;

public class ServerboundPacket : Packet
{
    private List<byte> packetData = new List<byte>();
    public List<byte> PacketData { get; }

    /// <summary>
    /// Build a VarInt over network
    /// </summary>
    /// <param name="paramInt">Integer to encode</param>
    protected void WriteVarInt(int paramInt)
    {
        while ((paramInt & -128) != 0)
        {
            packetData.Add((byte)(paramInt & 127 | 128));
            paramInt = (int)(((uint)paramInt) >> 7);
        }

        packetData.Add((byte)paramInt);
    }

    /// <summary>
    /// Build a boolean for sending over the network
    /// </summary>
    /// <param name="paramBool">Boolean to encode</param>
    protected void WriteBool(bool paramBool)
    {
        packetData.Add(Convert.ToByte(paramBool));
    }

    private void WriteNumberType<T>(T number) where T : unmanaged
    {
        byte[] bytes = null;

        // I wish there was a cleaner way of doing this
        if (typeof(T) == typeof(short))
            bytes = BitConverter.GetBytes((short)(object)number);
        else if (typeof(T) == typeof(ushort))
            bytes = BitConverter.GetBytes((ushort)(object)number);
        else if (typeof(T) == typeof(int))
            bytes = BitConverter.GetBytes((int)(object)number);
        else if (typeof(T) == typeof(uint))
            bytes = BitConverter.GetBytes((uint)(object)number);
        else if (typeof(T) == typeof(long))
            bytes = BitConverter.GetBytes((long)(object)number);
        else if (typeof(T) == typeof(ulong))
            bytes = BitConverter.GetBytes((ulong)(object)number);
        else if (typeof(T) == typeof(float))
            bytes = BitConverter.GetBytes((float)(object)number);
        else if (typeof(T) == typeof(double))
            bytes = BitConverter.GetBytes((double)(object)number);
        else
            throw new ArgumentException("Type parameter 'number' must be a numeric type.");

        Array.Reverse(bytes);
        packetData.AddRange(bytes);
    }


    /// <summary>
    /// Write a byte array representing a long integer
    /// </summary>
    /// <param name="number">Long to process</param>
    protected void WriteLong(long number)
    {
        WriteNumberType(number);
    }

    /// <summary>
    /// Write a byte array representing an unsigned long integer
    /// </summary>
    /// <param name="number">Long to process</param>
    /// <returns>Array ready to send</returns>
    protected void WriteUnsignedLong(ulong number)
    {
        WriteNumberType(number);
    }

    /// <summary>
    /// Write a byte array representing an integer
    /// </summary>
    /// <param name="number">Integer to process</param>
    protected void WriteInt(int number)
    {
        WriteNumberType(number);
    }

    /// <summary>
    /// Write a byte array representing a short
    /// </summary>
    /// <param name="number">Short to process</param>
    protected void WriteShort(short number)
    {
        WriteNumberType(number);
    }

    /// <summary>
    /// Writes a byte array representing an unsigned short
    /// </summary>
    /// <param name="number">Short to process</param>
    /// <returns>Array ready to send</returns>
    protected void WriteUnsignedShort(ushort number)
    {
        WriteNumberType(number);
    }

    /// <summary>
    /// Writes a byte array representing a double
    /// </summary>
    /// <param name="number">Double to process</param>
    protected void WriteDouble(double number)
    {
        WriteNumberType(number);
    }

    /// <summary>
    /// Writes a byte array representing a float
    /// </summary>
    /// <param name="number">Float to process</param>
    protected void WriteFloat(float number)
    {
        WriteNumberType(number);
    }

    /// <summary>
    /// Write a byte array with length information prepended to it
    /// </summary>
    /// <param name="array">Array to process</param>
    protected void WriteArray(byte[] array)
    {
        WriteVarInt(array.Length);
        WriteByteArray(array);
    }

    /// <summary>
    /// Write a byte array from the given string for sending over the network, with length information prepended.
    /// </summary>
    /// <param name="text">String to process</param>
    protected void WriteString(string text)
    {
        WriteArray(Encoding.UTF8.GetBytes(text));
    }

    /// <summary>
    /// Easily append several byte arrays
    /// </summary>
    /// <param name="bytes">Bytes to append</param>
    protected void WriteByteArray(params byte[][] bytes)
    {
        foreach (byte[] array in bytes)
            packetData.AddRange(array);
    }

    // TODO: Add missing methods
}