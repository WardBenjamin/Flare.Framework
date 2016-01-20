#region License
/* Flare - A Framework by Developers, for Developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare
{
    public class GameTime
    {
        #region Public Properties

        public TimeSpan TotalGameTime { get; internal set; }

        public TimeSpan ElapsedGameTime { get; internal set; }

        public bool IsRunningSlowly { get; internal set; }

        #endregion

        #region Public Constructors

        public GameTime() : this(TimeSpan.Zero, TimeSpan.Zero, false)
        {
        }

        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
            : this(totalGameTime, elapsedGameTime, false)
        {
        }

        public GameTime(TimeSpan totalRealTime, TimeSpan elapsedRealTime, bool isRunningSlowly)
        {
            TotalGameTime = totalRealTime;
            ElapsedGameTime = elapsedRealTime;
            IsRunningSlowly = isRunningSlowly;
        }

        #endregion
    }
}
