//-----------------------------------------------------------------------
// <copyright file="Locator.cs" company="Thomas Stollenwerk (motmot80)">
// Copyright (c) Thomas Stollenwerk (motmot80). All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CA1716

namespace Abune.Shared.Util
{
    using System;
    using System.Collections.Generic;
    using Abune.Shared.DataType;

    /// <summary>Implementation to locate areas and objects.</summary>
    public static class Locator
    {
        /// <summary>  The dimension (size) of one area.</summary>
        public const float AREASIZE = 200.0f;

        /// <summary>The world size.</summary>
        public const float MAXAREASIZE = 20000000.0f;

        /// <summary>  The absolute maximum position an object can have.</summary>
        public const float MAXPOSITION = MAXAREASIZE / 2.0f;

        /// <summary>Gets the area identifier from world position.</summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The area identifier.</returns>
        public static ulong GetAreaIdFromWorldPosition(AVector3 worldPosition)
        {
            float x = worldPosition.X;
            float y = worldPosition.Y;
            float z = worldPosition.Z;

            if (Math.Abs(x) > MAXPOSITION)
            {
                x = x > 0 ? MAXPOSITION : -MAXPOSITION;
            }

            if (Math.Abs(y) > MAXPOSITION)
            {
                y = y > 0 ? MAXPOSITION : -MAXPOSITION;
            }

            if (Math.Abs(z) > MAXPOSITION)
            {
                z = z > 0 ? MAXPOSITION : -MAXPOSITION;
            }

            return GetAreaId(x, y, z);
        }

        /// <summary>Gets all area identifiers within the given world boundaries.</summary>
        /// <param name="a">The world position start.</param>
        /// <param name="b">The world position end.</param>
        /// <returns>List of area identifiers.</returns>
        public static List<ulong> GetAreaIdsWithinWorldBoundaries(AVector3 a, AVector3 b)
        {
            List<ulong> areas = new List<ulong>();
            ulong aArea = GetAreaIdFromWorldPosition(a);
            ulong bArea = GetAreaIdFromWorldPosition(b);
            ulong aXPart, aYPart, aZPart, bXPart, bYPart, bZPart;
            GetPartsFromAreaId(aArea, out aXPart, out aYPart, out aZPart);
            GetPartsFromAreaId(bArea, out bXPart, out bYPart, out bZPart);
            for (ulong xPart = Math.Min(aXPart, bXPart); xPart <= Math.Max(aXPart, bXPart); xPart++)
            {
                for (ulong yPart = Math.Min(aYPart, bYPart); yPart <= Math.Max(aYPart, bYPart); yPart++)
                {
                    for (ulong zPart = Math.Min(aZPart, bZPart); zPart <= Math.Max(aZPart, bZPart); zPart++)
                    {
                        ulong areaId = zPart + (yPart * (ulong)MAXPOSITION) + (xPart * (ulong)MAXPOSITION * (ulong)MAXPOSITION);
                        areas.Add(areaId);
                    }
                }
            }

            return areas;
        }

        /// <summary>Gets the boundary of the area.</summary>
        /// <param name="areaId">The area identifier.</param>
        /// <param name="minX">The minimum x boundary.</param>
        /// <param name="minY">The minimum y boundary.</param>
        /// <param name="minZ">The minimum z boundary.</param>
        /// <param name="maxX">The maximum x boundary.</param>
        /// <param name="maxY">The maximum y boundary.</param>
        /// <param name="maxZ">The maximum z boundary.</param>
        public static void GetAreaBoundary(ulong areaId, out float minX, out float minY, out float minZ, out float maxX, out float maxY, out float maxZ)
        {
            ulong maxPosition = (ulong)(MAXAREASIZE / 2.0f);
            ulong xPart, yPart, zPart;
            GetPartsFromAreaId(areaId, out xPart, out yPart, out zPart);
            float areaX = xPart * AREASIZE;
            float areaY = yPart * AREASIZE;
            float areaZ = zPart * AREASIZE;
            minX = areaX - maxPosition;
            minY = areaY - maxPosition;
            minZ = areaZ - maxPosition;
            maxX = minX + AREASIZE;
            maxY = minY + AREASIZE;
            maxZ = minZ + AREASIZE;
        }

        private static void GetPartsFromAreaId(ulong areaId, out ulong xPart, out ulong yPart, out ulong zPart)
        {
            xPart = (ulong)(areaId / (ulong)MAXPOSITION / (ulong)MAXPOSITION);
            yPart = (ulong)(areaId / (ulong)MAXPOSITION) - (xPart * (ulong)MAXPOSITION);
            zPart = (ulong)areaId - (yPart * (ulong)MAXPOSITION) - (xPart * (ulong)MAXPOSITION * (ulong)MAXPOSITION);
        }

        private static ulong GetAreaId(float x, float y, float z)
        {
            ulong zPart = (ulong)((z + MAXPOSITION) / AREASIZE);
            ulong yPart = (ulong)((y + MAXPOSITION) / AREASIZE) * (ulong)MAXPOSITION;
            ulong xPart = (ulong)((x + MAXPOSITION) / AREASIZE) * ((ulong)MAXPOSITION * (ulong)MAXPOSITION);
            ulong areaId = zPart + yPart + xPart;
            return areaId;
        }
    }
}