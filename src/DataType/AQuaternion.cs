//-----------------------------------------------------------------------
// <copyright file="AQuaternion.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1716

namespace Abune.Shared.DataType
{
    using System;

    /// <summary>
    /// Class representing an orientation.
    /// </summary>
    public struct AQuaternion : IEquatable<AQuaternion>
    {
        /// <summary>
        /// Gets zero initialized quaternion.
        /// </summary>
        public static AQuaternion Zero
        {
            get
            {
                return new AQuaternion()
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = 0.0f,
                    W = 0.0f,
                };
            }
        }

        /// <summary>
        /// Gets or sets the X component of the Quaternion.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y component of the Quaternion.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the Z component of the Quaternion.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Gets or sets the W component of the Quaternion.
        /// </summary>
        public float W { get; set; }

        /// <summary>
        /// Compares two quaternions.
        /// </summary>
        /// <param name="left">Left quaternion.</param>
        /// <param name="right">Right quaternion.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public static bool operator ==(AQuaternion left, AQuaternion right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two quaternions.
        /// </summary>
        /// <param name="left">Left quaternion.</param>
        /// <param name="right">Right quaternion.</param>
        /// <returns>false if the specified object is equal to the current object; otherwise, true.</returns>
        public static bool operator !=(AQuaternion left, AQuaternion right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(AQuaternion other)
        {
            return
                this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z &&
                this.W == other.W;
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is AQuaternion))
            {
                return false;
            }

            return this.Equals((AQuaternion)obj);
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
                this.Z.GetHashCode() ^
                this.W.GetHashCode();
        }

        /// <summary>
        ///  Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"(Q) X:{this.X}, Y:{this.Y}, Z:{this.Z}, W:{this.W}";
        }
    }
}
