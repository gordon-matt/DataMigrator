﻿namespace DataMigrator.Common.Models;

public enum FieldType : byte
{
    Unknown = 0,
    Binary = 1,
    Byte = 2,
    Boolean = 3,
    Char = 4,
    Choice = 5,
    Calculated = 6,
    Currency = 7,
    Date = 8,
    DateTime = 9,
    DateTimeOffset = 10,
    Decimal = 11,
    Double = 12,
    Geometry = 13,
    Guid = 14,
    Int16 = 15,
    Int32 = 16,
    Int64 = 17,
    Json = 35,
    Lookup = 18,
    MultiChoice = 19,
    MultiLookup = 20,
    MultiUser = 21,
    Object = 22,
    RichText = 23,
    SByte = 24,
    Single = 25,
    String = 26,
    Time = 27,
    Timestamp = 28,
    UInt16 = 29,
    UInt32 = 30,
    UInt64 = 31,
    Url = 32,
    User = 33,
    Xml = 34,
    //AutoNumber = 35 (Not a good idea, because cannot insert data into Identity field. User can make Identity after insert data.)
}