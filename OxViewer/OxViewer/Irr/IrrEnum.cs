using System;
using System.Collections.Generic;
using System.Text;

namespace OxViewer.Irr
{
    public enum MaterialType
    {
        Solid = 0,
        Solid2Layer = 1,
        Lightmap = 2,
        LightmapAdd = 3,
        LightmapM2 = 4,
        LightmapM4 = 5,
        LightmapLighting = 6,
        LightmapLightingM2 = 7,
        LightmapLightingM4 = 8,
        DetailMap = 9,
        SphereMap = 10,
        Reflection2Layer = 11,
        TransparentAddColor = 12,
        TransparentAlphaChannel = 13,
        TransparentAlphaChannelRef = 14,
        TransparentVertexAlpha = 15,
        TransparentReflection2Layer = 16,
        NormalMapSolid = 17,
        NormalMapTransparentAddColor = 18,
        NormalMapTransparentVertexAlpha = 19,
        ParallaxMapSolid = 20,
        ParallaxMapTransparentAddColor = 21,
        ParallaxMapTransparentVertexAlpha = 22,
        OneTextureBlend = 23,
    }

    public enum ClampType
    {
        Repeat,
    }

    public enum CullingType
    {
        Box,
    }

    public static class EnumComverter
    {
        public static MaterialType ToMaterialType(string value)
        {
            switch (value)
            {
                case "solid":
                    return MaterialType.Solid;
                case "solid_2layer":
                    return MaterialType.Solid2Layer;
                case "lightmap":
                    return MaterialType.Lightmap;
                case "ligthmap_add":
                    return MaterialType.LightmapAdd;
                case "ligthmap_m2":
                    return MaterialType.LightmapM2;
                case "ligthmap_m4":
                    return MaterialType.LightmapM4;
                case "ligthmap_light":
                    return MaterialType.LightmapLighting;
                case "ligthmap_light_m2":
                    return MaterialType.LightmapLightingM2;
                case "ligthmap_light_m4":
                    return MaterialType.LightmapLightingM4;
                case "detail_map":
                    return MaterialType.DetailMap;
                case "sphere_map":
                    return MaterialType.SphereMap;
                case "reflection_2layer":
                    return MaterialType.Reflection2Layer;
                case "trans_add":
                    return MaterialType.TransparentAddColor;
                case "trans_alphach":
                    return MaterialType.TransparentAlphaChannel;
                case "trans_alphach_ref":
                    return MaterialType.TransparentAlphaChannelRef;
                case "trans_reflection_2layer":
                    return MaterialType.TransparentReflection2Layer;
                case "normalmap_solid":
                    return MaterialType.NormalMapSolid;
                case "normalmap_trans_add":
                    return MaterialType.NormalMapTransparentAddColor;
                case "normalmap_trans_vertexalpha":
                    return MaterialType.NormalMapTransparentVertexAlpha;
                case "parallaxmap_solid":
                    return MaterialType.ParallaxMapSolid;
                case "parallaxmap_trans_add":
                    return MaterialType.ParallaxMapTransparentAddColor;
                case "parallaxmap_trans_vertexalpha":
                    return MaterialType.ParallaxMapTransparentVertexAlpha;
                case "onetexture_blend":
                    return MaterialType.OneTextureBlend;
            }

            return MaterialType.Solid;
        }

        public static ClampType ToClampType(string value)
        {
            switch (value)
            {
                case "texture_clamp_repeat":
                    return ClampType.Repeat;
            }

            return ClampType.Repeat;
        }
    }
}
