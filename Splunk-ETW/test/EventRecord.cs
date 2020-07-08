using Microsoft.O365.Security.ETW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using SplunkETW;

namespace SplunkETWTest
{
    public class TestEventRecord : IEventRecord
    {
        public Dictionary<string, object> Fields { get; set; } 
        public IEnumerable<Property> Properties { get; set; }

        public string ProviderName { get; set; }

        public string Name { get; set; }

        public IntPtr UserData { get; set; }

        public ushort UserDataLength { get; set; }

        public Guid ActivityId { get; set; }

        public Guid ProviderId { get; set; }

        public DateTime Timestamp { get; set; }

        public uint ThreadId { get; set; }

        public uint ProcessId { get; set; }

        public EventHeaderProperty EventProperty { get; set; }

        public ushort Flags { get; set; }

        public byte Level { get; set; }

        public byte Version { get; set; }

        public byte Opcode { get; set; }

        public ushort Id { get; set; }

        public byte[] CopyUserData()
        {
            throw new NotImplementedException();
        }

        public string GetAnsiString(string name, string defaultValue)
        {
            return (string)this.Fields[name];
        }

        public string GetAnsiString(string name)
        {
            return (string)this.Fields[name];
        }

        public byte[] GetBinary(string name)
        {
            return (byte [])this.Fields[name];
        }

        public string GetCountedString(string name, string defaultValue)
        {
            return (string)this.Fields[name];
        }

        public string GetCountedString(string name)
        {
            return (string)this.Fields[name];
        }

        public ValueType GetDateTime(string name, ValueType defaultValue)
        {
            return (ValueType)this.Fields[name];
        }

        public ValueType GetDateTime(string name)
        {
            return (ValueType)this.Fields[name];
        }

        public short GetInt16(string name, short defaultValue)
        {
            return (short)this.Fields[name];
        }

        public short GetInt16(string name)
        {
            return (short)this.Fields[name];
        }

        public int GetInt32(string name, int defaultValue)
        {
            return (int)this.Fields[name];
        }

        public int GetInt32(string name)
        {
            return (int)this.Fields[name];
        }

        public long GetInt64(string name, long defaultValue)
        {
            return (long)this.Fields[name];
        }

        public long GetInt64(string name)
        {
            return (long)this.Fields[name];
        }

        public sbyte GetInt8(string name, sbyte defaultValue)
        {
            return (sbyte)this.Fields[name];
        }

        public sbyte GetInt8(string name)
        {
            return (sbyte)this.Fields[name];
        }

        public IPAddress GetIPAddress(string name, IPAddress defaultValue)
        {
            return (IPAddress)this.Fields[name];
        }

        public IPAddress GetIPAddress(string name)
        {
            return(IPAddress)this.Fields[name];
        }

        public SocketAddress GetSocketAddress(string name, SocketAddress defaultValue)
        {
            return (SocketAddress)this.Fields[name];
        }

        public SocketAddress GetSocketAddress(string name)
        {
            return (SocketAddress)this.Fields[name];
        }

        public ushort GetUInt16(string name, ushort defaultValue)
        {
            return (ushort)this.Fields[name];
        }

        public ushort GetUInt16(string name)
        {
            return (ushort)this.Fields[name];
        }

        public uint GetUInt32(string name, uint defaultValue)
        {
            return (uint)this.Fields[name];
        }

        public uint GetUInt32(string name)
        {
            return (uint)this.Fields[name];
        }

        public ulong GetUInt64(string name, ulong defaultValue)
        {
            return (ulong)this.Fields[name];
        }

        public ulong GetUInt64(string name)
        {
            return (ulong)this.Fields[name];
        }

        public byte GetUInt8(string name, byte defaultValue)
        {
            return (byte)this.Fields[name];
        }

        public byte GetUInt8(string name)
        {
            return (byte)this.Fields[name];
        }

        public string GetUnicodeString(string name, string defaultValue)
        {
            return (string)this.Fields[name];
        }

        public string GetUnicodeString(string name)
        {
            return (string)this.Fields[name];
        }

        public bool TryGetAnsiString(string name, out string result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetBinary(string name, out byte[] result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetContainerId(out Guid result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCountedString(string name, out string result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetDateTime(string name, out ValueType result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetInt16(string name, out short result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetInt32(string name, out int result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetInt64(string name, out long result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetInt8(string name, out sbyte result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetIPAddress(string name, out IPAddress result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSocketAddress(string name, out SocketAddress result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetUInt16(string name, out ushort result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetUInt32(string name, out uint result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetUInt64(string name, out ulong result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetUInt8(string name, out byte result)
        {
            throw new NotImplementedException();
        }

        public bool TryGetUnicodeString(string name, out string result)
        {
            throw new NotImplementedException();
        }
    }
}