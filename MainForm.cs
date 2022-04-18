using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using KG2.Figures;

namespace KG2
{
    public partial class MainForm : Form
    {
        Scene scene = new Scene();
        Bitmap bmp;

        bool wait = false;

        public MainForm()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            VerticalRotationBar.ValueChanged += (ob, ev) => HandleCamera();
            HorizontalRotationBar.ValueChanged += (ob, ev) => HandleCamera();
            ResetButton.Click += (ob, ev) => ResetCamera();

            PeakXBox.ValueChanged += (ob, ev) => HandleConePeak();
            PeakYBox.ValueChanged += (ob, ev) => HandleConePeak();
            PeakZBox.ValueChanged += (ob, ev) => HandleConePeak();

            BaseXBox.ValueChanged += (ob, ev) => HandleConeBase();
            BaseYBox.ValueChanged += (ob, ev) => HandleConeBase();
            BaseZBox.ValueChanged += (ob, ev) => HandleConeBase();

            RadiusBox.ValueChanged += (ob, ev) => HandleRadius();

            SourceXBox.ValueChanged += (ob, ev) => HandleSourcePos();
            SourceYBox.ValueChanged += (ob, ev) => HandleSourcePos();
            SourceZBox.ValueChanged += (ob, ev) => HandleSourcePos();

            ConeColorButton.Click += (ob, ev) => PickConeColor();
            SourceColorButton.Click += (ob, ev) => PickSourceColor();

            AntiAliasingBox.ValueChanged += (ob, ev) => HandleAntiAliasing();

            AmbientIntensityBox.ValueChanged += (ob, ev) => HandleAmbientIntensity();
            SourceIntensityBox.ValueChanged += (ob, ev) => HandleSourceIntensity();

            GlareFalloffBox.ValueChanged += (ob, ev) => HandleGlareFalloff();

            AmbientQuotientRBox.ValueChanged += (ob, ev) => HandleAmbientQuotient();
            AmbientQuotientGBox.ValueChanged += (ob, ev) => HandleAmbientQuotient();
            AmbientQuotientBBox.ValueChanged += (ob, ev) => HandleAmbientQuotient();

            DiffuseQuotientRBox.ValueChanged += (ob, ev) => HandleDiffuseQuotient();
            DiffuseQuotientGBox.ValueChanged += (ob, ev) => HandleDiffuseQuotient();
            DiffuseQuotientBBox.ValueChanged += (ob, ev) => HandleDiffuseQuotient();

            SpecularQuotientRBox.ValueChanged += (ob, ev) => HandleSpecularQuotient();
            SpecularQuotientGBox.ValueChanged += (ob, ev) => HandleSpecularQuotient();
            SpecularQuotientBBox.ValueChanged += (ob, ev) => HandleSpecularQuotient();

            ConeColorButton.BackColor = Color.FromArgb(255, 0, 128, 255);
            SourceColorButton.BackColor = Color.FromArgb(255, 255, 255, 255);

            InitializeScene();
            Render();
        }

        void InitializeScene()
        {
            Cone cone = new Cone(VectorAlgebra.Vec3(ConeColorButton.BackColor.R / 255.0, ConeColorButton.BackColor.G / 255.0, ConeColorButton.BackColor.B / 255.0),
                                 VectorAlgebra.Vec3((double)PeakXBox.Value, (double)PeakYBox.Value, (double)PeakZBox.Value),
                                 VectorAlgebra.Vec3((double)BaseXBox.Value, (double)BaseYBox.Value, (double)BaseZBox.Value),
                                 (double)RadiusBox.Value + 0.001);

            cone.Material = new Material(VectorAlgebra.Vec3((double)AmbientQuotientRBox.Value, (double)AmbientQuotientGBox.Value, (double)AmbientQuotientBBox.Value),
                                         VectorAlgebra.Vec3((double)DiffuseQuotientRBox.Value, (double)DiffuseQuotientGBox.Value, (double)DiffuseQuotientBBox.Value),
                                         VectorAlgebra.Vec3((double)SpecularQuotientRBox.Value, (double)SpecularQuotientGBox.Value, (double)SpecularQuotientBBox.Value));

            scene.Figure = cone;
            scene.AntiAliasingDepth = (int)AntiAliasingBox.Value;

            scene.SourcePos = VectorAlgebra.Vec3((double)SourceXBox.Value, (double)SourceYBox.Value, (double)SourceZBox.Value);

            scene.AmbientIntensity = VectorAlgebra.Vec3((double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.R / 255),
                                                        (double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.G / 255),
                                                        (double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.B / 255));

            scene.SourceIntensity = VectorAlgebra.Vec3((double)(SourceIntensityBox.Value * SourceColorButton.BackColor.R / 255),
                                                       (double)(SourceIntensityBox.Value * SourceColorButton.BackColor.G / 255),
                                                       (double)(SourceIntensityBox.Value * SourceColorButton.BackColor.B / 255));

            scene.GlareFalloff = (int)GlareFalloffBox.Value;

            double cv = Math.Cos(HorizontalRotationBar.Value / 24.0 * Math.PI);
            double sv = Math.Sin(HorizontalRotationBar.Value / 24.0 * Math.PI);

            double ch = Math.Cos(VerticalRotationBar.Value / 24.0 * Math.PI);
            double sh = Math.Sin(VerticalRotationBar.Value / 24.0 * Math.PI);

            scene.CameraDir = VectorAlgebra.Vec3(-cv * sh, -sv, -cv * ch); ;
        }

