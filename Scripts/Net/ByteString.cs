using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public sealed class ByteString : IEnumerable<byte>, IEquatable<ByteString>
{
    private static readonly ByteString empty = new ByteString(new byte[0]);

    private readonly byte[] bytes;

    /// <summary>
    /// 在调用该构造函数后，不能复制不能修改
    /// </summary>
    private ByteString(byte[] bytes)
    {
        this.bytes = bytes;
    }

    /// <summary>
    /// Returns an empty ByteString.
    /// </summary>
    public static ByteString Empty
    {
        get { return empty; }
    }

    /// <summary>
    /// Returns the length of this ByteString in bytes.
    /// </summary>
    public int Length
    {
        get { return bytes.Length; }
    }

    public bool IsEmpty
    {
        get { return Length == 0; }
    }

    public byte[] ToByteArray()
    {
        return (byte[])bytes.Clone();
    }

    /// <summary>
    /// Constructs a ByteString from the Base64 Encoded String.
    /// </summary>
    public static ByteString FromBase64(string bytes)
    {
        return new ByteString(System.Convert.FromBase64String(bytes));
    }

    /// <summary>
    /// Constructs a ByteString from the given array. The contents
    /// are copied, so further modifications to the array will not
    /// be reflected in the returned ByteString.
    /// </summary>
    public static ByteString CopyFrom(byte[] bytes)
    {
        return new ByteString((byte[])bytes.Clone());
    }

    /// <summary>
    /// Constructs a ByteString from a portion of a byte array.
    /// </summary>
    public static ByteString CopyFrom(byte[] bytes, int offset, int count)
    {
        byte[] portion = new byte[count];
        Array.Copy(bytes, offset, portion, 0, count);
        return new ByteString(portion);
    }

    /// <summary>
    /// Creates a new ByteString by encoding the specified text with
    /// the given encoding.
    /// </summary>
    public static ByteString CopyFrom(string text, Encoding encoding)
    {
        return new ByteString(encoding.GetBytes(text));
    }

    /// <summary>
    /// Creates a new ByteString by encoding the specified text in UTF-8.
    /// </summary>
    public static ByteString CopyFromUtf8(string text)
    {
        return CopyFrom(text, Encoding.UTF8);
    }

    /// <summary>
    /// Retuns the byte at the given index.
    /// </summary>
    public byte this[int index]
    {
        get { return bytes[index]; }
    }

    public string ToString(Encoding encoding)
    {
        return encoding.GetString(bytes, 0, bytes.Length);
    }

    public string ToStringUtf8()
    {
        return ToString(Encoding.UTF8);
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return ((IEnumerable<byte>)bytes).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Creates a CodedInputStream from this ByteString's data.
    /// </summary>
    public CodedInputStream CreateCodedInput()
    {

        // We trust CodedInputStream not to reveal the provided byte array or modify it
        return CodedInputStream.CreateInstance(bytes);
    }

    // TODO(jonskeet): CopyTo if it turns out to be required

    public override bool Equals(object obj)
    {
        ByteString other = obj as ByteString;
        if (obj == null)
        {
            return false;
        }
        return Equals(other);
    }

    public override int GetHashCode()
    {
        int ret = 23;
        for (int i = 0; i < bytes.Length; ++i)
        {
            ret = (ret << 8) | bytes[i];
        }
        return ret;
    }

    public bool Equals(ByteString other)
    {
        if (other.bytes.Length != bytes.Length)
        {
            return false;
        }
        for (int i = 0; i < bytes.Length; i++)
        {
            if (other.bytes[i] != bytes[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Builder for ByteStrings which allows them to be created without extra
    /// copying being involved. This has to be a nested type in order to have access
    /// to the private ByteString constructor.
    /// </summary>
    internal sealed class CodedBuilder
    {
        private readonly CodedOutputStream output;
        private readonly byte[] buffer;

        internal CodedBuilder(int size)
        {
            buffer = new byte[size];
            output = CodedOutputStream.CreateInstance(buffer);
        }

        internal ByteString Build()
        {
            output.CheckNoSpaceLeft();

            // We can be confident that the CodedOutputStream will not modify the
            // underlying bytes anymore because it already wrote all of them.  So,
            // no need to make a copy.
            return new ByteString(buffer);
        }

        internal CodedOutputStream CodedOutput
        {
            get
            {
                return output;
            }
        }
    }
}

