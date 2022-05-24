#if (UNITY_EDITOR)
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace TextureUtility
{
    [Serializable]
    public struct TextureFormatData
    {
        public TextureFormatData(TextureFormat textureFormat, ColorWriteMask[] colorWriteMasks)
        {
            _textureFormat = textureFormat;
            _colorWriteMasks = colorWriteMasks;
        }

        private TextureFormat _textureFormat;
        public TextureFormat textureFormat { get { return _textureFormat; } }

        private ColorWriteMask[] _colorWriteMasks;
        public ColorWriteMask[] colorWriteMasks { get { return _colorWriteMasks; } }
        public int channelCount { get { return _colorWriteMasks.Length; } }
    }
}
#endif