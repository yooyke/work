using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using OxLoader;
using OxJson;
using OxCore.Data;

namespace OxHost
{
    public partial class Form1 : Form
    {
        private enum LoginType
        {
            None,
            LoginReq,
            Login,
            LogoutReq,
        }

        private delegate void GeneralDelegate();
        private delegate void BooleanDelegate(bool enabled);
        private delegate void StringDelegate(string text);
        private delegate void AgentAnimationListDelegate(string id, string[] list);
        private delegate void ObjectInfoDelegate(string id, string type, string click, string name, float[] position, float[] rotation, float[] scale);

        private Dictionary<string, string> login_param= new Dictionary<string, string>();
        private string login_param_path;
        private Loader loader;
        private int chatMessageLength;
        private int loginWaitTime = 1;
        private LoginType loginState = LoginType.None;
        private bool exit = false;
        private int requestClickedID = -1;
        private int requestID = 0;
        // Agent data
        private string id;
        private string first;
        private string last;

        public Form1()
        {
            InitializeComponent();

            login_param.Add("first", string.Empty);
            login_param.Add("last", string.Empty);
            login_param.Add("pass", string.Empty);
            login_param.Add("location", string.Empty);
            login_param.Add("uri", string.Empty);

            chatTypeCB.SelectedIndex = 1;
        }

        private void Exit()
        {
            exit = true;

            if (loader != null)
                loader.Exit();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            loader = new Loader(panel.Width, panel.Height, (int)Loader.ModeType.Normal);
            loader.OnEventJS += new OxEventHandler(loader_OnEventJS);
            loader.Run(panel.Handle);
        }

        void loader_OnEventJS(string message)
        {
            JsonMessage msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out msg);

            switch (type)
            {
                case JsonType.State:
                    State(msg.value);
                    break;
                case JsonType.AgentInfo:
                    AgentInfo(msg.value);
                    break;
                case JsonType.AgentAnimationList:
                    AgentAnimationList(msg.value);
                    break;
                case JsonType.ChatReceived:
                    ChatReceived(msg.value);
                    break;
                case JsonType.IMReceived:
                    IMReceived(msg.value);
                    break;
                case JsonType.Clicked:
                    Clicked(msg.value);
                    break;
                case JsonType.ObjectInfo:
                    ObjectInfo(msg.value);
                    break;
                case JsonType.PathInfo:
                    PathInfo(msg.value);
                    break;
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            switch (loginState)
            {
                case LoginType.None:
                    LoginEnabled(false);
                    loginState = LoginType.LoginReq;
                    Login();
                    break;
                case LoginType.Login:
                    LoginEnabled(false);
                    loginState = LoginType.LogoutReq;
                    Logout();
                    break;
            }
        }

        private void chatButton_Click(object sender, EventArgs e)
        {
            int type = 1;
            switch (chatTypeCB.SelectedIndex)
            {
                case 0://whisper
                    type = 0;
                    break;
                case 1://normal
                    type = 1;
                    break;
                case 2://showt
                    type = 2;
                    break;
            }

            Chat(chatMessageTB.Text, 0, type);
            chatMessageTB.Text = string.Empty;
        }

        private void chatB_TextChanged(object sender, EventArgs e)
        {
            if (chatMessageLength == 0)
                Chat(chatMessageTB.Text, 0, 4); // StartTyping
            else if (chatMessageTB.Text.Length == 0)
                Chat(chatMessageTB.Text, 0, 5); // StopTyping

            chatMessageLength = chatMessageTB.Text.Length;
        }

        private void imButton_Click(object sender, EventArgs e)
        {
            IM(imIDTB.Text, imMessageTB.Text);
            imMessageTB.Text = string.Empty;
        }

