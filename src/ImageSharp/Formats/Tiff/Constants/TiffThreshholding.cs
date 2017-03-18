// <copyright file="TiffThreshholding.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Formats
{
    /// <summary>
    /// Enumeration representing the threshholding applied to image data defined by the Tiff file-format.
    /// </summary>
    internal enum TiffThreshholding
    {
        /// <summary>
        /// No dithering or halftoning.
        /// </summary>
        None = 1,

        /// <summary>
        ///  An ordered dither or halftone technique.
        /// </summary>
        Ordered = 2,

        /// <summary>
        /// A randomized process such as error diffusion.
        /// </summary>
        Random = 3
    }
}