using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KG2.Figures
{
    abstract class Figure
    {
        public Vector3 Color { get; set; }
        public Material Material { get; set; }

        public abstract Vector4 RayTrace(Vector3 RayOrigin, Vector3 RayDirection);

        public abstract void RotateX(double angle);
        public abstract void RotateY(double angle);
        public abstract void RotateZ(double angle);

        public abstract void Transform(Matrix3x3 trans);
    }
}
