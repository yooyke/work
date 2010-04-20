using System;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;
using OxUtil;
using OxJson;

namespace OxRender.Plugin.Prim
{
    public class Camera : OxRenderComponentPlugin
    {
        private CameraSceneNode node;

        public Camera(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderCamera;
        }

        public override void Load()
        {
            node = Render.Scene.AddCameraSceneNode(Root);
            node.FOV = 60 * NewMath.DEGTORAD;
            node.Position = new Vector3D(0, 0, -256);
            node.Target = new Vector3D(0, 0, 0);
            node.UpVector = new Vector3D(0, 0, 1);

            base.Load();
        }

        public override void Update(ApplicationTime time)
        {
            IOxRenderPluginAvatar avatar = (IOxRenderPluginAvatar)Ox.Service.Get(typeof(IOxRenderPluginAvatar));
            SceneNode sn = avatar.GetAvatarScneNode(Ox.DataStore.World.Agent.ID);
            if (sn == null || node == null)
                return;

            node.Target = sn.Position + Render.RenderData.AgentHeadPosition;

            Matrix4 rot = new Matrix4();
            //rot.RotationDegrees = Util.ToRotationRH(new float[] {
            //    0,
            //    0,
            //    (float)(Ox.DataStore.World.Agent.Head * NewMath.RADTODEG) + Util.ROTATION_AND_3DS_OFFSET.Z });

            Quaternion q0 = new Quaternion();
            Quaternion q1 = new Quaternion();
            q0.fromAngleAxis(Ox.DataStore.Camera.Angle[0] + NewMath.DEGTORAD * Util.ROTATION_AND_3DS_OFFSET.Z, new Vector3D(0, 0, 1));
            q1.fromAngleAxis(Ox.DataStore.Camera.Angle[1], new Vector3D(1, 0, 0));
            q1 = q1 * q0;
            Vector3D vec = new Vector3D(0, -Ox.DataStore.Camera.Distance, 0);
            node.Position = node.Target + q1.Matrix.RotateVect(ref vec);

            if (Ox.DataStore.Camera.Angle[1] < -MathHelper.PIOver2 || MathHelper.PIOver2 < Ox.DataStore.Camera.Angle[1])
                node.UpVector = new Vector3D(0, 0, -1);
            else
                node.UpVector = new Vector3D(0, 0, 1);

            base.Update(time);
        }

        public override void Unload()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();

            node = null;

            base.Unload();
        }
    }
}
