using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KG2.Figures
{
    class Cone : Figure
    {
        public Vector3 Peak { get; set; }
        public Vector3 BaseCenter { get; set; }
        public double BaseRadius { get; set; }

        public Cone(Vector3 color, Vector3 peak, Vector3 center, double radius)
        {
            Color = color;
            Peak = peak;
            BaseCenter = center;
            BaseRadius = radius;
        }

        public override Vector4 RayTrace(Vector3 rayOrigin, Vector3 rayDirection)
        {
            Vector3 axis = BaseCenter - Peak;
            Vector3 originToPeak = rayOrigin - Peak;
            Vector3 originToBase = rayOrigin - BaseCenter;
            double axisLength2 = VectorAlgebra.Dot(axis, axis);
            double axisOriginToPeakAlignment = VectorAlgebra.Dot(originToPeak, axis);
            double axisDirectionAlignment = VectorAlgebra.Dot(rayDirection, axis);
            double directionOriginToPeakAlignment = VectorAlgebra.Dot(rayDirection, originToPeak);
            double originToPeakLength2 = VectorAlgebra.Dot(originToPeak, originToPeak);
            double baseVisibility = VectorAlgebra.Dot(originToBase, axis);

            double t;

            if (axisDirectionAlignment < 0.0)
            {
                t = -baseVisibility / axisDirectionAlignment;
                if (VectorAlgebra.Dot2(originToBase + rayDirection * t) < (BaseRadius * BaseRadius))
                    return VectorAlgebra.Vec4(t, 1.0 / Math.Sqrt(axisLength2) * axis);
            }

            double hypothenuse = axisLength2 + BaseRadius * BaseRadius;
            double k2 = axisLength2 * axisLength2 - axisDirectionAlignment * axisDirectionAlignment * hypothenuse;
            double k1 = axisLength2 * axisLength2 * directionOriginToPeakAlignment - axisOriginToPeakAlignment * axisDirectionAlignment * hypothenuse;
            double k0 = axisLength2 * axisLength2 * originToPeakLength2 - axisOriginToPeakAlignment * axisOriginToPeakAlignment * hypothenuse;
            double h = k1 * k1 - k2 * k0;
            if (h <= 0.0) return VectorAlgebra.Vec4(-1.0);
            t = (-k1 - Math.Sqrt(h)) / k2;
            double y = axisOriginToPeakAlignment + t * axisDirectionAlignment;
            if (y < 0.0 || y > axisLength2) return VectorAlgebra.Vec4(-1.0);
            return VectorAlgebra.Vec4(t, VectorAlgebra.Normalize(axisLength2 * (axisLength2 * (originToPeak + t * rayDirection)) - axis * hypothenuse * y));
        }

        public override void RotateX(double angle)
        {
            Matrix3x3 tmp = Matrix3x3.RotateX(angle);
            Peak *= tmp;
            BaseCenter *= tmp;
        }

        public override void RotateY(double angle)
        {
            Matrix3x3 tmp = Matrix3x3.RotateY(angle);
            Peak *= tmp;
            BaseCenter *= tmp;
        }

        public override void RotateZ(double angle)
        {
            Matrix3x3 tmp = Matrix3x3.RotateZ(angle);
            Peak *= tmp;
            BaseCenter *= tmp;
        }

        public override void Transform(Matrix3x3 trans)
        {
            Peak *= trans;
            BaseCenter *= trans;
        }
    }
}
