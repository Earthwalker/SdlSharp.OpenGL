//-----------------------------------------------------------------------
// <copyright file="Utility.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace SdlSharp.OpenGL
{
    /// <summary>
    /// Utility
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Creates a vector of the specified x and y values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        /// The new vector.
        /// </returns>
        internal static double[] Vector(params double[] values)
        {
            return values;
        }

        /// <summary>
        /// Creates a vector of the specified x and y values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        /// The new vector.
        /// </returns>
        internal static int[] VectorInt(params int[] values)
        {
            return values;
        }

        /// <summary>
        /// Returns the X component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The X component.</returns>
        internal static double X(this double[] vector)
        {
            if (vector?.Length > 0)
                return vector[0];

            return 0;
        }

        /// <summary>
        /// Returns the X component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The X component.</returns>
        internal static int X(this int[] vector)
        {
            if (vector?.Length > 0)
                return vector[0];

            return 0;
        }

        /// <summary>
        /// Returns the X component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The X component.</returns>
        internal static bool X(this bool[] vector)
        {
            if (vector?.Length > 0)
                return vector[0];

            return false;
        }

        /// <summary>
        /// Returns the Y component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Y component.</returns>
        internal static double Y(this double[] vector)
        {
            if (vector?.Length > 1)
                return vector[1];

            return 0;
        }

        /// <summary>
        /// Returns the Y component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Y component.</returns>
        internal static int Y(this int[] vector)
        {
            if (vector?.Length > 1)
                return vector[1];

            return 0;
        }

        /// <summary>
        /// Returns the Y component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Y component.</returns>
        internal static bool Y(this bool[] vector)
        {
            if (vector?.Length > 1)
                return vector[1];

            return false;
        }

        /// <summary>
        /// Returns the Z component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Z component.</returns>
        internal static double Z(this double[] vector)
        {
            if (vector?.Length > 2)
                return vector[2];

            return 0;
        }

        /// <summary>
        /// Returns the Z component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Z component.</returns>
        internal static int Z(this int[] vector)
        {
            if (vector?.Length > 2)
                return vector[2];

            return 0;
        }

        /// <summary>
        /// Returns the Z component of the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Z component.</returns>
        internal static bool Z(this bool[] vector)
        {
            if (vector?.Length > 2)
                return vector[2];

            return false;
        }

        /// <summary>
        /// Gets the distance between two points.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <returns>The distance.</returns>
        internal static double Distance(this int[] source, int[] other)
        {
            // d = √ (x2-x1)^2 + (y2-y1)^2 + (z2-z1)^2
            return Math.Sqrt(Math.Pow(Convert.ToDouble(other.X()) - Convert.ToDouble(source.X()), 2) +
                             Math.Pow(Convert.ToDouble(other.Y()) - Convert.ToDouble(source.Y()), 2) +
                             Math.Pow(Convert.ToDouble(other.Z()) - Convert.ToDouble(source.Z()), 2));
        }

        /// <summary>
        /// Returns the red component of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The red component.</returns>
        internal static byte Red(this byte[] color)
        {
            if (color?.Length > 0)
                return color[0];

            return 0;
        }

        /// <summary>
        /// Returns the green component of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The green component.</returns>
        internal static byte Green(this byte[] color)
        {
            if (color?.Length > 1)
                return color[1];

            return 0;
        }

        /// <summary>
        /// Returns the blue component of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The blue component.</returns>
        internal static byte Blue(this byte[] color)
        {
            if (color?.Length > 2)
                return color[2];

            return 0;
        }

        /// <summary>
        /// Returns the alpha component of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The alpha component.</returns>
        internal static byte Alpha(this byte[] color)
        {
            if (color?.Length > 3)
                return color[3];

            return 0;
        }
    }
}
