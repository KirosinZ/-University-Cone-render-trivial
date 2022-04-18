using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KG2.Figures
{
    struct Material
    {
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public Material(Vector3 amb, Vector3 dif, Vector3 spec)
        {
            Ambient = amb;
            Diffuse = dif;
            Specular = spec;
        }
    }
}