        void Render()
        {
            var t = DateTime.Now;
            scene.Render(bmp);
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            int ms = (int)(DateTime.Now - t).TotalMilliseconds;
            Console.WriteLine($"Time taken to render: {ms}ms");
        }

        void HandleCamera()
        {
            if (wait) return;

            double cv = Math.Cos(HorizontalRotationBar.Value / 24.0 * Math.PI);
            double sv = Math.Sin(HorizontalRotationBar.Value / 24.0 * Math.PI);

            double ch = Math.Cos(VerticalRotationBar.Value / 12.0 * Math.PI);
            double sh = Math.Sin(VerticalRotationBar.Value / 12.0 * Math.PI);

            scene.CameraDir = VectorAlgebra.Vec3(-cv * sh, -sv, -cv * ch);

            Render();
        }

        void ResetCamera()
        {
            wait = true;
            VerticalRotationBar.Value = 0;
            HorizontalRotationBar.Value = 6;
            wait = false;

            HandleCamera();
        }

        void HandleConePeak()
        {
            if (wait) return;

            var cone = scene.Figure as Cone;
            cone.Peak = VectorAlgebra.Vec3((double)PeakXBox.Value, (double)PeakYBox.Value, (double)PeakZBox.Value);

            Render();
        }

        void HandleConeBase()
        {
            if (wait) return;

            var cone = scene.Figure as Cone;
            cone.BaseCenter = VectorAlgebra.Vec3((double)BaseXBox.Value, (double)BaseYBox.Value, (double)BaseZBox.Value);

            Render();
        }

        void HandleRadius()
        {
            if (wait) return;

            var cone = scene.Figure as Cone;
            cone.BaseRadius = (double)RadiusBox.Value + 0.001;

            Render();
        }

        void HandleAntiAliasing()
        {
            if (wait) return;

            scene.AntiAliasingDepth = (int)AntiAliasingBox.Value;

            Render();
        }

        void HandleSourcePos()
        {
            if (wait) return;

            scene.SourcePos = VectorAlgebra.Vec3((double)SourceXBox.Value, (double)SourceYBox.Value, (double)SourceZBox.Value);

            Render();
        }

        void PickConeColor()
        {
            if (ColorPicker.ShowDialog(this) != DialogResult.OK) return;

            var col = ColorPicker.Color;

            ConeColorButton.BackColor = col;

            HandleConeColor();
        }

        void HandleConeColor()
        {
            if (wait) return;

            scene.Figure.Color = VectorAlgebra.Vec3(ConeColorButton.BackColor.R / 255.0, ConeColorButton.BackColor.G / 255.0, ConeColorButton.BackColor.B / 255.0);

            Render();
        }

        void PickSourceColor()
        {
            if (ColorPicker.ShowDialog(this) != DialogResult.OK) return;

            var col = ColorPicker.Color;

            SourceColorButton.BackColor = col;

            HandleSourceColor();
        }

        void HandleSourceColor()
        {
            if (wait) return;

            scene.AmbientIntensity = VectorAlgebra.Vec3((double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.R / 255),
                                                        (double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.G / 255),
                                                        (double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.B / 255));

            scene.SourceIntensity = VectorAlgebra.Vec3((double)(SourceIntensityBox.Value * SourceColorButton.BackColor.R / 255),
                                                       (double)(SourceIntensityBox.Value * SourceColorButton.BackColor.G / 255),
                                                       (double)(SourceIntensityBox.Value * SourceColorButton.BackColor.B / 255));

            Render();
        }

        void HandleAmbientIntensity()
        {
            if (wait) return;

            scene.AmbientIntensity = VectorAlgebra.Vec3((double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.R / 255),
                                                        (double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.G / 255),
                                                        (double)(AmbientIntensityBox.Value * SourceColorButton.BackColor.B / 255));

            Render();
        }

        void HandleSourceIntensity()
        {
            if (wait) return;

            scene.SourceIntensity = VectorAlgebra.Vec3((double)(SourceIntensityBox.Value * SourceColorButton.BackColor.R / 255),
                                                       (double)(SourceIntensityBox.Value * SourceColorButton.BackColor.G / 255),
                                                       (double)(SourceIntensityBox.Value * SourceColorButton.BackColor.B / 255));

            Render();
        }

        void HandleGlareFalloff()
        {
            if (wait) return;

            scene.GlareFalloff = (int)GlareFalloffBox.Value;

            Render();
        }

        void HandleAmbientQuotient()
        {
            if (wait) return;

            Material tmp = scene.Figure.Material;
            tmp.Ambient = VectorAlgebra.Vec3((double)AmbientQuotientRBox.Value, (double)AmbientQuotientGBox.Value, (double)AmbientQuotientBBox.Value);
            scene.Figure.Material = tmp;

            Render();
        }

        void HandleDiffuseQuotient()
        {
            if (wait) return;

            Material tmp = scene.Figure.Material;
            tmp.Diffuse = VectorAlgebra.Vec3((double)DiffuseQuotientRBox.Value, (double)DiffuseQuotientGBox.Value, (double)DiffuseQuotientBBox.Value);
            scene.Figure.Material = tmp;

            Render();
        }

        void HandleSpecularQuotient()
        {
            if (wait) return;

            Material tmp = scene.Figure.Material;
            tmp.Specular = VectorAlgebra.Vec3((double)SpecularQuotientRBox.Value, (double)SpecularQuotientGBox.Value, (double)SpecularQuotientBBox.Value);
            scene.Figure.Material = tmp;

            Render();
        }
    }
}
