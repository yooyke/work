using System;
using System.Collections.Generic;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;
using OxUtil;

namespace OxViewer
{
    public class Calculator : OxComponent
    {
        private Queue<JsonObjectUpdated> pipline = new Queue<JsonObjectUpdated>();

        public Calculator(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.ControllerCalc;

            ox.OnEvent += new OxEventHandler(ox_OnEvent);
        }

        public override void Update(ApplicationTime time)
        {
            while (pipline.Count > 0)
            {
                JsonObjectUpdated j;
                lock (pipline)
                    j = pipline.Dequeue();

                switch (j.type)
                {
                    case (int)JsonObjectUpdated.Type.Add:
                    case (int)JsonObjectUpdated.Type.Update:
                        UpdateObject(ref j);
                        break;
                }
            }

            CalcAgentHead();

            base.Update(time);
        }

        private void UpdateObject(ref JsonObjectUpdated j)
        {
            SimData data;
            if (!Ox.DataStore.World.SimCollection.TryGet(j.simID, out data))
                return;

            ObjectData objectData;
            if (!data.TryGet(j.id, out objectData))
                return;

            ObjectData parentData = null;
            if (!string.IsNullOrEmpty(objectData.ParentID))
            {
                if (data.Contaion(objectData.ParentID))
                {
                    if (!data.TryGet(objectData.ParentID, out parentData))
                        return;
                }
                else
                    return;
            }

            Q qo = new Q(objectData.OQuaternion[0], objectData.OQuaternion[1], objectData.OQuaternion[2], objectData.OQuaternion[3]);
            if (parentData == null)
            {
                for (int i = 0; i < objectData.OPosition.Length; i++)
                    objectData.Position[i] = objectData.OPosition[i];
            }
            else
            {
                Q qp = new Q(parentData.OQuaternion[0], parentData.OQuaternion[1], parentData.OQuaternion[2], parentData.OQuaternion[3]);
                qo = qo * qp;

                float[] v = MathHelper.RotateVect(qp.Matrix, objectData.OPosition);
                objectData.Position[0] = parentData.OPosition[0] + v[0];
                objectData.Position[1] = parentData.OPosition[1] + v[1];
                objectData.Position[2] = parentData.OPosition[2] + v[2];
            }
            float[] deg = MathHelper.RotationDegree(qo.Matrix);
            objectData.Rotation[0] = deg[0];
            objectData.Rotation[1] = deg[1];
            objectData.Rotation[2] = deg[2];
        }

        private void CalcAgentHead()
        {
            if (!Ox.DataStore.Input.MPress(MouseType.LButton))
                return;

            ObjectData objectData;
            if (!Ox.DataStore.World.TryGetMyAvatarData(out objectData))
                return;

            float[] tPos = new float[] { Ox.DataStore.World.Point.Position[0], Ox.DataStore.World.Point.Position[1], Ox.DataStore.World.Point.Position[2] };
            float[] pPos = new float[] { objectData.OPosition[0], objectData.OPosition[1], objectData.OPosition[2] };

            float angle = MathHelper.CalcTargetFromPosition(tPos, pPos);
            string msg = JsonUtil.SerializeMessage(JsonType.AgentHead, new JsonAgentHead(angle));
            Ox.Function(msg);
        }

        void ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type != JsonType.ObjectUpdated)
                return;

            JsonObjectUpdated j = (JsonObjectUpdated)JsonUtil.Deserialize<JsonObjectUpdated>(parse_msg.value);
            lock (pipline)
                pipline.Enqueue(j);
        }
    }
}
