using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KG2.Figures;

using VA = KG2.VectorAlgebra;
using Vec2 = KG2.Vector2;
using Vec3 = KG2.Vector3;
using Vec4 = KG2.Vector4;

namespace KG2
{
    class Scene
    {
        public Figure Figure { get; set; }
        public int AntiAliasingDepth { get; set; }

        public Vec3 BackgroundColor { get; set; } = VA.Vec3(0.125);
        public Vec3 HighLightColor { get; set; } = VA.Vec3(0.125);

        public Vec3 AmbientIntensity { get; set; }
        public Vec3 SourceIntensity { get; set; }
        public Vec3 SourcePos { get; set; }

        public Vec2 ProjectPlaneSize { get; set; } = VA.Vec2(16, 16);

        public Vec3 CameraPos { get; set; }
        public Vec3 CameraDir { get; set; }

        public int GlareFalloff { get; set; }

        public void RotateFigureX(double rad)
        {
            Figure.RotateX(rad);
        }

        public void RotateFigureY(double rad)
        {
            Figure.RotateY(rad);
        }

        public void RotateFigureZ(double rad)
        {
            Figure.RotateZ(rad);
        }

        public void TransformFigure(Matrix3x3 m)
        {
            Figure.Transform(m);
        }

        public void Render(Bitmap bm)
        {
            BitmapWrapper bmw = new BitmapWrapper(bm);

            Vec3 x = ProjectPlaneSize.X * VA.Normalize(VA.Vec3(-CameraDir.Z, 0, CameraDir.X));
            Vec3 y = ProjectPlaneSize.Y * VA.Normalize(VA.Cross(CameraDir, x));

            Vec3 corner = CameraPos - (x + y) / 2;

            bmw.Lock();
            double ia = 1.0 / AntiAliasingDepth;

            x *= 1.0 / bmw.Width;
            y *= 1.0 / bmw.Height;

            for (int i = 0; i < bmw.Height; i++)
            {
                for(int j = 0; j < bmw.Width; j++)
                {

                    Vec3 ro = corner + j * x + i * y;
                    Vec3 col = VA.Vec3(0.0);
                    for(int k = 0; k < AntiAliasingDepth; k++)
                    {
                        for(int l = 0; l < AntiAliasingDepth; l++)
                        {
                            Vec3 target = ro + k * ia * y + l * ia * x;
                            col += Pixel(target, i, j, bmw.Height, bmw.Width);
                        }
                    }
                    col *= ia * ia;

                    col = VA.Clamp(col, 0, 1);

                    bmw[j, i] = Color.FromArgb(255, (int)(255 * col.X), (int)(255 * col.Y), (int)(255 * col.Z));
                }
            }
            bmw.Unlock();
        }

        Vec3 Pixel(Vec3 ro, int i, int j, int h, int w)
        {
            Vec4 tnor = Figure.RayTrace(ro, CameraDir);
            double t = tnor.X;

            Vec3 color = BackgroundColor;

            if (Math.Abs(j - w / 2) <= 1 || Math.Abs(i - h / 2) <= 1) color = HighLightColor;

            if(tnor.X != -1 || tnor.Y != -1 || tnor.Z != -1 || tnor.W != -1)
            {
                Vec3 position = ro + t * CameraDir;
                Vec3 light = VA.Normalize(SourcePos - position);
                Vec3 normal = VA.Normalize(VA.Vec3(tnor.Y, tnor.Z, tnor.W));

                Vec3 reflected = VA.Normalize(VA.Reflect(-light, normal));
                Vec3 sight = VA.Normalize(-CameraDir);
                double align = VA.Dot(normal, light);
                Vec3 intensity;
                if (align <= 0.001)
                {
                    intensity = AmbientIntensity * Figure.Material.Ambient * Figure.Color;
                }
                else
                {
                    intensity = (AmbientIntensity * Figure.Material.Ambient + SourceIntensity * Figure.Material.Diffuse * align) * Figure.Color + SourceIntensity * Figure.Material.Specular * Math.Pow(VA.Clamp(VA.Dot(reflected, sight), 0, 1), GlareFalloff);
                }

                color = VA.Clamp(intensity, 0, 1);

                color = VA.Sqrt(color);
            }

            return color;
        }
    }
}
