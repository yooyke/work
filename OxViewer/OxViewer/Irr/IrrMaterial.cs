using System;
using System.Collections.Generic;
using System.Text;

namespace OxViewer.Irr
{
    public class IrrMaterial
    {
        public MaterialType Type;
        public byte[] Ambient;
        public byte[] Diffuse;
        public byte[] Emissive;
        public byte[] Specular;
        public float Shininess;
        public float Param1;
        public float Param2;
        public string Texture1;
        public string Texture2;
        public string Texture3;
        public string Texture4;
        public bool Wireframe;
        public bool GouraudShading;
        public bool Lighting;
        public bool ZWriteEnable;
        public int ZBuffer;
        public bool BackfaceCulling;
        public bool FrontfaceCulling;
        public bool FogEnable;
        public bool NormalizeNormals;
        public bool BilinearFilter1;
        public bool BilinearFilter2;
        public bool BilinearFilter3;
        public bool BilinearFilter4;
        public bool TrilinearFilter1;
        public bool TrilinearFilter2;
        public bool TrilinearFilter3;
        public bool TrilinearFilter4;
        public bool AnisotropicFilter1;
        public bool AnisotropicFilter2;
        public bool AnisotropicFilter3;
        public bool AnisotropicFilter4;
        public ClampType TextureWrap1;
        public ClampType TextureWrap2;
        public ClampType TextureWrap3;
        public ClampType TextureWrap4;

        private List<string> assetList = new List<string>();

        public void AddAsset(string asset)
        {
            if (string.IsNullOrEmpty(asset) || assetList.Contains(asset))
                return;

            assetList.Add(asset);
        }

        public string[] GetAssets()
        {
            if (assetList.Count == 0)
                return null;

            return assetList.ToArray();
        }

        public static IrrMaterial Get(IrrData data)
        {
            IrrMaterial material = new IrrMaterial();
            Copy(data, material);

            return material;
        }

        public static void Copy(IrrData data, IrrMaterial material)
        {
            foreach (IrrData child in data.Children)
            {
                if (string.IsNullOrEmpty(child.Attr.Name))
                    continue;

                switch (child.Attr.Name.ToLower())
                {
                    case "type":
                        material.Type = EnumComverter.ToMaterialType(child.Attr.Value as string);
                        break;
                    case "ambient":
                        material.Ambient = child.Attr.Value as byte[];
                        break;
                    case "diffuse":
                        material.Diffuse = child.Attr.Value as byte[];
                        break;
                    case "emissive":
                        material.Emissive = child.Attr.Value as byte[];
                        break;
                    case "specular":
                        material.Specular = child.Attr.Value as byte[];
                        break;
                    case "shininess":
                        material.Shininess = (float)child.Attr.Value;
                        break;
                    case "param1":
                        material.Param1 = (float)child.Attr.Value;
                        break;
                    case "param2":
                        material.Param2 = (float)child.Attr.Value;
                        break;
                    case "texture1":
                        material.AddAsset(material.Texture1 = child.Attr.Value as string);
                        break;
                    case "texture2":
                        material.AddAsset(material.Texture2 = child.Attr.Value as string);
                        break;
                    case "texture3":
                        material.AddAsset(material.Texture3 = child.Attr.Value as string);
                        break;
                    case "texture4":
                        material.AddAsset(material.Texture4 = child.Attr.Value as string);
                        break;
                    case "wireframe":
                        material.Wireframe = (bool)child.Attr.Value;
                        break;
                    case "gouraudshading":
                        material.GouraudShading = (bool)child.Attr.Value;
                        break;
                    case "lighting":
                        material.Lighting = (bool)child.Attr.Value;
                        break;
                    case "zwriteenable":
                        material.ZWriteEnable = (bool)child.Attr.Value;
                        break;
                    case "zbuffer":
                        material.ZBuffer = (int)child.Attr.Value;
                        break;
                    case "backfaceculling":
                        material.BackfaceCulling = (bool)child.Attr.Value;
                        break;
                    case "frontfaceculling":
                        material.FrontfaceCulling = (bool)child.Attr.Value;
                        break;
                    case "fogenable":
                        material.FogEnable = (bool)child.Attr.Value;
                        break;
                    case "normalizenormals":
                        material.NormalizeNormals = (bool)child.Attr.Value;
                        break;
                    case "bilinearfilter1":
                        material.BilinearFilter1 = (bool)child.Attr.Value;
                        break;
                    case "bilinearfilter2":
                        material.BilinearFilter2 = (bool)child.Attr.Value;
                        break;
                    case "bilinearfilter3":
                        material.BilinearFilter3 = (bool)child.Attr.Value;
                        break;
                    case "bilinearfilter4":
                        material.BilinearFilter4 = (bool)child.Attr.Value;
                        break;
                    case "trilinearfilter1":
                        material.TrilinearFilter1 = (bool)child.Attr.Value;
                        break;
                    case "trilinearfilter2":
                        material.TrilinearFilter2 = (bool)child.Attr.Value;
                        break;
                    case "trilinearfilter3":
                        material.TrilinearFilter3 = (bool)child.Attr.Value;
                        break;
                    case "trilinearfilter4":
                        material.TrilinearFilter4 = (bool)child.Attr.Value;
                        break;
                    case "anisotropicfilter1":
                        material.AnisotropicFilter1 = (bool)child.Attr.Value;
                        break;
                    case "anisotropicfilter2":
                        material.AnisotropicFilter2 = (bool)child.Attr.Value;
                        break;
                    case "anisotropicfilter3":
                        material.AnisotropicFilter3 = (bool)child.Attr.Value;
                        break;
                    case "anisotropicfilter4":
                        material.AnisotropicFilter4 = (bool)child.Attr.Value;
                        break;
                    case "texturewrap1":
                        material.TextureWrap1 = EnumComverter.ToClampType(child.Attr.Value as string);
                        break;
                    case "texturewrap2":
                        material.TextureWrap2 = EnumComverter.ToClampType(child.Attr.Value as string);
                        break;
                    case "texturewrap3":
                        material.TextureWrap3 = EnumComverter.ToClampType(child.Attr.Value as string);
                        break;
                    case "texturewrap4":
                        material.TextureWrap4 = EnumComverter.ToClampType(child.Attr.Value as string);
                        break;
                }
            }
        }
    }
}
