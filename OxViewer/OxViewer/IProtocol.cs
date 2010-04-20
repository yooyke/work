using OxCore;

namespace OxViewer
{
    interface IProtocol : IOxComponent
    {
        void Login(string first, string last, string pass, string uir);
        void Logout();
        void RequestImage(string iamgeID);
        void Forward(bool press);
        void Backward(bool press);
        void Chat(string message, int channel, int type);
        void IM(string targetUUID, string message);
        void Sit(string targetUUID);
        void Standup();
        void Touch(string targetUUID);
        void Teleport(string landmarkUUID);
        void Teleport(string simName, float x, float y, float z);
        void Teleport(string simName, float x, float y, float z, float lookX, float lookY, float lookZ);

        void AgentHead(float angle);
        void Rotation(float velocity);
        void AlwaysRun(bool run);
    }
}
