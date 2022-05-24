#if (UNITY_EDITOR)

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.Rendering;

namespace TextureUtility
{
    public class EditorWindow_Texture2D_ChannelPacker : EditorWindow
    {
        #region Variables
        #region GUIContent
        private GUIContent _previewTextureTransparencyToggle;
        private GUIContent _GUIContent_FormatPopup = new GUIContent("Format");
        private Vector2 _scrollPosition;
        #endregion
        #region Texture2D
        [SerializeField] private Texture2D _texture2D;
        private RenderTexture _renderTexture2D;
        #endregion
        #region TextureResolution
        private Vector2Int _resolution = new Vector2Int(1024, 1024);
        #endregion
        #region TextureFormatData
        [SerializeField] private TextureFormatData _textureFormatData;
        public TextureFormatData textureFormatData
        {
            get
            {
                return _textureFormatData;
            }
        }
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
        #region TexturePreview
        private bool _previewTextureTransparent = false;
        private int _colorWriteMask = (int)(ColorWriteMask.Red | ColorWriteMask.Green | ColorWriteMask.Blue);
        #endregion
        #region Layers
        [SerializeField] private List<EditorWindow_Texture2D_ChannelPacker_Layer> _layers;
        #endregion
        #region ComputeShader
        private string _assetPath = "Packages/com.textureutilities.editor/Editor/Channel Packer/ComputeShader/CS_ChannelPacker.compute";
        private ComputeShader _computeShader;
        private int _CS_ChannelPacker;
        private int _CS_ClearRenderTexture2D;
        #endregion
        #region SavePath
        private string _savePath = "Texture2D";
        #endregion
        #region ExportFormats
        private enum ExportFormats { EXR, JPG, PNG, TGA };
        private ExportFormats _exportFormat = ExportFormats.PNG;
        #endregion
        #endregion

        #region Menu Item
        [MenuItem("Tools/Texture2D/Channel Packer")]
        public static void ShowEditorWindow()
        {
            EditorWindow_Texture2D_ChannelPacker _editorWindow = (EditorWindow_Texture2D_ChannelPacker)EditorWindow.GetWindow(typeof(EditorWindow_Texture2D_ChannelPacker));                          // Find existing Editor Window or create new one
            _editorWindow.Show();                                                                                                                                       // Show Editor Window in Unity Editor

            _editorWindow.titleContent = new GUIContent("Texture2D Channel Packer");
            _editorWindow.minSize = new Vector2(89f * 4f + 512f, Screen.height * 0.4f);
            _editorWindow.maxSize = new Vector2(89f * 4f + 512f, Screen.height * 0.8f);

            // Create Texture2D
            if (_editorWindow._texture2D == null)
            {
                _editorWindow._texture2D = new Texture2D(
                    _editorWindow._resolution.x,
                    _editorWindow._resolution.y,
                    _editorWindow._textureFormat,
                    false
                );
                _editorWindow._texture2D.Apply();
            }
            // Create RenderTexture2D
            if (_editorWindow._renderTexture2D == null)
            {
                _editorWindow._renderTexture2D = new RenderTexture(
                    _editorWindow._resolution.x,
                    _editorWindow._resolution.y,
                    0,
                    RenderTextureFormat.ARGB32
                );
                _editorWindow._renderTexture2D.enableRandomWrite = true;
                _editorWindow._renderTexture2D.Create();
            }
            _editorWindow._computeShader = (ComputeShader)AssetDatabase.LoadAssetAtPath(_editorWindow._assetPath, typeof(ComputeShader));                               // ComputeShader Reference

            _editorWindow._CS_ChannelPacker = _editorWindow._computeShader.FindKernel("CS_ChannelPacker");
            _editorWindow._CS_ClearRenderTexture2D = _editorWindow._computeShader.FindKernel("CS_ClearRenderTexture2D");
            _editorWindow._computeShader.SetTexture(_editorWindow._CS_ChannelPacker, "_targetTexture2D", _editorWindow._renderTexture2D);
            _editorWindow._computeShader.SetTexture(_editorWindow._CS_ClearRenderTexture2D, "_targetTexture2D", _editorWindow._renderTexture2D);
            _editorWindow._computeShader.Dispatch(_editorWindow._CS_ClearRenderTexture2D, _editorWindow._resolution.x, _editorWindow._resolution.y, 1);
            Graphics.CopyTexture(_editorWindow._renderTexture2D, _editorWindow._texture2D);

            _editorWindow._previewTextureTransparencyToggle = new GUIContent(EditorGUIUtility.IconContent("d_Shaded").image, "Preview Alpha Transparency");

            _editorWindow._textureFormatData = TextureFormats.GetData(_editorWindow._textureFormat);
        }
        #endregion

