using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class WireFormat
{

    #region Fixed sizes.
    // TODO(jonskeet): Move these somewhere else. They're messy. Consider making FieldType a smarter kind of enum
    internal const int Fixed32Size = 4;
    internal const int Fixed64Size = 8;
    internal const int SFixed32Size = 4;
    internal const int SFixed64Size = 8;
    internal const int FloatSize = 4;
    internal const int DoubleSize = 8;
    internal const int BoolSize = 1;
    #endregion


    public enum WireType : uint
    {
        Varint = 0,
        Fixed64 = 1,
        LengthDelimited = 2,
        StartGroup = 3,
        EndGroup = 4,
        Fixed32 = 5
    }

    internal static class MessageSetField
    {
        internal const int Item = 1;
        internal const int TypeID = 2;
        internal const int Message = 3;
    }

    internal static class MessageSetTag
    {
        internal static readonly uint ItemStart = MakeTag(MessageSetField.Item, WireType.StartGroup);
        internal static readonly uint ItemEnd = MakeTag(MessageSetField.Item, WireType.EndGroup);
        internal static readonly uint TypeID = MakeTag(MessageSetField.TypeID, WireType.Varint);
        internal static readonly uint Message = MakeTag(MessageSetField.Message, WireType.LengthDelimited);
    }

    private const int TagTypeBits = 3;
    private const uint TagTypeMask = (1 << TagTypeBits) - 1;

    /// <summary>
    /// Given a tag value, determines the wire type (lower 3 bits).
    /// </summary>

    public static WireType GetTagWireType(uint tag)
    {
        return (WireType)(tag & TagTypeMask);
    }


    public static bool IsEndGroupTag(uint tag)
    {
        return (WireType)(tag & TagTypeMask) == WireType.EndGroup;
    }

    /// <summary>
    /// Given a tag value, determines the field number (the upper 29 bits).
    /// </summary>

    public static int GetTagFieldNumber(uint tag)
    {
        return (int)tag >> TagTypeBits;
    }

    /// <summary>
    /// Makes a tag value given a field number and wire type.
    /// TODO(jonskeet): Should we just have a Tag structure?
    /// </summary>

    public static uint MakeTag(int fieldNumber, WireType wireType)
    {
        return (uint)(fieldNumber << TagTypeBits) | (uint)wireType;
    }
}

