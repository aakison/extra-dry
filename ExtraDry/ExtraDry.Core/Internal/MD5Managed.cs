//Copyright (c) Microsoft Corporation.  All rights reserved.
// **************************************************************
// * Raw implementation of the MD5 hash algorithm
// * from RFC 1321.
// *
// * Written By: Reid Borsuk and Jenny Zheng
// * Copyright (c) Microsoft Corporation.  All rights reserved.
// **************************************************************

using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ExtraDry.Core.Internal;

/// <summary>
/// An implementation of an MD5 Hash. This has been removed from .NET (browser profile). This is
/// only used for non-secure purposes, in particular, the generation of gravatars. As such, this is
/// not exposed publicly.
/// </summary>
internal class MD5 : HashAlgorithm
{
    private readonly byte[] _data;

    private ABCDStruct _abcd;

    private Int64 _totalLength;

    private int _dataSize;

    public MD5()
    {
        HashSizeValue = 0x80;
        _data = new byte[64];
        Initialize();
    }

    public override void Initialize()
    {
        _dataSize = 0;
        _totalLength = 0;
        //Intitial values as defined in RFC 1321
        _abcd = new ABCDStruct {
            A = 0x67452301,
            B = 0xefcdab89,
            C = 0x98badcfe,
            D = 0x10325476
        };
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        var startIndex = ibStart;
        var totalArrayLength = _dataSize + cbSize;
        if(totalArrayLength >= 64) {
            Array.Copy(array, startIndex, _data, _dataSize, 64 - _dataSize);
            // Process message of 64 bytes (512 bits)
            MD5Core.GetHashBlock(_data, ref _abcd, 0);
            startIndex += 64 - _dataSize;
            totalArrayLength -= 64;
            while(totalArrayLength >= 64) {
                Array.Copy(array, startIndex, _data, 0, 64);
                MD5Core.GetHashBlock(array, ref _abcd, startIndex);
                totalArrayLength -= 64;
                startIndex += 64;
            }
            _dataSize = totalArrayLength;
            Array.Copy(array, startIndex, _data, 0, totalArrayLength);
        }
        else {
            Array.Copy(array, startIndex, _data, _dataSize, cbSize);
            _dataSize = totalArrayLength;
        }
        _totalLength += cbSize;
    }

    protected override byte[] HashFinal()
    {
        HashValue = MD5Core.GetHashFinalBlock(_data, 0, _dataSize, _abcd, _totalLength * 8);
        return HashValue;
    }

    /// <summary>
    /// Convenience method that creates a MD5 hash of a string and returns a string. Encapsulates
    /// the `ComputeHash` method and simply provide byte to string translations.
    /// </summary>
    public string ComputeStringHash(string input)
    {
        var encoder = new UTF8Encoding();
        var hashedBytes = ComputeHash(encoder.GetBytes(input));
        var hash = string.Join("", hashedBytes.Select(e => e.ToString("X2", CultureInfo.InvariantCulture)));
        return hash;
    }
}