        private void State(string message)
        {
            JsonState p = (JsonState)JsonUtil.Deserialize<JsonState>(message);
            switch (p.state)
            {
                case (int)JsonState.Type.Initialize:
                    Initialize();
                    break;
                case (int)JsonState.Type.Cleanup:
                    break;
                case (int)JsonState.Type.Login:
                    LoggedIn();
                    break;
                case (int)JsonState.Type.Logout:
                    LoggedOut();
                    break;
            }
        }

        private void AgentInfo(string message)
        {
            JsonAgentInfo p = (JsonAgentInfo)JsonUtil.Deserialize<JsonAgentInfo>(message);
            id = p.id;
            first = p.first;
            last = p.last;
        }

        private void AgentAnimationList(string message)
        {
            JsonAgentAnimationList p = (JsonAgentAnimationList)JsonUtil.Deserialize<JsonAgentAnimationList>(message);
            gestureCB.Invoke(new AgentAnimationListDelegate(AgentAnimationListInvoke), new object[] { p.id, p.list });
        }

        private void ChatReceived(string message)
        {
            JsonChatReceived p = (JsonChatReceived)JsonUtil.Deserialize<JsonChatReceived>(message);
            if (p.type < 3)
                chatLB.Invoke(new StringDelegate(ChatReceivedInvoke), new object[] { string.Format("{0} : {1}", p.name, p.message) });
        }

        private void IMReceived(string message)
        {
            JsonIMReceived p = (JsonIMReceived)JsonUtil.Deserialize<JsonIMReceived>(message);
            chatLB.Invoke(new StringDelegate(IMReceivedInvoke), new object[] { string.Format("{0} : {1}", p.fromName, p.message) });
        }

        private void Clicked(string message)
        {
            JsonClicked p = (JsonClicked)JsonUtil.Deserialize<JsonClicked>(message);
            if (string.IsNullOrEmpty(p.id))
                return;

            requestClickedID = requestID;
            loader.Function(JsonUtil.SerializeMessage(JsonType.RequestObjectInfo, new JsonRequestObjectInfo(requestID++, p.id)));
        }

        private void ObjectInfo(string message)
        {
            JsonObjectInfo p = (JsonObjectInfo)JsonUtil.Deserialize<JsonObjectInfo>(message);
            objectIDTB.Invoke(new ObjectInfoDelegate(ObjectInfoInvoke), new object[] {
                p.id,
                string.Format("{0} : {1}", p.type, ((PointData.ObjectType)p.type).ToString()),
                string.Format("{0} : {1}", p.click, ((ClickActionType)p.click).ToString()),
                p.name,
                p.position,
                p.rotation,
                p.scale
            });

            if (p.type == (int)PointData.ObjectType.Avatar)
                imIDTB.Invoke(new StringDelegate(IMIDInvoke), new object[] { p.id });

            if (requestClickedID == p.requestID)
            {
                string msg = string.Empty;
                switch (p.click)
                {
                    case (int)ClickActionType.Touch:
                        msg = JsonUtil.SerializeMessage(JsonType.Touch, new JsonTouch(p.id));
                        break;
                    case (int)ClickActionType.Sit:
                        msg = JsonUtil.SerializeMessage(JsonType.Sit, new JsonSit(p.id));
                        break;
                }

                if (!string.IsNullOrEmpty(msg))
                    loader.Function(msg);
    
                requestClickedID = -1;
            }
        }

        private void PathInfo(string message)
        {
            JsonPathInfo p = (JsonPathInfo)JsonUtil.Deserialize<JsonPathInfo>(message);

            if (p.count == 0)
                return;

            string user_dir = null;
            for (int i = 0; i < p.count; i++)
            {
                if (p.keys[i] == "user")
                {
                    user_dir = p.values[i];
                    break;
                }
            }

            if (string.IsNullOrEmpty(user_dir))
                return;

            login_param_path = Path.Combine(user_dir, "login_param.xml");
            LoadLoginParam();
        }

