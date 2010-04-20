using System;
using System.Collections.Generic;
using System.IO;
using IrrlichtNETCP;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;
using OxUtil;

namespace OxRender.Plugin.Default.Avatar
{
    public partial class Avatar : OxRenderAvatarPlugin
    {
        private static readonly Vector3D AVATAR_OFFSET = new Vector3D(0, 0, -0.78f);
        private const float AVATAR_INTERPOLATE_WALK_SPEED = 0.100f;
        private const float AVATAR_INTERPOLATE_RUN_SPEED = 0.180f;
        private const float AVATAR_INTERPOLATE_SYNC_DISTANCE = 3.0f;
        private const float AVATAR_RUN_LENGTH = 3.5f;
        private const string DEFAULT_MODEL_FILENAME = "default.x";
        private const string DEFAULT_MODEL_ANIMATION_FILENAME = "default.xml";

        private class Data
        {
            public AvatarMoveData Move = null;
            public AnimatedMeshSceneNode Node = null;
            public AvatarAnimationData Animation = null;
        }

        private SceneNode selectorNode = new SceneNode();
        private Dictionary<string, Data> list = new Dictionary<string, Data>();
        private Queue<JsonObjectUpdated> pipline = new Queue<JsonObjectUpdated>();
        private string dir;

        public Avatar(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderAvatar;

            Ox.OnEvent += new OxEventHandler(ox_OnEvent);
            dir = Path.Combine(Path.Combine(Ox.Paths.Application, "media"), "model_default");
        }

        public override void Initialize()
        {
            SetMyAvatarHeadPosition();

            base.Initialize();
        }

        public override void Load()
        {
            selectorNode = Render.Scene.AddEmptySceneNode(Root, -1);

            base.Load();
        }

        public override void Update(ApplicationTime time)
        {
            while (pipline.Count > 0)
            {
                JsonObjectUpdated j;
                lock (pipline)
                    j = pipline.Dequeue();

                if (Ox.DataStore.World.Status.Status < StatusData.Type.LoginEnd || StatusData.Type.Logout <= Ox.DataStore.World.Status.Status)
                    continue;

                if (string.IsNullOrEmpty(j.id))
                    continue;

                switch (j.type)
                {
                    case (int)JsonObjectUpdated.Type.Add:
                        CreateAvatar(ref j);
                        UpdateAvatar(ref j);
                        UpdateAvatarAnimation(ref j);
                        break;
                    case (int)JsonObjectUpdated.Type.Update:
                        UpdateAvatar(ref j);
                        break;
                    case (int)JsonObjectUpdated.Type.UpdateAnimation:
                        UpdateAvatarAnimation(ref j);
                        break;
                    case (int)JsonObjectUpdated.Type.Delete:
                        DeleteAvatar(j.id);
                        break;
                }
            }

            foreach (string key in list.Keys)
            {
                UpdatePosition(time, key);
                UpdateAnimation(key);
            }

            foreach (Data data in list.Values)
                data.Node.AnimateJoints(true);

            base.Update(time);
        }

        public override void Unload()
        {
            pipline.Clear();
            list.Clear();

            if (Root.Children.Length > 0)
                Root.RemoveAll();

            base.Unload();
        }

        public override SceneNode GetAvatarScneNode(string key)
        {
            if (list.ContainsKey(key))
                return list[key].Node;

            return null;
        }

        public override void Collision()
        {
            if (Render.Scene.ActiveCamera == null)
                return;

            Position2D mouse = new Position2D((int)Ox.DataStore.Input.Position[0], (int)Ox.DataStore.Input.Position[1]);
            Line3D line = Render.Scene.CollisionManager.GetRayFromScreenCoordinates(mouse, Render.Scene.ActiveCamera);

            foreach (string key in list.Keys)
            {
                if (list[key].Node == null)
                    continue;

                AnimatedMeshSceneNode node = list[key].Node;

                Box3D box;
                Util.CreateBox3DFromNode(node, out box);

                // Check inside bounding box
                if (box.IntersectsWithLimitedLine(line))
                {
                    Vector3D intersection = new Vector3D();
                    Triangle3D triangle = new Triangle3D();

                    Mesh mesh = node.AnimatedMesh.GetMesh(node.CurrentFrame);
                    TriangleSelector ts = Render.Scene.CreateTriangleSelector(mesh, node);

                    bool hit = Render.Scene.CollisionManager.GetCollisionPoint(
                                line,
                                ts,
                                out intersection,
                                out triangle
                                );

                    if (!hit)
                        continue;

                    ObjectData data;
                    if (!Ox.DataStore.World.SimCollection.TryGetObject(key, out data))
                        continue;

                    if (!(data is AvatarData))
                        continue;

                    AvatarData avatarData = data as AvatarData;
                    float[] point = Util.ToPositionArrayFromIrrlicht(ref intersection);
                    float length = (float)intersection.DistanceFrom(line.Start);
                    PointData.HitData hitData = new PointData.HitData(
                        point[0], point[1], point[2],
                        (avatarData.Myself ? PointData.ObjectType.AvatarSelf  : PointData.ObjectType.Avatar),
                        ClickActionType.None,
                        length,
                        key
                        );
                    Ox.DataStore.World.Point.Add(ref hitData);
                }
            }
        }

