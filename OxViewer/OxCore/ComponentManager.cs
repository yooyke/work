using System;
using System.Collections.Generic;

namespace OxCore
{
    public class ComponentManager
    {
        private class SortByPriority : IComparer<OxComponent>
        {
            public int Compare(OxComponent x, OxComponent y)
            {
                return (x.Priority - y.Priority);
            }
        }

        private List<OxComponent> list;
        private SortByPriority sortclass;

        public ComponentManager()
        {
            list = new List<OxComponent>();
            sortclass = new SortByPriority();
        }

        public void Add(OxComponent component)
        {
            list.Add(component);
        }

        public void Remove(OxComponent component)
        {
            list.Remove(component);
        }

        public OxComponent[] GetAll()
        {
            return list.ToArray();
        }

        public void Sort()
        {
            list.Sort(sortclass);
        }
    }
}
