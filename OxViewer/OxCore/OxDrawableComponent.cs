using System;
using System.Collections.Generic;
using System.Text;

namespace OxCore
{
    public interface IDrawable
    {
        void Draw();
    }

    public interface IOxDrawableComponent : IOxComponent, IDrawable
    {
    }

    public class OxDrawableComponent : OxComponent, IOxDrawableComponent
    {
        public OxDrawableComponent(Ox ox)
            : base(ox)
        {
        }

        public virtual void Draw() { }
    }
}
