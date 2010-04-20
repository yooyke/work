using System;
using System.Collections.Generic;
using System.Text;

namespace OxCore
{
    public interface IUpdatable
    {
        void Initialize();
        void Update(ApplicationTime time);
    }

    public interface IOxComponent : IUpdatable, IDisposable
    {
        Ox Ox { get; }
    }

    public class OxComponent : IOxComponent
    {
        private Ox ox;
        private int priority = (int)PriorityBase.Max;
        private int update_count = 0;

        protected int UpdateCount { get { return update_count; } }

        public Ox Ox { get { return ox; } }
        public int Priority { get { return priority; } set { priority = value; } }

        public OxComponent(Ox ox)
        {
            this.ox = ox;
            Ox.Component.Add(this);
        }

        public virtual void Initialize() { }
        public virtual void Update(ApplicationTime time)
        {
            update_count = (update_count + 1) % int.MaxValue;
        }
        public virtual void Cleanup() { }
        public virtual void Dispose() { }
    }
}
