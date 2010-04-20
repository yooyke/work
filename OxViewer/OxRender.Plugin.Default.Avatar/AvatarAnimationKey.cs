using System;
using System.Collections.Generic;
using System.IO;
using IrrlichtNETCP;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxRender.Plugin.Default.Avatar
{
    public partial class Avatar
    {
        private class AK
        {
            private class Define
            {
                private bool loop;
                private bool interpolation;

                public bool Loop { get { return loop; } }
                public bool Interpolation { get { return interpolation; } }

                public Define(bool loop, bool interpolation)
                {
                    this.loop = loop;
                    this.interpolation = interpolation;
                }
            }

            private static Dictionary<string, string> keys;
            private static Dictionary<string, Define> defines;

            /// <summary>
            /// customize animation uuid list
            /// </summary>
            private static readonly string[] cList = new string[]{
                "C5829C0B-B82C-4f3d-9475-0826D48E5DB8",
                "C006EBC3-A40D-4a7d-B24D-A8323A198DF2",
                "046903EF-9358-45e1-BBDF-433CE99D3366",
                "290FE528-5128-4451-B9A7-39E761D8F60F",
                "43DEFB09-3996-41d2-ACBC-E1E217111396",
                "3BF9354B-8D8F-4d74-AB1B-98B867101285",
                "7B5960F3-5634-4e97-8225-E5DFCFC78654",
                "CF5A0D1D-8CCC-48ba-9D3F-81A4A50A7D11",
                "0F56F522-AB3F-44ae-B3A1-9425CF47DF7E",
                "2A356685-63C5-4454-9AC9-BAB87E37DA5A",
                "84B2F1B0-534C-4c51-82C0-0917BEA3C673",
                "20C1A61A-BC42-4202-9A53-9976BE26545E",
                "F7B995EA-9F96-4b7c-9C32-15BB50A17F73",
                "83A1870B-BAB5-4c2c-BAC9-558C7E22D0A9",
                "228B2569-8AD1-42f6-9C86-78DF237F0A86",
                "A377FEBB-2732-4e77-952D-A2F7326D6539",
                "81563482-4C13-4063-8986-1473D8AD2235",
                "2BA7296F-9F84-43fe-B078-C79047CF3085",
                "11D022B0-3851-4d77-BB85-08DFBBFC3BD4",
                "3DDFE90E-A50A-454d-8B87-5B48AB20E29D",
                "E980C815-0CAC-4ec4-9C19-3071085C4804",
                                                         };

            private static void Initialize()
            {
                keys = new Dictionary<string, string>();
                keys.Add("2408fe9e-df1d-1d7d-f4ff-1384fa7b350f", AnimationType.Standing.ToString().ToLower());
                keys.Add("6ed24bd8-91aa-4b12-ccc7-c97c857ab4e0", AnimationType.Walking.ToString().ToLower());
                keys.Add("05ddbff8-aaa9-92a1-2b74-8fe77a29b445", AnimationType.Running.ToString().ToLower());
                keys.Add("1a5fe8ac-a804-8a5d-7cbd-56bd83184568", AnimationType.SitStart.ToString().ToLower());

                defines = new Dictionary<string, Define>();
                defines.Add(AnimationType.Standing.ToString().ToLower(), new Define(true, true));
                defines.Add(AnimationType.Walking.ToString().ToLower(), new Define(true, true));
                defines.Add(AnimationType.Running.ToString().ToLower(), new Define(true, true));
                defines.Add(AnimationType.SitStart.ToString().ToLower(), new Define(false, false));

                for (int i = 0; i < cList.Length; i++)
                    keys.Add(cList[i], string.Format("customize{0}", i));
            }

            public static bool IsLoopAnimation(string key)
            {
                key = key.ToLower();
                if (!ContainDefines(key))
                    return false;

                return defines[key].Loop;
            }

            public static bool IsInterpolationAnimation(string key)
            {
                key = key.ToLower();
                if (!ContainDefines(key))
                    return false;

                return defines[key].Interpolation;
            }

            private static bool ContainDefines(string key)
            {
                if (string.IsNullOrEmpty(key))
                    return false;

                if (!defines.ContainsKey(key))
                    return false;

                return true;
            }

            public static string GetNameFromKey(string animationID)
            {
                if (string.IsNullOrEmpty(animationID))
                    return string.Empty;

                if (keys == null)
                    Initialize();

                if (keys.ContainsKey(animationID))
                    return keys[animationID];

                return string.Empty;
            }

            public static string GetKeyFromName(string animationName)
            {
                if (string.IsNullOrEmpty(animationName))
                    return string.Empty;

                animationName = animationName.ToLower();

                if (keys == null)
                    Initialize();

                if (keys.ContainsValue(animationName))
                {
                    foreach (string key in keys.Keys)
                    {
                        if (keys[key] == animationName)
                            return key;
                    }
                }

                return string.Empty;
            }

            public static string GetCKeyFromIndex(int index)
            {
                if (cList.Length >= index)
                    return string.Empty;

                return cList[index];
            }
        }
    }
}
