//-----------------------------------------------------------------------
// <copyright file="AVector3.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716

namespace Abune.Shared.DataType
{
    using System;

    /// <summary>
    /// Struct representing a three dimensional vector.
    /// </summary>
    public struct AVector3 : IEquatable<AVector3>
    {
        /// <summary>
        /// Gets zero initialized vector.
        /// </summary>
        public static AVector3 Zero
        {
            get
            {
                return new AVector3()
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = 0.0f,
                };
            }
        }

        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the Z component of the vector.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Compares two vectors.
        /// </summary>
        /// <param name="left">Left vector.</param>
        /// <param name="right">Right vector.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public static bool operator ==(AVector3 left, AVector3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two vectors.
        /// </summary>
        /// <param name="left">Left vector.</param>
        /// <param name="right">Right vector.</param>
        /// <returns>false if the specified object is equal to the current object; otherwise, true.</returns>
        public static bool operator !=(AVector3 left, AVector3 right)
        {
            return !(left == right);
        }

        /// <summary>
        ///  Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"(V) X:{this.X}, Y:{this.Y}, Z:{this.Z}";
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(AVector3 other)
        {
            return
                this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z;
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is AVector3))
            {
                return false;
            }

            return this.Equals((AVector3)obj);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return
                this.X.GetHashCode() ^
                this.Y.GetHashCode() ^
                this.Z.GetHashCode();
        }
    }
}
