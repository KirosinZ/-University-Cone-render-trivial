using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KG2
{
    static class VectorAlgebra
    {
        public static Vector2 Vec2(double val) => new Vector2(val, val);
        public static Vector2 Vec2(double x, double y) => new Vector2(x, y);

        public static Vector3 Vec3(double val) => new Vector3(val, val, val);
        public static Vector3 Vec3(double val, Vector2 v) => new Vector3(val, v.X, v.Y);
        public static Vector3 Vec3(Vector2 v, double val = 0) => new Vector3(v.X, v.Y, val);
        public static Vector3 Vec3(double x, double y, double z) => new Vector3(x, y, z);

        public static Vector4 Vec4(double val) => new Vector4(val, val, val, val);
        public static Vector4 Vec4(double val, Vector3 v) => new Vector4(val, v.X, v.Y, v.Z);
        public static Vector4 Vec4(Vector3 v, double val = 0) => new Vector4(v.X, v.Y, v.Z, val);
        public static Vector4 Vec4(Vector2 v1, Vector2 v2) => new Vector4(v1.X, v1.Y, v2.X, v2.Y);
        public static Vector4 Vec4(Vector2 v) => new Vector4(v.X, v.Y, 0, 0);
        public static Vector4 Vec4(double x, double y, double z, double w) => new Vector4(x, y, z, w);

        public static double Dot(Vector2 l, Vector2 r) => l.X * r.X + l.Y * r.Y;
        public static double Dot(Vector3 l, Vector3 r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z;
        public static double Dot(Vector4 l, Vector4 r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z + l.W * r.W;

        public static double Dot2(Vector2 v) => Dot(v, v);
        public static double Dot2(Vector3 v) => Dot(v, v);
        public static double Dot2(Vector4 v) => Dot(v, v);

        public static Vector3 Cross(Vector3 l, Vector3 r) => new Vector3(l.Y * r.Z - l.Z * r.Y, l.Z * r.X - l.X * r.Z, l.X * r.Y - l.Y * r.X);

        public static double Clamp(double x, double minval, double maxval) => Math.Min(Math.Max(x, minval), maxval);
        public static Vector2 Clamp(Vector2 v, double minval, double maxval) => Vec2(Clamp(v.X, minval, maxval), Clamp(v.Y, minval, maxval));
        public static Vector2 Clamp(Vector2 v, Vector2 minvals, Vector2 maxvals) => Vec2(Clamp(v.X, minvals.X, maxvals.X), Clamp(v.Y, minvals.Y, maxvals.Y));

        public static Vector3 Clamp(Vector3 v, double minval, double maxval) => Vec3(Clamp(v.X, minval, maxval), Clamp(v.Y, minval, maxval), Clamp(v.Z, minval, maxval));
        public static Vector3 Clamp(Vector3 v, Vector3 minvals, Vector3 maxvals) => Vec3(Clamp(v.X, minvals.X, maxvals.X), Clamp(v.Y, minvals.Y, maxvals.Y), Clamp(v.Z, minvals.Z, maxvals.Z));

        public static Vector4 Clamp(Vector4 v, double minval, double maxval) => Vec4(Clamp(v.X, minval, maxval), Clamp(v.Y, minval, maxval), Clamp(v.Z, minval, maxval), Clamp(v.W, minval, maxval));
        public static Vector4 Clamp(Vector4 v, Vector4 minvals, Vector4 maxvals) => Vec4(Clamp(v.X, minvals.X, maxvals.X), Clamp(v.Y, minvals.Y, maxvals.Y), Clamp(v.Z, minvals.Z, maxvals.Z), Clamp(v.W, minvals.W, maxvals.W));

        public static double Distance(Vector2 l, Vector2 r) => (r - l).Length;
        public static double Distance(Vector3 l, Vector3 r) => (r - l).Length;
        public static double Distance(Vector4 l, Vector4 r) => (r - l).Length;

        public static Vector2 Normalize(Vector2 v) => v.InvLength * v;
        public static Vector3 Normalize(Vector3 v) => v.InvLength * v;
        public static Vector4 Normalize(Vector4 v) => v.InvLength * v;

        public static Vector2 Reflect(Vector2 i, Vector2 n) => i - 2 * Dot(n, i) * n;
        public static Vector3 Reflect(Vector3 i, Vector3 n) => i - 2 * Dot(n, i) * n;
        public static Vector4 Reflect(Vector4 i, Vector4 n) => i - 2 * Dot(n, i) * n;

        public static Vector2 Refract(Vector2 i, Vector2 n, double eta)
        {
            double d = Dot(n, i);
            double k = 1 - eta * eta * (1 - d * d);
            if (k < 0) return Vec2(0);
            return eta * i - ((eta * d + Math.Sqrt(k)) * n);
        }
        public static Vector3 Refract(Vector3 i, Vector3 n, double eta)
        {
            double d = Dot(n, i);
            double k = 1 - eta * eta * (1 - d * d);
            if (k < 0) return Vec3(0);
            return eta * i - ((eta * d + Math.Sqrt(k)) * n);
        }
        public static Vector4 Refract(Vector4 i, Vector4 n, double eta)
        {
            double d = Dot(n, i);
            double k = 1 - eta * eta * (1 - d * d);
            if (k < 0) return Vec4(0);
            return eta * i - ((eta * d + Math.Sqrt(k)) * n);
        }

        public static double Sqrt(double a) => Math.Sqrt(a);
        public static double InverseSqrt(double a) => 1 / Sqrt(a);
        public static Vector2 Sqrt(Vector2 v) => Vec2(Sqrt(v.X), Sqrt(v.Y));
        public static Vector3 Sqrt(Vector3 v) => Vec3(Sqrt(v.X), Sqrt(v.Y), Sqrt(v.Z));
        public static Vector4 Sqrt(Vector4 v) => Vec4(Sqrt(v.X), Sqrt(v.Y), Sqrt(v.Z), Sqrt(v.W));
    }

    public struct Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double Length => VectorAlgebra.Sqrt(VectorAlgebra.Dot2(this));
        public double Length2 => VectorAlgebra.Dot2(this);
        public double InvLength => VectorAlgebra.InverseSqrt(VectorAlgebra.Dot2(this));

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 l, Vector2 r) => new Vector2(l.X + r.X, l.Y + r.Y);
        public static Vector2 operator -(Vector2 l, Vector2 r) => new Vector2(l.X - r.X, l.Y - r.Y);
        public static Vector2 operator -(Vector2 v) => new Vector2(-v.X, -v.Y);
        public static Vector2 operator *(Vector2 l, Vector2 r) => new Vector2(l.X * r.X, l.Y * r.Y);
        public static Vector2 operator /(Vector2 l, Vector2 r) => new Vector2(l.X / r.X, l.Y / r.Y);

        public static Vector2 operator +(double a, Vector2 v) => new Vector2(a + v.X, a + v.Y);
        public static Vector2 operator +(Vector2 v, double a) => new Vector2(a + v.X, a + v.Y);
        public static Vector2 operator -(double a, Vector2 v) => new Vector2(a - v.X, a - v.Y);
        public static Vector2 operator -(Vector2 v, double a) => new Vector2(-a + v.X, -a + v.Y);
        public static Vector2 operator *(double a, Vector2 v) => new Vector2(a * v.X, a * v.Y);
        public static Vector2 operator *(Vector2 v, double a) => new Vector2(a * v.X, a * v.Y);
        public static Vector2 operator /(Vector2 v, double a) => new Vector2(v.X / a, v.Y / a);
    }

    public struct Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Length => VectorAlgebra.Sqrt(VectorAlgebra.Dot2(this));
        public double Length2 => VectorAlgebra.Dot2(this);
        public double InvLength => VectorAlgebra.InverseSqrt(VectorAlgebra.Dot2(this));

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator +(Vector3 l, Vector3 r) => new Vector3(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
        public static Vector3 operator -(Vector3 l, Vector3 r) => new Vector3(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
        public static Vector3 operator -(Vector3 v) => new Vector3(-v.X, -v.Y, -v.Z);
        public static Vector3 operator *(Vector3 l, Vector3 r) => new Vector3(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
        public static Vector3 operator /(Vector3 l, Vector3 r) => new Vector3(l.X / r.X, l.Y / r.Y, l.Z / r.Z);

        public static Vector3 operator +(double a, Vector3 v) => new Vector3(a + v.X, a + v.Y, a + v.Z);
        public static Vector3 operator +(Vector3 v, double a) => new Vector3(a + v.X, a + v.Y, a + v.Z);
        public static Vector3 operator -(double a, Vector3 v) => new Vector3(a - v.X, a - v.Y, a - v.Z);
        public static Vector3 operator -(Vector3 v, double a) => new Vector3(-a + v.X, -a + v.Y, -a + v.Z);
        public static Vector3 operator *(double a, Vector3 v) => new Vector3(a * v.X, a * v.Y, a * v.Z);
        public static Vector3 operator *(Vector3 v, double a) => new Vector3(a * v.X, a * v.Y, a * v.Z);
        public static Vector3 operator /(Vector3 v, double a) => new Vector3(v.X / a, v.Y / a, v.Z / a);
    }

    public struct Vector4
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public double Length => VectorAlgebra.Sqrt(VectorAlgebra.Dot2(this));
        public double Length2 => VectorAlgebra.Dot2(this);
        public double InvLength => VectorAlgebra.InverseSqrt(VectorAlgebra.Dot2(this));

        public Vector4(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector4 operator +(Vector4 l, Vector4 r) => new Vector4(l.X + r.X, l.Y + r.Y, l.Z + r.Z, l.W + r.W);
        public static Vector4 operator -(Vector4 l, Vector4 r) => new Vector4(l.X - r.X, l.Y - r.Y, l.Z - r.Z, l.W - r.W);
        public static Vector4 operator -(Vector4 v) => new Vector4(-v.X, -v.Y, -v.Z, -v.W);
        public static Vector4 operator *(Vector4 l, Vector4 r) => new Vector4(l.X * r.X, l.Y * r.Y, l.Z * r.Z, l.W * r.W);
        public static Vector4 operator /(Vector4 l, Vector4 r) => new Vector4(l.X / r.X, l.Y / r.Y, l.Z / r.Z, l.W / r.W);

        public static Vector4 operator +(double a, Vector4 v) => new Vector4(a + v.X, a + v.Y, a + v.Z, a + v.W);
        public static Vector4 operator +(Vector4 v, double a) => new Vector4(a + v.X, a + v.Y, a + v.Z, a + v.W);
        public static Vector4 operator -(double a, Vector4 v) => new Vector4(a - v.X, a - v.Y, a - v.Z, a - v.W);
        public static Vector4 operator -(Vector4 v, double a) => new Vector4(-a + v.X, -a + v.Y, -a + v.Z, -a + v.W);
        public static Vector4 operator *(double a, Vector4 v) => new Vector4(a * v.X, a * v.Y, a * v.Z, a * v.W);
        public static Vector4 operator *(Vector4 v, double a) => new Vector4(a * v.X, a * v.Y, a * v.Z, a * v.W);
        public static Vector4 operator /(Vector4 v, double a) => new Vector4(v.X / a, v.Y / a, v.Z / a, v.W / a);
    }

    public struct Matrix2x2
    {
        double[,] elems;
        public double Det => elems[0, 0] * elems[1, 1] - elems[1, 0] * elems[0, 1];

        public Matrix2x2(double a, double b, double c, double d)
        {
            elems = new double[2, 2];
            elems[0, 0] = a; elems[0, 1] = b;
            elems[1, 0] = c; elems[1, 1] = d;
        }

        public static Matrix2x2 Rotate(double angle)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            return new Matrix2x2(c, -s, s, c);
        }

        public static Matrix2x2 operator +(Matrix2x2 l, Matrix2x2 r) => new Matrix2x2(l[0, 0] + r[0, 0], l[0, 1] + r[0, 1], l[1, 0] + r[1, 0], l[1, 1] + r[1, 1]);
        public static Matrix2x2 operator -(Matrix2x2 l, Matrix2x2 r) => new Matrix2x2(l[0, 0] - r[0, 0], l[0, 1] - r[0, 1], l[1, 0] - r[1, 0], l[1, 1] - r[1, 1]);
        public static Matrix2x2 operator -(Matrix2x2 v) => new Matrix2x2(-v[0, 0], -v[0, 1], -v[1, 0], -v[1, 1]);

        public static Vector2 operator *(Vector2 v, Matrix2x2 m) => new Vector2(v.X * m[0, 0] + v.Y * m[1, 0], v.X * m[0, 1] + v.Y * m[1, 1]);
        public static Matrix2x2 operator *(Matrix2x2 l, Matrix2x2 r) => new Matrix2x2(l[0, 0] * r[0, 0] + l[0, 1] * r[1, 0], l[0, 0] * r[0, 1] + l[0, 1] * r[1, 1],
                                                                                      l[1, 0] * r[0, 0] + l[1, 1] * r[1, 0], l[1, 0] * r[0, 1] + l[1, 1] * r[1, 1]);

        public double this[int i, int j] => elems[i, j];
    }

    public struct Matrix3x3
    {
        double[,] elems;
        public double Det => elems[0, 0] * (elems[1, 1] * elems[2, 2] - elems[1, 2] * elems[2, 1])
                            -elems[0, 1] * (elems[1, 0] * elems[2, 2] - elems[1, 2] * elems[2, 0])
                            +elems[0, 2] * (elems[1, 0] * elems[2, 1] - elems[1, 1] * elems[2, 0]);

        public Matrix3x3(double a, double b, double c, double d, double e, double f, double g, double h, double i)
        {
            elems = new double[3, 3];
            elems[0, 0] = a; elems[0, 1] = b; elems[0, 2] = c;
            elems[1, 0] = d; elems[1, 1] = e; elems[1, 2] = f;
            elems[2, 0] = g; elems[2, 1] = h; elems[2, 2] = i;
        }

        public static Matrix3x3 RotateX(double angle)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            return new Matrix3x3(1, 0, 0,
                                 0, c, -s,
                                 0, s, c);
        }
        public static Matrix3x3 RotateY(double angle)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            return new Matrix3x3(c, 0, -s,
                                 0, 1, 0,
                                 s, 0, c);
        }
        public static Matrix3x3 RotateZ(double angle)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            return new Matrix3x3(c, -s, 0,
                                 s, c, 0,
                                 0, 0, 1);
        }

        public static Matrix3x3 operator +(Matrix3x3 l, Matrix3x3 r) => new Matrix3x3(l[0, 0] + r[0, 0], l[0, 1] + r[0, 1], l[0, 2] + r[0, 2],
                                                                                      l[1, 0] + r[1, 0], l[1, 1] + r[1, 1], l[1, 2] + r[1, 2],
                                                                                      l[2, 0] + r[2, 0], l[2, 1] + r[2, 1], l[2, 2] + r[2, 2]);
        public static Matrix3x3 operator -(Matrix3x3 l, Matrix3x3 r) => new Matrix3x3(l[0, 0] - r[0, 0], l[0, 1] - r[0, 1], l[0, 2] - r[0, 2],
                                                                                      l[1, 0] - r[1, 0], l[1, 1] - r[1, 1], l[1, 2] - r[1, 2],
                                                                                      l[2, 0] - r[2, 0], l[2, 1] - r[2, 1], l[2, 2] - r[2, 2]);
        public static Matrix3x3 operator -(Matrix3x3 v) => new Matrix3x3(-v[0, 0], -v[0, 1], -v[0, 2],
                                                                         -v[1, 0], -v[1, 1], -v[1, 2],
                                                                         -v[2, 0], -v[2, 1], -v[2, 2]);

        public static Vector3 operator *(Vector3 v, Matrix3x3 m) => new Vector3(v.X * m[0, 0] + v.Y * m[1, 0] + v.Z * m[2, 0],
                                                                                v.X * m[0, 1] + v.Y * m[1, 1] + v.Z * m[2, 1],
                                                                                v.X * m[0, 2] + v.Y * m[1, 2] + v.Z * m[2, 2]);
        public static Matrix3x3 operator *(Matrix3x3 l, Matrix3x3 r)
        {
            Func<int, int, double> el = (int i, int j) => l[i, 0] * r[0, j] + l[i, 1] * r[1, j] + l[i, 2] * r[2, j];

            return new Matrix3x3(el(0, 0), el(0, 1), el(0, 2),
                                 el(1, 0), el(1, 1), el(1, 2),
                                 el(2, 0), el(2, 1), el(2, 2));
        }

        public double this[int i, int j] => elems[i, j];
    }
}
