using System;
using OxCore.Data;

namespace OxCore.Data
{
    public class WorldData
    {
        private AgentData agent = new AgentData();
        private PointData point = new PointData();
        private SimCollectionData simCollection = new SimCollectionData();
        private StatusData status = new StatusData();

        public AgentData Agent { get { return agent; } }
        public PointData Point { get { return point; } }
        public SimCollectionData SimCollection { get { return simCollection; } }
        public StatusData Status { get { return status; } }

        public bool TryGetMyAvatarData(out ObjectData objectData)
        {
            objectData = null;
            if (simCollection.TryGetObject(agent.SimID, agent.ID, out objectData))
                return true;

            return false;
        }

        public bool TryGetCurrentSim(out SimData data)
        {
            if (simCollection.TryGet(agent.SimID, out data))
                return true;

            return false;
        }

        public void Reset()
        {
            agent.Reset();
            simCollection.Reset();
        }
    }
}
