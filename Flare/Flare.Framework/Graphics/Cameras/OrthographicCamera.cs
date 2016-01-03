using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare.Framework.Graphics.Cameras
{
    /// <summary>
    /// Orthographic camera. See Camera base class for more information.
    /// </summary>
    public class OrthographicCamera : Camera
    {
        internal const OrthographicCamera Default = Camera.CreateOrthographic();

        internal OrthographicCamera(Matrix4 projectionMatrix)
        {
            ProjectionMatrix = projectionMatrix;
        }
    }
}
