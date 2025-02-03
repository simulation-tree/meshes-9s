using System.Numerics;
using Worlds;

namespace Meshes.NineSliced.Components
{
    [Component]
    public struct Mesh9SliceSettings
    {
        public Vector4 geometryMargins;
        public Vector4 uvMargins;
        public uint version;

        public Mesh9SliceSettings(Vector4 geometryMargins, Vector4 uvMargins, uint version = default)
        {
            this.geometryMargins = geometryMargins;
            this.uvMargins = uvMargins;
            this.version = version;
        }
    }
}