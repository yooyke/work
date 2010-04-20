using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxRender.Plugin.Default
{
    /// <summary>
    /// This class is for title. So load and unload is opposite to the other plugins.
    /// </summary>
    public class Title : OxRenderComponentPlugin
    {
        private CameraSceneNode camera;
        private SceneNode root;

        protected new SceneNode Root { get { return root; } }

        public Title(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.Render;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Set title camera
            System.Type type = this.GetType();
            camera = Render.Scene.AddCameraSceneNode(Render.Scene.RootSceneNode);
            camera.Position = new Vector3D(0, 0, -256);
            camera.Target = new Vector3D(0, 0, 0);
            camera.UpVector = new Vector3D(0, 0, 1);
            camera.Name = type.FullName + ".Camera" ;

            root = Render.Scene.AddEmptySceneNode(Render.Scene.RootSceneNode, -1);
            root.Name = type.FullName + ".Root";

            CreateTitle();
        }

        /// <summary>
        /// [load and unload is opposite to the other plugins!!]
        /// </summary>
        public override void Load()
        {
            DeleteTitle();

            base.Load();
        }

        /// <summary>
        /// [load and unload is opposite to the other plugins!!]
        /// </summary>
        public override void Unload()
        {
            CreateTitle();

            base.Unload();
        }

        private void CreateTitle()
        {
            //Render.Scene.AddSphereSceneNode(32, 8, Root);
            Render.Scene.ActiveCamera = camera;
        }

        private void DeleteTitle()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();
        }
    }
}
