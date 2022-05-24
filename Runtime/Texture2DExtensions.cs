using UnityEngine;
using Unity.Collections;
using UnityEngine.Networking;

namespace TextureUtility
{
    public static class Texture2DExtensions
    {
        #region Get Readable Texture2D
        /// <summary>
        /// Creates a readable Texture2D from a Texture2D
        /// </summary>
        /// <param name="texture2D"></param>
        /// <returns></returns>
        public static Texture2D GetReadable(this Texture2D texture2D)
        {
            RenderTexture _activeRenderTexture = RenderTexture.active;                                                                                                  // Cache active RenderTexture
            RenderTexture _temporaryRenderTexture = RenderTexture.GetTemporary(                                                                                         // Get Temporary RenderTexture
                texture2D.width,
                texture2D.height
            );
            Graphics.Blit(texture2D, _temporaryRenderTexture);
            RenderTexture.active = _temporaryRenderTexture;                                                                                                             // Set New RenderTexture to active

            Texture2D _outputTexture2D = new Texture2D(                                                                                                                 // Create output Texture2D
                texture2D.width,
                texture2D.height
            );
            _outputTexture2D.ReadPixels(                                                                                                                                // Read from active RenderTexture
                new Rect(
                    0,
                    0,
                    texture2D.width,
                    texture2D.height
                ),
                0,
                0
            );
            _outputTexture2D.Apply();                                                                                                                                   // Apply OutputTexture2D

            RenderTexture.active = _activeRenderTexture;                                                                                                                // Reset active RenderTexture
            RenderTexture.ReleaseTemporary(_temporaryRenderTexture);                                                                                                    // Release Temporary RenderTexture

            return _outputTexture2D;
        }
        #endregion
    }
}
