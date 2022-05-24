# if (UNITY_EDITOR)

using UnityEditor;
using UnityEngine;

namespace TextureUtility
{
    public class EditorWindow_Texture2D_ArrayPacker : EditorWindow
    {
        #region Variables
        private SerializedObject serializedObject;
        private SerializedProperty serializedProperty_texture2DArray;
        private SerializedProperty serializedProperty_createAssetPath;

        [SerializeField] private Texture2D[] _inputTexture2Ds;
        private string _createAssetPath = "Texture2DArray.asset";
        private GUIContent _GUIContentPath = new GUIContent("Save as: ", "Save as from Path: Assets/");
        #endregion

        #region Menu Item
        [MenuItem("Tools/Texture2D/Array Packer")]
        static void ShowEditorWindow()
        {
            EditorWindow_Texture2D_ArrayPacker _editorWindow = (EditorWindow_Texture2D_ArrayPacker)EditorWindow.GetWindow(typeof(EditorWindow_Texture2D_ArrayPacker));

            _editorWindow.serializedObject = new SerializedObject(_editorWindow);
            _editorWindow.serializedProperty_texture2DArray = _editorWindow.serializedObject.FindProperty("_inputTexture2Ds");
            _editorWindow.serializedProperty_createAssetPath = _editorWindow.serializedObject.FindProperty("_createAssetPath");

            _editorWindow.titleContent = new GUIContent("Texture2D Array Packer");

            _editorWindow.Show();
        }
        #endregion

        #region OnEnable
        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            serializedProperty_texture2DArray = serializedObject.FindProperty("_inputTexture2Ds");
            serializedProperty_createAssetPath = serializedObject.FindProperty("_createAssetPath");
        }
        #endregion

        #region OnGUI
        void OnGUI()
        {
            // Title
            EditorGUILayout.LabelField("Texture2DArray Packer", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Texture 2D Array
            EditorGUILayout.LabelField("Textures to Pack into Texture2DArray");
            EditorGUILayout.PropertyField(serializedProperty_texture2DArray);
            EditorGUILayout.Space();

            // Path
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_GUIContentPath, GUILayout.Width(64));
            EditorGUILayout.TextField(_createAssetPath);
            EditorGUILayout.EndHorizontal();

            // Create Button
            if (GUILayout.Button("Create"))
            {
                _inputTexture2Ds = new Texture2D[serializedProperty_texture2DArray.arraySize];
                for (int _i = 0; _i < _inputTexture2Ds.Length; _i++)
                {
                    _inputTexture2Ds[_i] = (Texture2D)serializedProperty_texture2DArray.GetArrayElementAtIndex(_i).objectReferenceValue;
                }
                Texture2DArray _texture2DArray = Texture2DUtility.CreateTexture2DArray(_inputTexture2Ds, _inputTexture2Ds[0].width, _inputTexture2Ds[0].height);
                AssetDatabase.CreateAsset(
                    _texture2DArray,
                    "Assets/" + _createAssetPath
                );
            }
        }
        #endregion
    }
}
#endif