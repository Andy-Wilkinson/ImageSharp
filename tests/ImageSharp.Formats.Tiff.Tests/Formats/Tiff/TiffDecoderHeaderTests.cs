// <copyright file="TiffDecoderHeaderTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;
    using Xunit;

    using ImageSharp.Formats;

    public class TiffDecoderHeaderTests
    {
        public static object[][] IsLittleEndianValues = new[] { new object[] { false },
                                                                new object[] { true } };

        [Theory]
        [MemberData(nameof(IsLittleEndianValues))]
        public void ReadHeader_ReadsEndianness(bool isLittleEndian)
        {
            Stream stream = new TiffGenHeader()
                            {
                                FirstIfd = new TiffGenIfd()
                            }
                            .ToStream(isLittleEndian);

            TiffDecoderCore decoder = new TiffDecoderCore(stream, false, null);

            decoder.ReadHeader();

            Assert.Equal(isLittleEndian, decoder.IsLittleEndian);
        }

        [Theory]
        [MemberData(nameof(IsLittleEndianValues))]
        public void ReadHeader_ReadsFirstIfdOffset(bool isLittleEndian)
        {
            Stream stream = new TiffGenHeader()
                            {
                                FirstIfd = new TiffGenIfd()
                            }
                            .ToStream(isLittleEndian);

            TiffDecoderCore decoder = new TiffDecoderCore(stream, false, null);

            uint firstIfdOffset = decoder.ReadHeader();

            Assert.Equal(8u, firstIfdOffset);
        }

        [Theory]
        [InlineData(0x1234)]
        [InlineData(0x4912)]
        [InlineData(0x1249)]
        [InlineData(0x4D12)]
        [InlineData(0x124D)]
        [InlineData(0x494D)]
        [InlineData(0x4D49)]
        public void Decode_ThrowsException_WithInvalidByteOrderMarkers(ushort byteOrderMarker)
        {
            Stream stream = new TiffGenHeader()
                            {
                                FirstIfd = new TiffGenIfd(),
                                ByteOrderMarker = byteOrderMarker
                            }
                            .ToStream(true);

            TiffDecoder decoder = new TiffDecoder();
            
            ImageFormatException e = Assert.Throws<ImageFormatException>(() => { TestDecode(decoder, stream); });
            
            Assert.Equal("Invalid TIFF file header.", e.Message);
        }

        [Theory]
        [MemberData(nameof(IsLittleEndianValues))]
        public void Decode_ThrowsException_WithIncorrectMagicNumber(bool isLittleEndian)
        {
            Stream stream = new TiffGenHeader()
                            {
                                FirstIfd = new TiffGenIfd(),
                                MagicNumber = 32
                            }
                            .ToStream(isLittleEndian);

            TiffDecoder decoder = new TiffDecoder();
            
            ImageFormatException e = Assert.Throws<ImageFormatException>(() => { TestDecode(decoder, stream); });
            
            Assert.Equal("Invalid TIFF file header.", e.Message);
        }

        [Theory]
        [MemberData(nameof(IsLittleEndianValues))]
        public void Decode_ThrowsException_WithNoIfdZero(bool isLittleEndian)
        {
            Stream stream = new TiffGenHeader()
                            {
                                FirstIfd = null
                            }
                            .ToStream(isLittleEndian);

            TiffDecoder decoder = new TiffDecoder();
            
            ImageFormatException e = Assert.Throws<ImageFormatException>(() => { TestDecode(decoder, stream); });
            
            Assert.Equal("Invalid TIFF file header.", e.Message);
        }

        private void TestDecode(TiffDecoder decoder, Stream stream)
        {
            Configuration.Default.AddImageFormat(new TiffFormat());
            Image image = new Image(1,1);
            decoder.Decode<Color>(image, stream, null);
        }
    }
}