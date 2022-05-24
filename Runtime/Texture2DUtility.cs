using UnityEngine;

namespace TextureUtility
{
    public static class Texture2DUtility
    {
        #region Texture2DArray
        #region Create Texture2DArray
        public static Texture2DArray CreateTexture2DArray(Texture2D[] texture2Ds, int width, int height)
        {
            if (texture2Ds.Length == 0)
            {
                Debug.LogError("Texture2DArray input textures must not be null");
                return null;
            }

            Texture2DArray _texture2DArray = new Texture2DArray(
                width,
                height,
                texture2Ds.Length,
                TextureFormat.RGBA32,
                true,
                false
            );
            _texture2DArray.filterMode = FilterMode.Bilinear;
            _texture2DArray.wrapMode = TextureWrapMode.Repeat;

            for (int _i = 0; _i < texture2Ds.Length; _i++)
            {
                if (texture2Ds[_i] == null)
                {
                    Debug.LogError("Texture2DArray input textures at index " + _i + "must not be null");
                    return null;
                }
                if ((texture2Ds[_i].width != width) || (texture2Ds[_i].height != height))
                {
                    Debug.LogError("Texture2DArray input textures must match Texture2DArray resolution");
                    return null;
                }
                _texture2DArray.SetPixels(texture2Ds[_i].GetReadable().GetPixels(0), _i, 0);
            }
            _texture2DArray.Apply();                                                                                                                                    // Apply Texture2DArray

            return _texture2DArray;                                                                                                                                     // Return
        }
        #endregion
        #endregion
    }
}