        #region OnGUI
        private void OnGUI()
        {
            this.minSize = new Vector2(89f * 4f + 512f, 512f + 88f);
            this.maxSize = new Vector2(89f * 4f + 512f, 512f + 88f);

            #region Layers
            Rect _layersRect = new Rect(
                Vector2.zero,
                new Vector2(
                    89f * 4f,
                    512f + 88
                    )
                );
            GUILayout.BeginArea(_layersRect);
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            if (_layers == null)
            {
                _layers = new List<EditorWindow_Texture2D_ChannelPacker_Layer>();
            }
            for (int _i = 0; _i < _layers.Count; _i++)
            {
                _layers[_i].OnGUI(_layersRect);                                                                                                                             // Render Layers
            }
            if (GUILayout.Button("Add Layer"))                                                                                                                              // Add Layer Button
            {
                _layers.Add(new EditorWindow_Texture2D_ChannelPacker_Layer(this));
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
            #endregion

            Rect _previewRect = new Rect(
                new Vector2(
                        _layersRect.width,
                        0f
                    ),
                new Vector2(
                        512f,
                        Screen.height
                    )
                );
            GUILayout.BeginArea(_previewRect);
            // Preview
            EditorGUILayout.Space();
            Rect _rect = new Rect();
            _rect.position = GUILayoutUtility.GetLastRect().position;
            _rect.width = _previewRect.width;
            _rect.height = _previewRect.width;

            if (_previewTextureTransparent)                                                                                                                                 // Draw Texture Transparent
            {
                EditorGUI.DrawTextureTransparent(
                    _rect,
                    _texture2D,
                    ScaleMode.StretchToFill,
                    1f,
                    0,
                    (ColorWriteMask)_colorWriteMask
                );
            }
            else                                                                                                                                                            // Draw Texture Alpha
            {
                if ((_colorWriteMask & (int)ColorWriteMask.Alpha) == (int)ColorWriteMask.Alpha)
                {
                    EditorGUI.DrawTextureAlpha(
                        _rect,
                        _texture2D,
                        ScaleMode.ScaleToFit,
                        1f
                    );
                }
                else
                {
                    EditorGUI.DrawPreviewTexture(
                        _rect,
                        _texture2D,
                        null,
                        ScaleMode.ScaleToFit,
                        1f,
                        0,
                        (ColorWriteMask)_colorWriteMask
                        );
                }
            }
            EditorGUILayout.Space(_rect.height);

            // Preview Texture Channel Buttons
            EditorGUILayout.BeginHorizontal();
            for (int _i = 0; _i < _textureFormatData.channelCount; _i++)                                                                                                    // Preview Texture Channel Buttons
            {
                int _channelColorWriteMask = (int)_textureFormatData.colorWriteMasks[_i];
                if ((_colorWriteMask & _channelColorWriteMask) != _channelColorWriteMask)
                {
                    GUI.backgroundColor = Color.gray;                                                                                                                       // Set Button Color
                }
                if (GUILayout.Button(_textureFormatData.colorWriteMasks[_i].ToString()))                                                         // Toggle Buttons
                {
                    if ((_colorWriteMask & _channelColorWriteMask) != _channelColorWriteMask)                                                                               // Toggle ColorWriteMask
                    {
                        _colorWriteMask |= 1 << TextureFormats.BitIndex(_textureFormatData.colorWriteMasks[_i]);                                 // Set ColorWriteMask Bit
                    }
                    else
                    {
                        _colorWriteMask &= ~(1 << TextureFormats.BitIndex(_textureFormatData.colorWriteMasks[_i]));                              // Unset ColorWriteMask Bit
                    }
                    if (!_previewTextureTransparent)                                                                                                                        // Toggle Channel Buttons between RGB <=> A
                    {
                        if (_channelColorWriteMask == (int)ColorWriteMask.Alpha)                                                                                            // Alpha Toggle Button was pressed
                        {
                            if ((_colorWriteMask & (int)ColorWriteMask.Alpha) == (int)ColorWriteMask.Alpha)                                                                 // Deselect RGB if Alpha is toggled
                            {
                                _colorWriteMask &= ~(1 << 3);                                                                                                               // Deselect R - Channel
                                _colorWriteMask &= ~(1 << 2);                                                                                                               // Deselect G - Channel
                                _colorWriteMask &= ~(1 << 1);                                                                                                               // Deselect B - Channel
                            }
                            else                                                                                                                                            // Select RGB if Alhpa is untoggled
                            {
                                _colorWriteMask |= 1 << 3;                                                                                                                  // Select R - Channel
                                _colorWriteMask |= 1 << 2;                                                                                                                  // Select G - Channel
                                _colorWriteMask |= 1 << 1;                                                                                                                  // Select B - Channel
                            }
                        }
                        else                                                                                                                                                // RGB Toggle Button was Pressed
                        {
                            _colorWriteMask &= ~(1 << 0);                                                                                                                   // Deselect A - Channel
                        }
                    }
                }
                GUI.backgroundColor = Color.white;                                                                                                                          // Reset Button Color
            }

            // Preview Transparency Button
            if (!_previewTextureTransparent)
            {
                GUI.backgroundColor = Color.gray;
            }
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

            // Pack
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Resolution: ", GUILayout.Width(64));
            EditorGUI.BeginChangeCheck();
            _resolution = EditorGUILayout.Vector2IntField("", _resolution);
            if (EditorGUI.EndChangeCheck())
            {
                _texture2D = new Texture2D(
                    _resolution.x,
                    _resolution.y,
                    _textureFormat,
                    false
                );
                _renderTexture2D = new RenderTexture(
                    _resolution.x,
                    _resolution.y,
                    0,
                    RenderTextureFormat.ARGB32
                );
                _renderTexture2D.enableRandomWrite = true;
                _renderTexture2D.Create();
                _computeShader.SetTexture(_CS_ChannelPacker, "_targetTexture2D", _renderTexture2D);
                _computeShader.SetTexture(_CS_ClearRenderTexture2D, "_targetTexture2D", _renderTexture2D);
                this.UpdatePreview();
            }
            EditorGUI.BeginChangeCheck();
            _textureFormatOption = EditorGUILayout.Popup(_textureFormatOption, _textureFormatOptions, GUILayout.Width(96));
            if (EditorGUI.EndChangeCheck())
            {
                _textureFormat = _textureFormats[_textureFormatOption];
                _textureFormatData = TextureFormats.GetData(_textureFormat);
                this.UpdatePreview();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Save as: ", GUILayout.Width(78));
            _savePath = EditorGUILayout.TextField(_savePath);
            _exportFormat = (ExportFormats)EditorGUILayout.EnumPopup(_exportFormat, GUILayout.Width(48));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Save"))
            {
                RenderTexture.active = _renderTexture2D;                                                                                                                    // Set RenderTexture active
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
                System.IO.File.WriteAllBytes("Assets/" + _savePath + _fileExtension, _bytes);
                AssetDatabase.Refresh();
            }
            GUILayout.EndArea();
        }
        #endregion

        #region Methods
        #region Layer Menu
        public void Remove(EditorWindow_Texture2D_ChannelPacker_Layer layer)
        {
            _layers.Remove(layer);
            this.UpdatePreview();
        }
        public void MoveUp(EditorWindow_Texture2D_ChannelPacker_Layer layer)
        {
            int _index = _layers.IndexOf(layer);
            if (_index >= 1)
            {
                _layers.Remove(layer);
                _layers.Insert(_index - 1, layer);
            }
            this.UpdatePreview();
        }
        public void MoveDown(EditorWindow_Texture2D_ChannelPacker_Layer layer)
        {
            int _index = _layers.IndexOf(layer);
            if (_index < _layers.Count - 1)
            {
                _layers.Remove(layer);
                _layers.Insert(_index + 1, layer);
            }
            this.UpdatePreview();
        }
        #endregion
        #region Preview
        public void UpdatePreview()
        {
            _computeShader.Dispatch(_CS_ClearRenderTexture2D, _resolution.x, _resolution.y, 1);                                                                         // Clear RenderTexture2D
            _computeShader.SetVectorArray("_targets", new Vector4[4]);
            for (int _i = 0; _i < _layers.Count; _i++)
            {
                if (_layers[_i].texture2D != null)
                {
                    _computeShader.SetTexture(_CS_ChannelPacker, "_sourceTexture2D", _layers[_i].texture2D);                                                            // Set Layers Texture2D to ComputeShader
                    _computeShader.SetVectorArray("_targets", _layers[_i].channelPackData);
                    // Dispatch ComputeShader
                    _computeShader.Dispatch(_CS_ChannelPacker, _resolution.x, _resolution.y, 1);
                }
            }
            Graphics.CopyTexture(_renderTexture2D, _texture2D);                                                                                                         // Copy RenderTexture2D to Texture2D
        }
        #endregion
        #endregion
    }
}
#endif