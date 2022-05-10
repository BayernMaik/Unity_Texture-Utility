#if (UNITY_EDITOR)

using System;
using UnityEditor;
using UnityEngine;

namespace TextureUtility
{
    [Serializable]
    public class EditorWindow_ChannelPacker_Layer
    {
        #region Variables
        #region Channel Packer
        private EditorWindow_ChannelPacker _channelPacker;
        #endregion
        #region Texture2D
        private Texture2D _texture2D;
        public Texture2D texture2D
        {
            get { return _texture2D; }
            set { _texture2D = value; }
        }
        #endregion
        #region Texture Format Data                                                                                                                                         
        private TextureFormatData _textureFormatData;
        #endregion
        #region Show GUI Content
        private bool _showGUIContent = true;
        #endregion
        #region Channels
        [SerializeField] private bool[][] _channels = new bool[4][];
        [SerializeField] private Vector4[] _channelPackData = new Vector4[4];
        public Vector4[] channelPackData { get { return _channelPackData; } }
        #endregion
        #endregion

        #region Constructors
        public EditorWindow_ChannelPacker_Layer(EditorWindow_ChannelPacker channelPacker)
        {
            _channelPacker = channelPacker;
        }
        #endregion

        #region OnGUI
        public void OnGUI(Rect _layersRect)
        {
            #region OnRecompile
            if (_channels == null)
            {
                _channels = new bool[4][];
                _channelPackData = new Vector4[4];
                _channelPacker.UpdatePreview();
            }
            #endregion

            float _width = _layersRect.width * 0.975f;
            #region Foldout Menu
            EditorGUILayout.BeginHorizontal();                                                                                                                              // Foldout Menu
            _showGUIContent = EditorGUILayout.BeginFoldoutHeaderGroup(_showGUIContent, _texture2D == null ? "Texture2D" : _texture2D.name);                                       // BeginFoldoutHeaderGroup
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_scrollup"), GUILayout.Width(20), GUILayout.Height(16)))                                                    // Move Up Button
            {
                _channelPacker.MoveUp(this);
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_scrolldown"), GUILayout.Width(20), GUILayout.Height(16)))                                                  // Move Down Button
            {
                _channelPacker.MoveDown(this);
            }
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_clear"), GUILayout.Width(20), GUILayout.Height(16)))                                                       // Remove Button
            {
                _channelPacker.Remove(this);
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Content
            if (_showGUIContent)
            {
                #region Texture2D
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(                                                                                                                                 // Texture2D Info
                    "Texture2D\n" +
                    (_texture2D == null ? "Name" : _texture2D.name) + "\n" +                                                                                                // Texture2D Name
                    (_texture2D == null ? "Resolution" : _texture2D.width.ToString() + "x" + _texture2D.height.ToString()) + "px\n" +                                       // Texture2D Resolution
                    (_texture2D == null ? "Format" : _texture2D.format),                                                                                                    // Texture2D Format
                    GUILayout.Width(280), GUILayout.Height(64)
                );
                EditorGUI.BeginChangeCheck();
                _texture2D = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(""), _texture2D, typeof(Texture2D), false, GUILayout.Width(64), GUILayout.Height(64));   // Texture2D Reference
                if (EditorGUI.EndChangeCheck())
                {
                    if (_texture2D != null)
                    {
                        _textureFormatData = TextureFormats.GetData(_texture2D.format);                                                                                     // Find TextureFormat Data
                    }
                    _channels = new bool[4][];
                    _channelPackData = new Vector4[4];
                    _channelPacker.UpdatePreview();
                }
                EditorGUILayout.EndHorizontal();
                #endregion
                #region Texture2D Channels
                if (_texture2D != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int _from = 0; _from < _textureFormatData.channelCount; _from++)
                    {
                        #region Source Channel Buttons
                        if (_channels[_from] == null)
                        {
                            GUI.backgroundColor = Color.gray;
                        }
                        EditorGUILayout.BeginVertical();
                        float _buttonWidth = 344f / _textureFormatData.channelCount - (_textureFormatData.channelCount - 1) * 1f;
                        if (GUILayout.Button(_textureFormatData.colorWriteMasks[_from].ToString(), GUILayout.Width(_buttonWidth)))                                          // Source Channel Button
                        {
                            if (_channels[_from] == null)
                            {
                                _channels[_from] = new bool[_channelPacker.textureFormatData.channelCount];
                            }
                            else
                            {
                                _channels[_from] = null;
                            }
                            _channelPackData[_from] = Vector4.zero;
                            _channelPacker.UpdatePreview();                                                                                                                 // Update Preview
                        }
                        GUI.backgroundColor = Color.white;
                        #endregion
                        if (_channels[_from] != null)
                        {
                            #region Target Channel Buttons
                            EditorGUILayout.BeginHorizontal();
                            for (int _to = 0; _to < _channels[_from].Length; _to++)
                            {
                                if (!_channels[_from][_to])
                                {
                                    GUI.backgroundColor = Color.gray;                                                                                                       // Set Buttons GUI.backgroundColor to gray if Channel is not selected                                
                                }
                                if (GUILayout.Button(
                                        _channelPacker.textureFormatData.colorWriteMasks[_to].ToString().Substring(0, 1),
                                        GUILayout.MaxWidth(_buttonWidth / _channelPacker.textureFormatData.channelCount - (_channelPacker.textureFormatData.channelCount - 1) * 1f)
                                        )
                                    )
                                {
                                    _channels[_from][_to] = !_channels[_from][_to];
                                    if (Mathf.Approximately(_channelPackData[_from][_to], 0f))
                                    {
                                        _channelPackData[_from][_to] = 1f;
                                    }
                                    else
                                    {
                                        _channelPackData[_from][_to] = 0f;
                                    }
                                    _channelPacker.UpdatePreview();                                                                                                         // Update Preview
                                }
                                GUI.backgroundColor = Color.white;                                                                                                          // Reset GUI.backgroundColor to default
                            }
                            EditorGUILayout.EndHorizontal();
                            #endregion
                        }
                        EditorGUILayout.EndVertical();
                        GUI.backgroundColor = Color.white;                                                                                                                  // Reset GUI.backgroundColor to default
                    }
                    EditorGUILayout.EndHorizontal();
                }
                #endregion
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();                                                                                                                        // EndFoldoutHeaderGroup from Foldout Menu
            #endregion
        }
        #endregion
    }
}
#endif