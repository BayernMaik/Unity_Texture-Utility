#if (UNITY_EDITOR)

using UnityEngine;
using UnityEngine.Rendering;

namespace TextureUtility
{
    public static class TextureFormats
    {
        public static TextureFormatData[] data = new TextureFormatData[] {
        new TextureFormatData(                                                                                                                                          // 1 - TextureFormat.Alpha8
            TextureFormat.Alpha8,
            new ColorWriteMask[] { ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 2 - TextureFormat.ARGB4444
            TextureFormat.ARGB4444,
            new ColorWriteMask[] { ColorWriteMask.Alpha, ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 3 - TextureFormat.RGB24
            TextureFormat.RGB24,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 4 - TextureFormat.RGBA32
            TextureFormat.RGBA32,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 5 - TextureFormat.ARGB32
            TextureFormat.ARGB32,
            new ColorWriteMask[] { ColorWriteMask.Alpha, ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 7 - TextureFormat.ARGB32
            TextureFormat.RGB565,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 9 - TextureFormat.R16
            TextureFormat.R16,
            new ColorWriteMask[] { ColorWriteMask.Red }
        ),
        new TextureFormatData(                                                                                                                                          // 10 - TextureFormat.DXT1
            TextureFormat.DXT1,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 12 - TextureFormat.DXT5
            TextureFormat.DXT5,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 13 - TextureFormat.RGBA4444
            TextureFormat.RGBA4444,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 14 - TextureFormat.BGRA32
            TextureFormat.BGRA32,
            new ColorWriteMask[] { ColorWriteMask.Blue, ColorWriteMask.Green, ColorWriteMask.Red, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 15 - TextureFormat.RHalf
            TextureFormat.RHalf,
            new ColorWriteMask[] { ColorWriteMask.Red }
        ),
        new TextureFormatData(                                                                                                                                          // 16 - TextureFormat.RGHalf
            TextureFormat.RGHalf,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green }
        ),
        new TextureFormatData(                                                                                                                                          // 17 - TextureFormat.RGBAHalf
            TextureFormat.RGBAHalf,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 18 - TextureFormat.RFloat
            TextureFormat.RFloat,
            new ColorWriteMask[] { ColorWriteMask.Red }
        ),
        new TextureFormatData(                                                                                                                                          // 19 - TextureFormat.RGFloat
            TextureFormat.RGFloat,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green }
        ),
        new TextureFormatData(                                                                                                                                          // 20 - TextureFormat.RGBAFloat
            TextureFormat.RGBAFloat,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 21 - TextureFormat.YUY2
            TextureFormat.YUY2,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 22 - TextureFormat.RGB9e5Float
            TextureFormat.RGB9e5Float,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 24 - TextureFormat.BC6H
            TextureFormat.BC6H,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 25 - TextureFormat.BC7
            TextureFormat.BC7,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 26 - TextureFormat.BC4
            TextureFormat.BC4,
            new ColorWriteMask[] { ColorWriteMask.Red }
        ),
        new TextureFormatData(                                                                                                                                          // 27 - TextureFormat.BC5
            TextureFormat.BC5,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green }
        ),
        new TextureFormatData(                                                                                                                                          // 28 - TextureFormat.DXT1Crunched
            TextureFormat.DXT1Crunched,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 29 - TextureFormat.DXT5Crunched
            TextureFormat.DXT5Crunched,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 30 - TextureFormat.PVRTC_RGB2
            TextureFormat.PVRTC_RGB2,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 31 - TextureFormat.PVRTC_RGBA2
            TextureFormat.PVRTC_RGBA2,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 32 - TextureFormat.PVRTC_RGB4
            TextureFormat.PVRTC_RGB4,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 33 - TextureFormat.PVRTC_RGBA4
            TextureFormat.PVRTC_RGBA4,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 34 - TextureFormat.ETC_RGB4
            TextureFormat.ETC_RGB4,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 41 - TextureFormat.EAC_R
            TextureFormat.EAC_R,
            new ColorWriteMask[] { ColorWriteMask.Red }
        ),
        new TextureFormatData(                                                                                                                                          // 42 - TextureFormat.EAC_R_SIGNED
            TextureFormat.EAC_R_SIGNED,
            new ColorWriteMask[] { ColorWriteMask.Red }
        ),
        new TextureFormatData(                                                                                                                                          // 43 - TextureFormat.EAC_RG
            TextureFormat.EAC_RG,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green }
        ),
        new TextureFormatData(                                                                                                                                          // 44 - TextureFormat.EAC_RG_SIGNED
            TextureFormat.EAC_RG_SIGNED,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green }
        ),
        new TextureFormatData(                                                                                                                                          // 45 - TextureFormat.ETC2_RGB
            TextureFormat.ETC2_RGB,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 46 - TextureFormat.ETC2_RGBA1
            TextureFormat.ETC2_RGBA1,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 47 - TextureFormat.ETC2_RGBA8
            TextureFormat.ETC2_RGBA8,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 48 - TextureFormat.ASTC_4x4
            TextureFormat.ASTC_4x4,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 49 - TextureFormat.ASTC_5x5
            TextureFormat.ASTC_5x5,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 50 - TextureFormat.ASTC_6x6
            TextureFormat.ASTC_6x6,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 51 - TextureFormat.ASTC_8x8
            TextureFormat.ASTC_8x8,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 52 - TextureFormat.ASTC_10x10
            TextureFormat.ASTC_10x10,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 53 - TextureFormat.ASTC_12x12
            TextureFormat.ASTC_12x12,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 62 - TextureFormat.RG16
            TextureFormat.RG16,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green }
        ),
        new TextureFormatData(                                                                                                                                          // 63 - TextureFormat.R8
            TextureFormat.R8,
            new ColorWriteMask[] { ColorWriteMask.Red }
        ),
        new TextureFormatData(                                                                                                                                          // 64 - TextureFormat.ETC_RGB4Crunched
            TextureFormat.ETC_RGB4Crunched,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 65 - TextureFormat.ETC2_RGBA8Crunched
            TextureFormat.ETC2_RGBA8Crunched,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 66 - TextureFormat.ASTC_HDR_4x4
            TextureFormat.ASTC_HDR_4x4,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 67 - TextureFormat.ASTC_HDR_5x5
            TextureFormat.ASTC_HDR_5x5,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 68 - TextureFormat.ASTC_HDR_6x6
            TextureFormat.ASTC_HDR_6x6,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 69 - TextureFormat.ASTC_HDR_8x8
            TextureFormat.ASTC_HDR_8x8,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 70 - TextureFormat.ASTC_HDR_10x10
            TextureFormat.ASTC_HDR_10x10,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 71 - TextureFormat.ASTC_HDR_12x12
            TextureFormat.ASTC_HDR_12x12,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        ),
        new TextureFormatData(                                                                                                                                          // 72 - TextureFormat.RG32
            TextureFormat.RG32,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green }
        ),
        new TextureFormatData(                                                                                                                                          // 73 - TextureFormat.RGB48
            TextureFormat.RGB48,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue }
        ),
        new TextureFormatData(                                                                                                                                          // 74 - TextureFormat.RGBA64
            TextureFormat.RGBA64,
            new ColorWriteMask[] { ColorWriteMask.Red, ColorWriteMask.Green, ColorWriteMask.Blue, ColorWriteMask.Alpha }
        )
    };

        public static TextureFormatData GetData(TextureFormat textureFormat)
        {
            for (int _i = 0; _i < data.Length; _i++)
            {
                if (data[_i].textureFormat == textureFormat)
                {
                    return data[_i];
                }
            }
            return data[0];
        }
        public static int index(TextureFormat textureFormat)
        {
            return (int)textureFormat;
        }
        public static int BitIndex(ColorWriteMask colorWriteMask)
        {
            switch (colorWriteMask)
            {
                case ColorWriteMask.Red:
                    return 3;
                case ColorWriteMask.Green:
                    return 2;
                case ColorWriteMask.Blue:
                    return 1;
                case ColorWriteMask.Alpha:
                    return 0;
                default:
                    return -1;
            }
        }
    }
}
#endif