        private void Login()
        {
            string msg;

            login_param["first"] = firstTB.Text;
            login_param["last"] = lastTB.Text;
            login_param["pass"] = passTB.Text;
            login_param["uri"] = uriTB.Text;
            login_param["location"] = locationTB.Text;
            
            msg= JsonUtil.SerializeMessage(JsonType.Setting, new JsonSetting(0, loginWaitTime));
            loader.Function(msg);

            msg = JsonUtil.SerializeMessage(JsonType.Login, new JsonLogin(login_param["first"], login_param["last"], login_param["pass"], login_param["uri"], login_param["location"]));
            loader.Function(msg);
        }

        private void Logout()
        {
            string msg;

            msg = JsonUtil.SerializeMessage(JsonType.Logout, new JsonLogout());
            loader.Function(msg);

            requestClickedID = -1;
        }

        private void Initialize()
        {
            // Setting
            string msg = JsonUtil.SerializeMessage(JsonType.Setting, new JsonSetting((int)JsonSetting.EconomyType.Full, 1));
            loader.Function(msg);

            // Title background
            msg = JsonUtil.SerializeMessage(JsonType.Title, new JsonTitle(new string[]{
                "http://www.geocities.jp/yattyatta2004/kurisumasu00011.jpg",
                "http://epep.furael.org/images/2005christmas_1_t.jpg",
                "http://www2u.biglobe.ne.jp/~endo-c/gallery/gifts/xmas2007.jpg",
                "http://oekakiart.net/xmas/xmas2008/xdada/xmas_art2008_112.png",
                "http://www.wallpaperlink.com/images/wallpaper/2007/0711/04094x.jpg",
                "http://us.123rf.com/400wm/135/0/olgaaltunina/olgaaltunina0812/olgaaltunina081200027/4024643.jpg",
                "http://us.123rf.com/400wm/400/400/chagall/chagall0810/chagall081000025/3762562.jpg",
                "http://pgj-gift.hp.infoseek.co.jp/a_img/imgGift/Gift49/03christmas-iwashi.JPG"
            }, 3));
            loader.Function(msg);

            // Request viewer path
            msg = JsonUtil.SerializeMessage(JsonType.RequestPathInfo, new JsonRequestPathInfo());
            loader.Function(msg);
        }

        private void LoggedIn()
        {
            loginButton.Invoke(new StringDelegate(LoginTextInvoke), new object[] { "Logout" });
            loginState = LoginType.Login;
            LoginEnabled(true);

            SaveLoginParam();
        }

        private void LoggedOut()
        {
            if (exit)
                return;

            loginButton.Invoke(new StringDelegate(LoginTextInvoke), new object[] { "Login" });
            loginState = LoginType.None;
            LoginEnabled(true);
        }

        private void LoginEnabled(bool enabled)
        {
            if (exit)
                return;

            loginPanel.Invoke(new BooleanDelegate(LoginEnabledInvoke), new object[] { enabled });
        }

        private void LoginParamSttingInvoke()
        {
            firstTB.Text = login_param["first"];
            lastTB.Text = login_param["last"];
            passTB.Text = login_param["pass"];
            uriTB.Text = login_param["uri"];
            locationTB.Text = login_param["location"];
        }

        private void LoginEnabledInvoke(bool enabled)
        {
            loginPanel.Enabled = enabled;
            loginButton.Enabled = enabled;
        }

        private void LoginTextInvoke(string text)
        {
            loginButton.Text = text;
        }

        private void Chat(string message, int channel, int type)
        {
            string msg = JsonUtil.SerializeMessage(JsonType.Chat, new JsonChat(message, channel, type));
            loader.Function(msg);
        }

        private void ChatReceivedInvoke(string message)
        {
            chatLB.Items.Add(message);
            chatLB.SelectedIndex = (chatLB.Items.Count - 1);
            tabControl1.SelectTab("chatTab");
        }

        private void IMReceivedInvoke(string message)
        {
            imLB.Items.Add(message);
            imLB.SelectedIndex = (imLB.Items.Count - 1);
            tabControl1.SelectTab("imTab");
        }

