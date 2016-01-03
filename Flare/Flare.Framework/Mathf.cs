using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare.Framework
{
    /// <summary>
    /// Provides extra math capability over System.Math
    /// </summary>
    public static class Mathf
    {
        /// <summary>
        /// Converts degrees to radians. (Double overload)
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <returns>Radians equivalent to the degree value</returns>
        public static double DegreesToRadians(double degrees)
        {
            return Math.PI / 180 * degrees;
        }

        /// <summary>
        /// Converts degrees to radians. (Float overload)
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <returns>Radians equivalent to the degree value</returns>
        public static float DegreesToRadians(float degrees)
        {
            return (float)(DegreesToRadians((double)degrees));
        }
    }
}
