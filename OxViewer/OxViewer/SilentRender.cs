using System;
using OxCore;

namespace OxViewer
{
    public class SilentRender : OxDrawableComponent
    {
        public SilentRender(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.Render;
        }

        public override void Update(ApplicationTime time)
        {
            System.Windows.Forms.Application.DoEvents();

            base.Update(time);
        }
    }
}
