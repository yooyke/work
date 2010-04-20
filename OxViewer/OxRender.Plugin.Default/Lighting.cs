using System;
using IrrlichtNETCP;
using OxCore;

namespace OxRender.Plugin.Default
{
    public class Lighting : OxRenderComponentPlugin
    {
        public Lighting(Ox ox, Render render)
            : base(ox, render)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // Ambient Light
            Render.Scene.SetAmbientLight(new Colorf(1, 0.8f, 0.8f, 0.8f));

            // Directional Light
            Light data = new Light();
            //---------------------------------------------
            // original default value
            data.DiffuseColor = new Colorf(1, 1, 1, 1);
            data.SpecularColor = new Colorf(1, 1, 1, 1);
            data.AmbientColor = new Colorf(1, 0, 0, 0);
            data.InnerCone = 0;
            data.OuterCone = 45;
            data.Falloff = 2;
            data.Radius = 100;
            data.Attenuation = new Vector3D(1, 0, 0);
            data.Direction = new Vector3D(0, 0, 1);
            data.CastShadows = true;
            data.Type = LightType.Point;
            //---------------------------------------------

            data.Type = LightType.Directional;

            LightSceneNode node = Render.Scene.AddLightSceneNode(Root, new Vector3D(), Colorf.Red, 8, -1);
            node.LightData = data;
            node.Rotation = new Vector3D(135, 45, 0);
        }
    }
}