        void ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type != JsonType.ObjectUpdated)
                return;

            JsonObjectUpdated j = (JsonObjectUpdated)JsonUtil.Deserialize<JsonObjectUpdated>(parse_msg.value);
            if (j.prim == (int)JsonObjectUpdated.PrimType.Prim) // If j.prim is prim. do return
                return;

            lock (pipline)
                pipline.Enqueue(j);
        }

        private void CreateAvatar(ref JsonObjectUpdated update)
        {
            if (list.ContainsKey(update.id))
                return;

            AnimatedMesh mesh = Render.Scene.GetMesh(Path.Combine(dir, DEFAULT_MODEL_FILENAME));
            if (mesh == null) return;

            AnimatedMeshSceneNode node = Render.Scene.AddAnimatedMeshSceneNode(mesh);
            if (node == null) return;

            Root.AddChild(node);
            node.Name = update.id;
            node.Scale = new Vector3D(0.012f, 0.012f, 0.012f);
            node.AnimationSpeed = 30;
            node.SetFrameLoop(187, 217);
            node.SetTransitionTime(0.15f);
            node.AnimationEnd += new AnimationEnd(node_AnimationEnd);
            //node.DebugDataVisible = DebugSceneType.BoundingBox;

            Data data = new Data();
            data.Animation = new AvatarAnimationData();
            data.Animation.Parse(Path.Combine(dir, DEFAULT_MODEL_ANIMATION_FILENAME));
            data.Node = node;

            ObjectData objectData;
            if (!Ox.DataStore.World.SimCollection.TryGetObject(update.simID, update.id, out objectData))
                return;

            AvatarData avatar;
            if (!(objectData is AvatarData))
                return;

            avatar = objectData as AvatarData;
            avatar.UpdateAnimation(new AvatarData.Animation[] { new AvatarData.Animation(AK.GetKeyFromName(AnimationType.Standing.ToString()), 0) });

            list.Add(update.id, data);

            if (update.id == Ox.DataStore.World.Agent.ID)
            {
                string[] animations = data.Animation.GetAnimationNames();
                Ox.EventFire(JsonUtil.SerializeMessage(JsonType.AgentAnimationList, new JsonAgentAnimationList(update.id, animations)), true);
            }
        }

        private void UpdateAvatar(ref JsonObjectUpdated update)
        {
            if (!list.ContainsKey(update.id))
                CreateAvatar(ref update);

            ObjectData objectData;
            if (!Ox.DataStore.World.SimCollection.TryGetObject(update.simID, update.id, out objectData))
                return;

            AvatarData avatar;
            if (!(objectData is AvatarData))
                return;

            avatar = objectData as AvatarData;

            AnimatedMeshSceneNode node = list[update.id].Node;

            Vector3D position = Util.ToPositionRH(avatar.Position) + AVATAR_OFFSET;
            if (list[update.id].Move == null)
            {
                list[update.id].Move = new AvatarMoveData(ref position);
                node.Position = position;
            }
            AvatarMoveData move = list[update.id].Move;

            bool sync = false;
            // position must sync when avatar is sitting
            sync |= !string.IsNullOrEmpty(avatar.ParentID);
            // position must sync when avatar teleported
            //sync |= teleport-flag;

            if (sync)
            {
                move.Current = position;
            }

            float length = MathHelper.Length(avatar.Velocity);
            move.Update(ref position, length);

            if (avatar.Myself)
            {
                Render.RenderData.AgentPosition = position;

                node.Rotation = Util.ToRotationRH(new float[] {
                    0 + Util.ROTATION_AND_3DS_OFFSET.X,
                    0 + Util.ROTATION_AND_3DS_OFFSET.Y,
                    (float)(Ox.DataStore.World.Agent.Head * NewMath.RADTODEG) + Util.ROTATION_AND_3DS_OFFSET.Z
                });
            }
            else
            {
                node.Rotation = Util.ToRotationRH(new float[] {
                    avatar.Rotation[0] + Util.ROTATION_AND_3DS_OFFSET.X,
                    avatar.Rotation[1] + Util.ROTATION_AND_3DS_OFFSET.Y,
                    avatar.Rotation[2] + Util.ROTATION_AND_3DS_OFFSET.Z });
            }
        }

        private void UpdateAvatarAnimation(ref JsonObjectUpdated update)
        {
            if (!list.ContainsKey(update.id))
                return;

            ObjectData objectData;
            if (!Ox.DataStore.World.SimCollection.TryGetObject(update.simID, update.id, out objectData))
                return;

            AvatarData avatar;
            if (!(objectData is AvatarData))
                return;

            avatar = objectData as AvatarData;

            if (avatar.Animations == null)
                return;

            int seq = -1;
            string animationID = string.Empty;
            foreach (AvatarData.Animation animation in avatar.Animations)
            {
                if (string.IsNullOrEmpty(animation.id))
                    continue;

                if (seq < animation.sequenceID)
                {
                    seq = animation.sequenceID;
                    animationID = animation.id;
                }
            }

            if (string.IsNullOrEmpty(animationID))
                return;

            string key = AK.GetNameFromKey(animationID);

            AvatarAnimationData aad = list[update.id].Animation;
            if (aad == null)
                return;

            aad.Request(key);
        }

        private void UpdatePosition(ApplicationTime time, string key)
        {
            AnimatedMeshSceneNode node = list[key].Node;
            if (node == null)
                return;

            AvatarMoveData move = list[key].Move;
            if (move == null)
                return;

            Vector3D vec = move.Target - move.Current;
            float rate = ((float)time.ElapsedTime.TotalSeconds / (1.0f / Ox.DataStore.Core.FpsActiveTarget));
            float lensq = vec.LengthSQ;
            float speed = (((move.Length / move.Span) > AVATAR_RUN_LENGTH) ? AVATAR_INTERPOLATE_RUN_SPEED : AVATAR_INTERPOLATE_WALK_SPEED) * rate;
            if (lensq <= (speed * speed) || (AVATAR_INTERPOLATE_SYNC_DISTANCE * AVATAR_INTERPOLATE_SYNC_DISTANCE) < lensq)
            {
                move.Current = move.Target;
            }
            else
            {
                vec = vec.Normalize();
                move.Current += (vec * speed);
            }

            node.Position = move.Current;
            move.IncrementCount();
        }

        private void UpdateAnimation(string key)
        {
            AnimatedMeshSceneNode node = list[key].Node;
            if (node == null)
                return;

            AvatarMoveData move = list[key].Move;
            if (move == null)
                return;

            AvatarAnimationData aad = list[key].Animation;
            if (aad == null)
                return;

            AnimationData data = aad.Play();
            if (data == null)
                return;
            
            node.JointMode = (AK.IsInterpolationAnimation(data.Name) ? JointUpdateOnRenderMode.Control : JointUpdateOnRenderMode.None);
            node.LoopMode = false;
            node.SetFrameLoop(data.Start, data.End);
        }

        void node_AnimationEnd(AnimatedMeshSceneNode node)
        {
            if (node == null)
                return;

            if (!list.ContainsKey(node.Name))
                return;

            node.JointMode = JointUpdateOnRenderMode.None;

            AvatarAnimationData aad = list[node.Name].Animation;
            if (aad == null)
                return;

            if (AK.IsLoopAnimation(aad.Current))
                node.LoopMode = true;
        }

        private void DeleteAvatar(string key)
        {
            if (!list.ContainsKey(key))
                return;

            AnimatedMeshSceneNode node = list[key].Node;
            if (node == null) return;

            node.Remove();
            list.Remove(key);
        }

        private void SetMyAvatarHeadPosition()
        {
            Render.RenderData.AgentHeadPosition = -AVATAR_OFFSET + new Vector3D(0, 0, 0.6f);
        }
    }
}
