using System;
using System.Collections.Generic;
using IrrlichtNETCP;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;
using OxUtil;

namespace OxRender
{
    public class Collision : OxComponent
    {
        private const int RATE_CALCCOLLISION = 5;

        private Queue<JsonObjectUpdated> pipline = new Queue<JsonObjectUpdated>();

        public Collision(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.ControllerCalc + 1;
        }

        public override void Update(ApplicationTime time)
        {
            if ((UpdateCount % RATE_CALCCOLLISION) == 0)
                CalcCollision();

            base.Update(time);
        }

        private void CalcCollision()
        {
            Ox.DataStore.World.Point.Type = PointData.ObjectType.None;

            CalcPlugins((IOxRenderPluginObject)Ox.Service.Get(typeof(IOxRenderPluginAvatar)));
            CalcPlugins((IOxRenderPluginObject)Ox.Service.Get(typeof(IOxRenderPluginPrim)));
            CalcPlugins((IOxRenderPluginObject)Ox.Service.Get(typeof(IOxRenderPluginTilePicker)));

            PointData.HitData[] datas = Ox.DataStore.World.Point.GetAll();
            if (datas == null)
                return;

            float length = float.MaxValue;
            foreach (PointData.HitData data in datas)
            {
                if (length > data.Length)
                {
                    Ox.DataStore.World.Point.Position = data.Point;
                    Ox.DataStore.World.Point.Type = data.Type;
                    Ox.DataStore.World.Point.Click = data.Click;
                    Ox.DataStore.World.Point.ID = data.ID;
                    length = data.Length;
                }
            }

            ObjectData objectData;
            if (Ox.DataStore.World.TryGetMyAvatarData(out objectData))
            {
                float[] len = new float[3];
                for (int i = 0; i < len.Length; i++)
                    len[i] = objectData.Position[i] - Ox.DataStore.World.Point.Position[i];

                float lenSq = MathHelper.Length(len);
                Ox.DataStore.World.Agent.LengthFromPoint = lenSq;
            }
        }

        private void CalcPlugins(IOxRenderPluginObject plugin)
        {
            if (plugin == null) return;
            plugin.Collision();
        }
    }
}