        private void IM(string targetUUID, string message)
        {
            string msg = JsonUtil.SerializeMessage(JsonType.IM, new JsonIM(targetUUID, message));
            loader.Function(msg);
        }

        private void AgentAnimationListInvoke(string id, string[] list)
        {
            if (this.id != id || list == null)
                return;

            gestureCB.Items.AddRange(list);
            gestureCB.SelectedIndex = 0;
        }

        private void ObjectInfoInvoke(string id, string type, string click, string name, float[] position, float[] rotaition, float[] scale)
        {
            objectIDTB.Text = id;
            objectTypeTB.Text = type;
            objectClickTB.Text = click;
            objectNameTB.Text = name;
            objectPositionTB.Text = string.Format("{0}, {1}, {2}", position[0], position[1], position[2]);
            objectRotationTB.Text = string.Format("{0}, {1}, {2}", rotaition[0], rotaition[1], rotaition[2]);
            objectScaleTB.Text = string.Format("{0}, {1}, {2}", scale[0], scale[1], scale[2]);
        }

        private void IMIDInvoke(string id)
        {
            imIDTB.Text = id;
        }

        private void Teleport_Click(object sender, EventArgs e)
        {
            float x, y, z;
            float.TryParse(telXLabel.Text, out x);
            float.TryParse(telYLabel.Text, out y);
            float.TryParse(telZLabel.Text, out z);
            string msg = JsonUtil.SerializeMessage(JsonType.TeleportFromSimName, new JsonTeleportFromSimName(telRegionLabel.Text, x, y, z));
            loader.Function(msg);
        }

        private void passTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                loginButton_Click(sender, e);
        }

        private void loginWait_Scroll(object sender, EventArgs e)
        {
            loginWaitLabel.Text = loginWait.Value.ToString();
            loginWaitTime = loginWait.Value;
        }

        private void standupButton_Click(object sender, EventArgs e)
        {
            string msg = JsonUtil.SerializeMessage(JsonType.Standup, new JsonStandup());
            loader.Function(msg);
        }

        private void LoadLoginParam()
        {
            if (string.IsNullOrEmpty(login_param_path) || !File.Exists(login_param_path))
                return;

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(login_param_path);
            }
            catch
            {
                doc = null;
            }

            if (doc == null)
                return;

            foreach (XmlNode child in doc.ChildNodes)
                RecursiveNode(child);
            firstTB.Invoke(new GeneralDelegate(LoginParamSttingInvoke));
        }

        private void RecursiveNode(XmlNode node)
        {
            if (!string.IsNullOrEmpty(node.Name))
            {
                switch (node.Name.ToLower())
                {
                    case "xml":
                    case "root":
                        break;
                    case "first":
                        login_param["first"] = node.InnerText;
                        break;
                    case "last":
                        login_param["last"] = node.InnerText;
                        break;
                    case "pass":
                        login_param["pass"] = node.InnerText;
                        break;
                    case "location":
                        login_param["location"] = node.InnerText;
                        break;
                    case "uri":
                        login_param["uri"] = node.InnerText;
                        break;
                }
            }
            foreach (XmlNode child in node.ChildNodes)
                RecursiveNode(child);
        }

        private void SaveLoginParam()
        {
            if (string.IsNullOrEmpty(login_param_path))
                return;

            using (XmlTextWriter w = new XmlTextWriter(login_param_path, Encoding.UTF8))
            {
                w.Formatting = Formatting.Indented;
                w.WriteStartDocument();
                w.WriteStartElement("root");
                foreach (string key in login_param.Keys)
                {
                    w.WriteStartElement(key);
                    w.WriteString(login_param[key]);
                    w.WriteEndElement();
                }
                w.Close();
            }
        }

        private void gestureButton_Click(object sender, EventArgs e)
        {

        }
    }
}
