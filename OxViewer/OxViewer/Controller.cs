using System;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;
using OxUtil;
using OxViewer.LibOMV;

namespace OxViewer
{
    partial class Controller : OxComponent
    {
        private const float WALK_LENGTH = 1.2f; // Avatar dosen't move if it is below the length. 
        private const float RUN_LENGTH = 4.5f;
        private const double FADE_TIME_SECOND = 1.2f;

        private delegate void ActionListener(string message);
        private IProtocol protocol;
        private Status status;
        private Progress progress;
        private ActionListener Action;
        private double counter;

        public Controller(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.Controller;

            Ox.OnEvent += new OxEventHandler(Ox_OnEvent);
            Ox.OnFunction += new OxEventHandler(Ox_OnFunction);

            protocol = new Protocol(Ox);
            status = new Status(Ox);
            progress = new Progress(Ox);
        }

        public override void Update(ApplicationTime time)
        {
            Ox.DataStore.World.Point.Clear();

            ProcessProgress();
            ProcessAllUserControl();
            if (Ox.DataStore.World.Status.Status == StatusData.Type.Waiting)
            {
                ProcessWaitingUserControl();
            }
            else if (Ox.DataStore.World.Status.Status == StatusData.Type.Running)
            {
                ProcessRunningUserControl();
            }

            if ((counter >= 0) && (counter += time.ElapsedTime.TotalSeconds) > FADE_TIME_SECOND)
            {
                counter = -1;

                if (Ox.DataStore.World.Status.Status == StatusData.Type.RunningFade)
                {
                    counter = FADE_TIME_SECOND - 0.2f;
                    Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.RunningBef)), false);
                }

                else if (Ox.DataStore.World.Status.Status == StatusData.Type.RunningBef)
                {
                    Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.Running)), false);
                }

                else if (Ox.DataStore.World.Status.Status == StatusData.Type.WaitingFade)
                {
                    counter = FADE_TIME_SECOND - 0.2f;
                    Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.WaitingBef)), false);
                }

                else if (Ox.DataStore.World.Status.Status == StatusData.Type.WaitingBef)
                {
                    Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.Waiting)), false);
                }
            }

            base.Update(time);
        }

        private void ProcessProgress()
        {
            if (Ox.DataStore.World.Status.Status != StatusData.Type.LoginEnd && Ox.DataStore.World.Status.Status != StatusData.Type.LogoutEnd)
                return;

            if (Ox.DataStore.World.Status.Progress == StatusData.PROGRESS_MAX && Ox.DataStore.World.Status.Status == StatusData.Type.LoginEnd)
            {
                counter = 0;
                Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.RunningFade)), false);
            }

            else if (Ox.DataStore.World.Status.Progress == 0 && Ox.DataStore.World.Status.Status == StatusData.Type.LogoutEnd)
            {
                counter = 0;
                Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.WaitingFade)), false);
            }
        }

        private void ProcessAllUserControl()
        {
            if (Ox.DataStore.Input.ReleaseTrg(KeyType.Apps))
            {
                if (!Ox.Menu.Visible)
                    Ox.Menu.Show();
            }
        }

        private void ProcessWaitingUserControl()
        {
        }

        private void ProcessRunningUserControl()
        {
            int move_key = 0;
            if (Ox.DataStore.Input.Press(KeyType.Up))
                move_key |= (int)JsonMovement.Type.Forward;

            if (Ox.DataStore.Input.Press(KeyType.Down))
                move_key |= (int)JsonMovement.Type.Backward;

            if (Ox.DataStore.Input.Press(KeyType.Left))
                move_key |= (int)JsonMovement.Type.Left;

            if (Ox.DataStore.Input.Press(KeyType.Right))
                move_key |= (int)JsonMovement.Type.Right;

            if (Ox.DataStore.Input.MPress(MouseType.LButton))
            {
                if (Ox.DataStore.World.Point.Type == PointData.ObjectType.Ground)
                {
                    if (Ox.DataStore.Input.MCount(MouseType.LButton) > 3)
                    {
                        if (Ox.DataStore.World.Agent.LengthFromPoint > WALK_LENGTH)
                        {
                            bool running = Ox.DataStore.World.Agent.LengthFromPoint > RUN_LENGTH;

                            // Check runnnig
                            if (running && !Ox.DataStore.World.Agent.AlwaysRun)
                                move_key |= (int)JsonMovement.Type.AlwaysRun;
                            else if (!running && Ox.DataStore.World.Agent.AlwaysRun)
                                move_key |= (int)JsonMovement.Type.AlwaysWalk;

                            // Walk (Run) to forward
                            move_key |= (int)JsonMovement.Type.Forward;
                        }
                    }

                    move_key |= (int)JsonMovement.Type.RotationUpdate;
                }
                else
                {
                    if (Ox.DataStore.Input.MPressTrg(MouseType.LButton))
                        Ox.EventFire(JsonUtil.SerializeMessage(JsonType.Clicked, new JsonClicked(Ox.DataStore.World.Point.ID)), true);
                }
            }

            Ox.Function(JsonUtil.SerializeMessage(JsonType.Movement, new JsonMovement(move_key)));

            if (Ox.DataStore.Input.Delta != 0)
            {
                float dist = Ox.DataStore.Camera.Distance - Ox.DataStore.Input.Delta * Default.CAMERA_DELTA_SPEED;
                Ox.DataStore.Camera.Distance = MathHelper.Clamp(dist, Ox.DataStore.Camera.DistanceMin, Ox.DataStore.Camera.DistanceMax);
            }

            if (Ox.DataStore.Input.MPress(MouseType.RButton))
            {
                if (!Ox.DataStore.Input.MPressTrg(MouseType.RButton))
                {
                    float x = Ox.DataStore.Camera.Angle[0] + (Ox.DataStore.Input.Position[0] - Ox.DataStore.Camera.Origin[0]) * Default.CAMERA_ROTATION_SPEED;
                    float y = Ox.DataStore.Camera.Angle[1] + (Ox.DataStore.Input.Position[1] - Ox.DataStore.Camera.Origin[1]) * Default.CAMERA_ROTATION_SPEED;
                    x %= MathHelper.TwoPI;
                    y %= MathHelper.TwoPI;
                    y = MathHelper.Clamp(y, -(MathHelper.PIOver2 - 0.01f), (MathHelper.PIOver2 - 0.01f));
                    Ox.DataStore.Camera.SetAngle(
                        x,
                        y
                        );
                }

                Ox.DataStore.Camera.SetOrigin(Ox.DataStore.Input.Position[0], Ox.DataStore.Input.Position[1]);
            }
        }
    }
}
