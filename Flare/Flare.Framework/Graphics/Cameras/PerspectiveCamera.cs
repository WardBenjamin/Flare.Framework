using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare.Framework.Graphics.Cameras
{
    /// <summary>
    /// Perspective camera. See Camera base class for more information.
    /// </summary>
    public class PerspectiveCamera : Camera
    {
        internal PerspectiveCamera(Matrix4 projectionMatrix)
        {
            ProjectionMatrix = projectionMatrix;
        }
    }
}
