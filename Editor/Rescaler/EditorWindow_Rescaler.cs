#if (UNITY_EDITOR)

using UnityEngine;
using UnityEngine.Rendering;

using UnityEditor;

using System.IO;

namespace TextureUtility
{
    public class EditorWindow_Texture2D_Rescaler : EditorWindow
    {
        #region Variables
        #region GUIContent
        private GUIContent _previewTextureTransparencyToggle = new GUIContent("T", "Preview Alpha Transparency");
        #endregion
        #region Source Texture2D
        private Texture2D _sourceTexture2D;
        #endregion
        #region Texture2D
        private Texture2D _texture2D;
        private Rect _previewRect;
        #endregion
        #region RenderTexture
        private RenderTexture _renderTexture;
        private RenderTextureFormat _renderTextureFormat = RenderTextureFormat.ARGB32;
        #endregion
        #region Resolution
        private Vector2Int _resolution = new Vector2Int(1024, 1024);
        #endregion
        #region TextureFormat
        private TextureFormat _textureFormat = TextureFormat.RGBA32;
        public TextureFormat textureFormat { get { return _textureFormat; } }
        private TextureFormat[] _textureFormats = new TextureFormat[]
        {
        TextureFormat.RGBA32
        };
        #endregion
        #region TextureFormatOptions
        private int _textureFormatOption = 0;
        private GUIContent[] _textureFormatOptions = new GUIContent[]
        {
        new GUIContent("RGBA32")
        };
        #endregion
        #region ColorWriteMask
        private bool _previewTextureTransparent = false;
        private int _colorWriteMask = (int)(ColorWriteMask.Red | ColorWriteMask.Green | ColorWriteMask.Blue);
        #endregion
        #region ComputeShader
        private ComputeShader _computeShader;
        private int _kernel_HorizontalLerp;
        private int _kernel_GroupAverage;
        private int _kernel_Default;
        private int _kernel_Bicubic;
        private string _computeShaderPath = "Packages/com.textureutilities.editor/Editor/Rescaler/ComputeShader/CS_Rescaler.compute";
        #endregion
        #region SavePath
        private string _savePath = "Texture2D";
        private GUIContent _GUILayout_SavePath = new GUIContent("Save as:", "Save Path from: 'Assets/...'");
        #endregion
        #region ExportFormats
        private enum ExportFormats { EXR, JPG, PNG, TGA };
        private ExportFormats _exportFormat = ExportFormats.PNG;
        #endregion
        #region Algorithms
        private int _selected = 0;
        private GUIContent[] _rescaleAlgorithms = new GUIContent[]
        {
            new GUIContent("Default", "Replace with nearest Pixel"),
            new GUIContent("Bilinear", "Linear Interpolation between 2x2 Pixelgrid"),
            new GUIContent("Blend", "Average from Pixel Group Size of Rescale Factor")
            //new GUIContent("Bicubic", "Bicubic interpolation in nearest 4x4 Pixelgrid")
        };
        #endregion
        #endregion

        #region MenuItem
        [MenuItem("Tools/Texture2D/Rescaler")]
        static void ShowEditorWindow()
        {
            EditorWindow_Texture2D_Rescaler _editorWindow = (EditorWindow_Texture2D_Rescaler)EditorWindow.GetWindow(typeof(EditorWindow_Texture2D_Rescaler));           // Find existing Editor Window or create new one
            _editorWindow.Show();

            _editorWindow.titleContent = new GUIContent("Texture2D Rescaler");

            _editorWindow._computeShader = (ComputeShader)AssetDatabase.LoadAssetAtPath(                                                                                // ComputeShader Reference
                _editorWindow._computeShaderPath,
                typeof(ComputeShader)
            );

            _editorWindow._kernel_Default = _editorWindow._computeShader.FindKernel("Kernel_Default");
            _editorWindow._kernel_HorizontalLerp = _editorWindow._computeShader.FindKernel("Kernel_Bilinear");
            _editorWindow._kernel_GroupAverage = _editorWindow._computeShader.FindKernel("Kernel_Blend");
            _editorWindow._kernel_Bicubic = _editorWindow._computeShader.FindKernel("Kernel_Bicubic");

            _editorWindow._renderTexture = new RenderTexture(
                _editorWindow._resolution.x,
                _editorWindow._resolution.y,
                0,
                RenderTextureFormat.ARGB32
            );
            _editorWindow._renderTexture.enableRandomWrite = true;
            _editorWindow._renderTexture.Create();

            _editorWindow._texture2D = new Texture2D(
                _editorWindow._resolution.x,
                _editorWindow._resolution.y,
                TextureFormat.ARGB32,
                false
            );
            _editorWindow._texture2D.Apply();
        }
        #endregion

