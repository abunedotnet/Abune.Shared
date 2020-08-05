//-----------------------------------------------------------------------
// <copyright file="Locator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable CA1716

namespace Abune.Shared.Util
{
    using System;
    using System.Collections.Generic;

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
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="z">The z position.</param>
        /// <returns>The area identifier.</returns>
        public static ulong GetAreaIdFromWorldPosition(float x, float y, float z)
        {
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
        /// <param name="minX">The minimum x position.</param>
        /// <param name="minY">The minimum y position.</param>
        /// <param name="minZ">The minimum z position.</param>
        /// <param name="maxX">The maximum x position.</param>
        /// <param name="maxY">The maximum y position.</param>
        /// <param name="maxZ">The maximum z position.</param>
        /// <returns>List of area identifiers.</returns>
        public static List<ulong> GetAreaIdsWithinWorldBoundaries(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            List<ulong> areas = new List<ulong>();
            ulong minArea = GetAreaIdFromWorldPosition(minX, minY, minZ);
            ulong maxArea = GetAreaIdFromWorldPosition(maxX, maxY, maxZ);
            ulong minXPart, minYPart, minZPart, maxXPart, maxYPart, maxZPart;
            GetPartsFromAreaId(minArea, out minXPart, out minYPart, out minZPart);
            GetPartsFromAreaId(maxArea, out maxXPart, out maxYPart, out maxZPart);
            for (ulong xPart = minXPart; xPart <= maxXPart; xPart++)
            {
                for (ulong yPart = minYPart; yPart <= maxYPart; yPart++)
                {
                    for (ulong zPart = minZPart; zPart <= maxZPart; zPart++)
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
            ulong areaId = (ulong)((z + MAXPOSITION) / AREASIZE)
                           + (ulong)((y + MAXPOSITION) / AREASIZE) * (ulong)MAXPOSITION
                           + (ulong)((x + MAXPOSITION) / AREASIZE) * ((ulong)MAXPOSITION * (ulong)MAXPOSITION);
            return areaId;
        }
    }
}