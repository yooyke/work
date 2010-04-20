using System;
using System.Text;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;

namespace OxRender.Plugin.Debug
{
    public class Information : OxRenderComponentPlugin
    {
        private enum Type
        {
            None,
            Agent,
            World,
            System,
        }

        private Type type = Type.None;
        private GUIStaticText elem;
        private StringBuilder sb;
        private EventHandler action;

        public Information(Ox ox, Render view)
            : base(ox, view)
        {
        }

        public override void Initialize()
        {
            IOxRenderDebugController debug = (IOxRenderDebugController)Ox.Service.Get(typeof(IOxRenderDebugController));
            debug.CheckedChanged += new EventHandler(debug_CheckedChanged);

            elem = Render.GUI.AddStaticTextW(string.Empty, new Rect(new Position2D(0, 16), new Dimension2D(640, 240)), false, true, Render.GUI.RootElement, 0, false);
            elem.OverrideColor = Color.Red;

            base.Initialize();
        }

        public override void Update(ApplicationTime time)
        {
            if (Ox.DataStore.Input.ReleaseTrg(KeyType.Key_1))
            {
                type++;

                if (type > Type.System)
                    type = Type.None;

                switch(type)
                {
                    case Type.Agent:
                        action = InfoAgent;
                        break;
                    case Type.World:
                        action = InfoWorld;
                        break;
                    case Type.System:
                        action = InfoSystem;
                        break;
                    default:
                        action = null;
                        break;
                }
            }

            if (action == null)
            {
                sb = null;
                elem.Text = string.Empty;
            }
            else
            {
                action(this, EventArgs.Empty);
                elem.Text = sb.ToString();
            }

            base.Update(time);
        }

        void debug_CheckedChanged(object sender, EventArgs e)
        {
            IOxRenderDebugController debug = (IOxRenderDebugController)Ox.Service.Get(typeof(IOxRenderDebugController));
            Root.Visible = debug.GetVisible(OxRender.Plugin.Debug.Controller.Type.Information);
            elem.Visible = debug.GetVisible(OxRender.Plugin.Debug.Controller.Type.Information);
        }

        private void InfoAgent(object sender, EventArgs args)
        {
            sb = new StringBuilder();

            AgentData agent = Ox.DataStore.World.Agent;
            sb.AppendLine("AgentID : " + agent.ID);
            sb.AppendLine("Agent SimID : " + agent.SimID);
            sb.AppendLine("Agent SimName : " + agent.SimName);

            ObjectData objectData;
            if (Ox.DataStore.World.TryGetMyAvatarData(out objectData))
            {
                sb.AppendFormat("Parent : {0}, Pos : {1},{2},{3}", objectData.ParentID, objectData.OPosition[0], objectData.OPosition[1], objectData.OPosition[2]);
                sb.AppendLine();
            }
        }

        private void InfoWorld(object sender, EventArgs args)
        {
            sb = new StringBuilder();

            sb.AppendLine("Sim Count : " + Ox.DataStore.World.SimCollection.Count);
            SimData[] sims = Ox.DataStore.World.SimCollection.GetAll();
            if (sims != null)
            {
                foreach (SimData sim in sims)
                    sb.AppendLine(" - ID : " + sim.ID);
            }

            sb.AppendLine("Avatar Count : " + Ox.DataStore.World.SimCollection.GetAvatarCount());
            sb.AppendLine("Prim Count : " + Ox.DataStore.World.SimCollection.GetPrimCount());
            sb.AppendLine("Texture Count : " + Render.Texture.Count);

            float[] p = Ox.DataStore.World.Point.Position;
            sb.AppendFormat("Point Pos : {0}, {1}, {2}", p[0], p[1], p[2]);
            sb.AppendLine();
            sb.AppendFormat("Point Len : {0} Type : {1} ID : {2}", Ox.DataStore.World.Agent.LengthFromPoint, Ox.DataStore.World.Point.Type, Ox.DataStore.World.Point.ID);
            sb.AppendLine();
        }

        private void InfoSystem(object sender, EventArgs args)
        {
            sb = new StringBuilder();
            sb.AppendLine("State : " + Ox.DataStore.World.Status.Status);

            int count = 0;
            count = NodeCount(Render.Scene.RootSceneNode);
            sb.AppendLine("Node Count : " + count);
        }

        private int NodeCount(SceneNode node)
        {
            int count = 1;
            foreach (SceneNode child in node.Children)
                count += NodeCount(child);

            return count;
        }
    }
}
