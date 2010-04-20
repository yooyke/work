using IrrlichtNETCP;
using OxCore;

namespace OxRender
{
    public partial class Render
    {
        void UpdateInput()
        {
            Ox.DataStore.Input.Backup();
        }

        void ResetInput()
        {
            Ox.DataStore.Input.Reset();
        }

        bool device_OnEvent(Event ev)
        {
            switch (ev.Type)
            {
                case EventType.MouseInputEvent:
                    if (ev.MouseInputEvent == MouseInputEvent.LMousePressedDown)
                        Ox.DataStore.Input.SetMKey(OxCore.Data.MouseType.LButton, true);
                    if (ev.MouseInputEvent == MouseInputEvent.MMousePressedDown)
                        Ox.DataStore.Input.SetMKey(OxCore.Data.MouseType.MButton, true);
                    if (ev.MouseInputEvent == MouseInputEvent.RMousePressedDown)
                        Ox.DataStore.Input.SetMKey(OxCore.Data.MouseType.RButton, true);
                    if (ev.MouseInputEvent == MouseInputEvent.LMouseLeftUp)
                        Ox.DataStore.Input.SetMKey(OxCore.Data.MouseType.LButton, false);
                    if (ev.MouseInputEvent == MouseInputEvent.MMouseLeftUp)
                        Ox.DataStore.Input.SetMKey(OxCore.Data.MouseType.MButton, false);
                    if (ev.MouseInputEvent == MouseInputEvent.RMouseLeftUp)
                        Ox.DataStore.Input.SetMKey(OxCore.Data.MouseType.RButton, false);

                    Ox.DataStore.Input.SetMPosition(ev.MousePosition.X, ev.MousePosition.Y);

                    if (ev.MouseInputEvent == MouseInputEvent.MouseWheel)
                        Ox.DataStore.Input.SetMDelta(ev.MouseWheelDelta);
                    break;

                case EventType.KeyInputEvent:
                    Ox.DataStore.Input.SetKey((OxCore.Data.KeyType)ev.KeyCode, ev.KeyPressedDown);
                    break;
            }

            return false;
        }
    }
}
