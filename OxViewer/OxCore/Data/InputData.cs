using System;
using System.Collections.Generic;
using System.Text;

namespace OxCore.Data
{
    public class InputData
    {
        private bool[] keys = new bool[(int)KeyType.CODES_COUNT];
        private bool[] back = new bool[(int)KeyType.CODES_COUNT];
        private int[] count = new int[(int)KeyType.CODES_COUNT];
        private bool[] mkeys = new bool[(int)MouseType.CODES_COUNT];
        private bool[] mback = new bool[(int)MouseType.CODES_COUNT];
        private int[] mcount = new int[(int)MouseType.CODES_COUNT];
        private float[] position = new float[2];
        private float delta;

        public float[] Position { get { return position; } }
        public float Delta { get { return delta; } }

        public int Count(KeyType key)
        {
            int code = (int)key;
            if (code >= keys.Length)
                return 0;

            return count[code];
        }

        public bool Press(KeyType key)
        {
            return Press(key, true);
        }

        private bool Press(KeyType key, bool current)
        {
            int code = (int)key;
            if (code >= keys.Length)
                return false;

            if (current)
                return keys[code];
            else
                return back[code];
        }

        public bool PressTrg(KeyType key)
        {
            return (Press(key, true) && !Press(key, false));
        }

        public bool ReleaseTrg(KeyType key)
        {
            return (!Press(key, true) && Press(key, false));
        }

        public int MCount(MouseType key)
        {
            int code = (int)key;
            if (code >= mkeys.Length)
                return 0;

            return mcount[code];
        }

        public bool MPress(MouseType key)
        {
            return MPress(key, true);
        }

        private bool MPress(MouseType key, bool current)
        {
            int code = (int)key;
            if (code >= mkeys.Length)
                return false;

            if (current)
                return mkeys[code];
            else
                return mback[code];
        }

        public bool MPressTrg(MouseType key)
        {
            return (MPress(key, true) && !MPress(key, false));
        }

        public bool MReleaseTrg(MouseType key)
        {
            return (!MPress(key, true) && MPress(key, false));
        }

        public void Backup()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                back[i] = keys[i];
                count[i] = keys[i] ? ++count[i] : 0;
            }

            for (int i = 0; i < mkeys.Length; i++)
            {
                mback[i] = mkeys[i];
                mcount[i] = mkeys[i] ? ++mcount[i] : 0;
            }

            delta = 0;
        }

        public void Reset()
        {
            for (int i = 0; i < keys.Length; i++)
                keys[i] = false;

            for (int i = 0; i < mkeys.Length; i++)
                mkeys[i] = false;

            delta = 0;
        }

        public void SetKey(KeyType key, bool press)
        {
            int code = (int)key;
            if (code >= keys.Length)
                return;

            keys[code] = press;
        }

        public void SetMKey(MouseType key, bool press)
        {
            int code = (int)key;
            if (code >= mkeys.Length)
                return;

            mkeys[code] = press;
        }

        public void SetMPosition(float x, float y)
        {
            position[0] = x;
            position[1] = y;
        }

        public void SetMDelta(float delta)
        {
            this.delta = delta;
        }
    }

    #region KeyType
    public enum MouseType
    {
        LButton,
        MButton,
        RButton,
        CODES_COUNT
    }

    // Copy from IrrlichtNET
    public enum KeyType
    {
        LButton = 1,
        RButton = 2,
        Cancel = 3,
        MButton = 4,
        XButton1 = 5,
        XButton2 = 6,
        Back = 8,
        Tab = 9,
        Clear = 12,
        Return = 13,
        Shift = 16,
        Control = 17,
        Menu = 18,
        Pause = 19,
        Capital = 20,
        Hangul = 21,
        Hanguel = 21,
        Kana = 21,
        Junja = 23,
        Final = 24,
        Hanja = 25,
        Kanji = 25,
        Escape = 27,
        Convert = 28,
        NonConvert = 29,
        Accept = 30,
        ModeChange = 31,
        Space = 32,
        Prior = 33,
        Next = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        Snapshot = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        Key_0 = 48,
        Key_1 = 49,
        Key_2 = 50,
        Key_3 = 51,
        Key_4 = 52,
        Key_5 = 53,
        Key_6 = 54,
        Key_7 = 55,
        Key_8 = 56,
        Key_9 = 57,
        Key_A = 65,
        Key_B = 66,
        Key_C = 67,
        Key_D = 68,
        Key_E = 69,
        Key_F = 70,
        Key_G = 71,
        Key_H = 72,
        Key_I = 73,
        Key_J = 74,
        Key_K = 75,
        Key_L = 76,
        Key_M = 77,
        Key_N = 78,
        Key_O = 79,
        Key_P = 80,
        Key_Q = 81,
        Key_R = 82,
        Key_S = 83,
        Key_T = 84,
        Key_U = 85,
        Key_V = 86,
        Key_W = 87,
        Key_X = 88,
        Key_Y = 89,
        Key_Z = 90,
        LWin = 91,
        RWin = 92,
        Apps = 93,
        Sleep = 95,
        Numpad_0 = 96,
        Numpad_1 = 97,
        Numpad_2 = 98,
        Numpad_3 = 99,
        Numpad_4 = 100,
        Numpad_5 = 101,
        Numpad_6 = 102,
        Numpad_7 = 103,
        Numpad_8 = 104,
        Numpad_9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        NumLock = 144,
        Scroll = 145,
        LShift = 160,
        RShift = 161,
        LControl = 162,
        RControl = 163,
        LMenu = 164,
        RMenu = 165,
        Plus = 187,
        Comma = 188,
        Minus = 189,
        Period = 190,
        Attn = 246,
        CrSel = 247,
        ExSel = 248,
        ErEOF = 249,
        Play = 250,
        Zoom = 251,
        PA1 = 253,
        OemClear = 254,
        CODES_COUNT = 255,
    }
    #endregion
}