        #region OnGUI
        void OnGUI()
        {
            if (_computeShader == null)
            {
                _computeShader = (ComputeShader)AssetDatabase.LoadAssetAtPath(
                    _computeShaderPath,
                    typeof(ComputeShader)
                );
            }
            if (_texture2D == null)
            {
                _texture2D = new Texture2D(
                    _resolution.x,
                    _resolution.y,
                    TextureFormat.ARGB32,
                    false
                );
                _texture2D.Apply();
            }
            if (_renderTexture == null)
            {
                _renderTexture = new RenderTexture(
                    _resolution.x,
                    _resolution.y,
                    0,
                    RenderTextureFormat.ARGB32
                );
                _renderTexture.enableRandomWrite = true;
                _renderTexture.Create();
            }


            #region Source Texture
            EditorGUILayout.LabelField("Source Texture", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(                                                                                                                                 // Texture Info
                "Texture2D\n" +
                (_sourceTexture2D == null ? "Name" : _sourceTexture2D.name) + "\n" +                                                                                    // Texture2D Name
                (_sourceTexture2D == null ? "Resolution" : _sourceTexture2D.width.ToString() + "x" + _sourceTexture2D.height.ToString()) + "px\n" +                     // Texture2D Resolution
                (_sourceTexture2D == null ? "Format" : _sourceTexture2D.format),                                                                                        // Texture2D Format
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 74), 
                GUILayout.Height(64)
            );
            EditorGUI.BeginChangeCheck();
            _sourceTexture2D = (Texture2D)EditorGUILayout.ObjectField(
                    _sourceTexture2D,                                                                                                                                   // Texture2D
                    typeof(Texture2D),
                    false,
                    GUILayout.Width(64),
                    GUILayout.Height(64)
                );
            if (EditorGUI.EndChangeCheck())
            {
                if (_sourceTexture2D != null)
                {
                    _computeShader.SetTexture(_kernel_HorizontalLerp, "_sourceTexture2D", _sourceTexture2D);
                    this.Dispatch();
                }
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region Preview Texture
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
            _previewRect = new Rect(
                0f,
                GUILayoutUtility.GetLastRect().y + GUILayoutUtility.GetLastRect().height,
                EditorGUIUtility.currentViewWidth,
                EditorGUIUtility.currentViewWidth
            );
            EditorGUILayout.Space(_previewRect.height);

            if (_previewTextureTransparent)                                                                                                                             // Draw Texture Transparent
            {
                EditorGUI.DrawTextureTransparent(
                    _previewRect,
                    _texture2D,
                    ScaleMode.StretchToFill,
                    1f,
                    0,
                    (ColorWriteMask)_colorWriteMask
                );
            }
            else                                                                                                                                                        // Draw Texture Alpha
            {
                if ((_colorWriteMask & (int)ColorWriteMask.Alpha) == (int)ColorWriteMask.Alpha)
                {
                    EditorGUI.DrawTextureAlpha(
                        _previewRect,
                        _texture2D,
                        ScaleMode.ScaleToFit,
                        1f
                    );
                }
                else
                {
                    EditorGUI.DrawPreviewTexture(
                        _previewRect,
                        _texture2D,
                        null,
                        ScaleMode.ScaleToFit,
                        1f,
                        0,
                        (ColorWriteMask)_colorWriteMask
                        );
                }
            }

            EditorGUILayout.BeginHorizontal();
            
            if ((_colorWriteMask & 0x08) != 0x08)
            {
                GUI.backgroundColor = Color.gray;
            }                                                                                                                    // Red
            if (GUILayout.Button("Red"))
            {
                if ((_colorWriteMask & 0x08) == 0x08)
                {
                    _colorWriteMask &= ~(1 << 3);
                }
                else
                {
                    _colorWriteMask |= (1 << 3);
                    if (!_previewTextureTransparent)
                    {
                        _colorWriteMask &= ~(1 << 0);
                    }
                }
            }
            GUI.backgroundColor = Color.white;

            if ((_colorWriteMask & 0x04) != 0x04)
            {
                GUI.backgroundColor = Color.gray;
            }                                                                                                                    // Green
            if (GUILayout.Button("Green"))
            {
                if ((_colorWriteMask & 0x04) == 0x04)
                {
                    _colorWriteMask &= ~(1 << 2);
                }
                else
                {
                    _colorWriteMask |= (1 << 2);
                    if (!_previewTextureTransparent)
                    {
                        _colorWriteMask &= ~(1 << 0);
                    }
                }
            }
            GUI.backgroundColor = Color.white;

            if ((_colorWriteMask & 0x02) != 0x02)
            {
                GUI.backgroundColor = Color.gray;
            }                                                                                                                    // Blue
            if (GUILayout.Button("Blue"))
            {
                if ((_colorWriteMask & 0x02) == 0x02)
                {
                    _colorWriteMask &= ~(1 << 1);
                }
                else
                {
                    _colorWriteMask |= (1 << 1);
                    if (!_previewTextureTransparent)
                    {
                        _colorWriteMask &= ~(1 << 0);
                    }
                }
            }
            GUI.backgroundColor = Color.white;

            if ((_colorWriteMask & 0x01) != 0x01)
            {
                GUI.backgroundColor = Color.gray;
            }                                                                                                                    // Alpha
            if (GUILayout.Button("Alpha"))
            {
                if ((_colorWriteMask & 0x01) == 0x01)
                {
                    _colorWriteMask &= ~(1 << 0);
                    if (!_previewTextureTransparent)
                    {
                        _colorWriteMask |= 1 << 3;
                        _colorWriteMask |= 1 << 2;
                        _colorWriteMask |= 1 << 1;
                    }
                }
                else
                {
                    _colorWriteMask |= (1 << 0); if (!_previewTextureTransparent)
                    {
                        _colorWriteMask &= ~(1 << 3);
                        _colorWriteMask &= ~(1 << 2);
                        _colorWriteMask &= ~(1 << 1);
                    }
                }
            }
            GUI.backgroundColor = Color.white;

            if (!_previewTextureTransparent)
            {
                GUI.backgroundColor = Color.gray;
            }                                                                                                                         // Transparent
            if (GUILayout.Button(_previewTextureTransparencyToggle, GUILayout.Width(19), GUILayout.Height(19)))
            {
                _previewTextureTransparent = !_previewTextureTransparent;
                if (!_previewTextureTransparent)
                {
                    _colorWriteMask |= 1 << 3;
                    _colorWriteMask |= 1 << 2;
                    _colorWriteMask |= 1 << 1;
                    _colorWriteMask &= ~(1 << 0);
                }
                else
                {
                    _colorWriteMask |= 1 << 3;
                    _colorWriteMask |= 1 << 2;
                    _colorWriteMask |= 1 << 1;
                    _colorWriteMask |= 1 << 0;
                }
            };
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
            #endregion

            #region Settings
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Algorithm:", GUILayout.Width(64));                                                                                              // Algorithm
            EditorGUI.BeginChangeCheck();
            _selected = EditorGUILayout.Popup(_selected, _rescaleAlgorithms);
            if (EditorGUI.EndChangeCheck())
            {
                this.Dispatch();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Resolution:", GUILayout.Width(64));                                                                                             // Resolution
            EditorGUI.BeginChangeCheck();
            _resolution = EditorGUILayout.Vector2IntField("", _resolution);
            _resolution.x = Mathf.Max(_resolution.x, 1);
            _resolution.y = Mathf.Max(_resolution.y, 1);
            if (EditorGUI.EndChangeCheck())
            {
                _renderTexture = new RenderTexture(
                    _resolution.x,
                    _resolution.y,
                    0,
                    _renderTextureFormat
                );
                _renderTexture.enableRandomWrite = true;
                _renderTexture.Create();

                _texture2D = new Texture2D(
                    _resolution.x,
                    _resolution.y,
                    _textureFormat,
                    false
                );
                _texture2D.Apply();

                this.Dispatch();
            }
            EditorGUI.BeginChangeCheck();
            _textureFormatOption = EditorGUILayout.Popup(_textureFormatOption, _textureFormatOptions);
            if (EditorGUI.EndChangeCheck())
            {
                _textureFormat = _textureFormats[_textureFormatOption];
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_GUILayout_SavePath, GUILayout.Width(64));
            _savePath = EditorGUILayout.TextField(_savePath);
            _exportFormat = (ExportFormats)EditorGUILayout.EnumPopup(_exportFormat, GUILayout.Width(48));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Save"))
            {
                RenderTexture.active = _renderTexture;                                                                                                                  // Set RenderTexture active
                _texture2D.ReadPixels(new Rect(0f, 0f, _resolution.x, _resolution.y), 0, 0, false);
                RenderTexture.active = null;
                _texture2D.Apply();
                byte[] _bytes;
                string _fileExtension;
                switch (_exportFormat)
                {
                    case ExportFormats.EXR:
                        _bytes = _texture2D.EncodeToEXR();
                        _fileExtension = ".exr";
                        break;
                    case ExportFormats.PNG:
                        _bytes = _texture2D.EncodeToPNG();
                        _fileExtension = ".png";
                        break;
                    case ExportFormats.JPG:
                        _bytes = _texture2D.EncodeToJPG();
                        _fileExtension = ".jpg";
                        break;
                    case ExportFormats.TGA:
                        _bytes = _texture2D.EncodeToTGA();
                        _fileExtension = ".tga";
                        break;
                    default:
                        return;
                }
                File.WriteAllBytes("Assets/" + _savePath + _fileExtension, _bytes);
                AssetDatabase.Refresh();
            }
            #endregion
        }
        #endregion

        #region Dispatch
        private void Dispatch()
        {
            switch (_selected)
            {
                case 0:                                                                                                                                                 // Default
                    _computeShader.SetVector(
                        "_step",
                        new Vector2(
                            (float)_sourceTexture2D.width / _resolution.x,
                            (float)_sourceTexture2D.height / _resolution.y
                        )
                    );
                    _computeShader.SetTexture(_kernel_Default, "_sourceTexture2D", _sourceTexture2D);
                    _computeShader.SetTexture(_kernel_Default, "_renderTexture", _renderTexture);
                    _computeShader.Dispatch(_kernel_Default, _resolution.x, _resolution.y, 1);
                    Graphics.CopyTexture(_renderTexture, _texture2D);
                    break;

                case 1:                                                                                                                                                 // Bilinear
                    _computeShader.SetVector(
                        "_step",
                        new Vector2(
                            (float)_sourceTexture2D.width / _resolution.x,
                            (float)_sourceTexture2D.height / _resolution.y
                        )
                    );
                    _computeShader.SetTexture(_kernel_HorizontalLerp, "_sourceTexture2D", _sourceTexture2D);
                    _computeShader.SetTexture(_kernel_HorizontalLerp, "_renderTexture", _renderTexture);
                    _computeShader.Dispatch(_kernel_HorizontalLerp, _resolution.x, _resolution.y, 1);
                    Graphics.CopyTexture(_renderTexture, _texture2D);
                    break;

                case 2:                                                                                                                                                 // Blend
                    _computeShader.SetVector(
                        "_step",
                        new Vector2(
                            (float)_sourceTexture2D.width / _resolution.x,
                            (float)_sourceTexture2D.height / _resolution.y
                        )
                    );
                    _computeShader.SetVector(
                        "_groupSize",
                        new Vector2(
                            Mathf.RoundToInt((float)_sourceTexture2D.width / _resolution.x),
                            Mathf.RoundToInt((float)_sourceTexture2D.height / _resolution.y)
                        )
                    );
                    _computeShader.SetTexture(_kernel_GroupAverage, "_sourceTexture2D", _sourceTexture2D);
                    _computeShader.SetTexture(_kernel_GroupAverage, "_renderTexture", _renderTexture);
                    _computeShader.Dispatch(_kernel_GroupAverage, _resolution.x, _resolution.y, 1);
                    Graphics.CopyTexture(_renderTexture, _texture2D);
                    break;

                case 3:// Bicubic
                    _computeShader.SetVector(
                        "_step",
                        new Vector2(
                            (float)_sourceTexture2D.width / _resolution.x,
                            (float)_sourceTexture2D.height / _resolution.y
                        )
                    );
                    _computeShader.SetTexture(_kernel_Bicubic, "_sourceTexture2D", _sourceTexture2D);
                    _computeShader.SetTexture(_kernel_Bicubic, "_renderTexture", _renderTexture);
                    _computeShader.Dispatch(_kernel_Bicubic, _resolution.x, _resolution.y, 1);
                    Graphics.CopyTexture(_renderTexture, _texture2D);

                    break;

                default: break;
            }
        }
        #endregion
    }
}
#endif