#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: PoolEverything - CandyHunt
     
*/
#endregion

using CandyHunt;
using ScriptableObjectUtility.Legacy;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CandyHuntEditor.Utility
{

    //Used to delete associated enum script
    class MaterialDatabasePostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach(var str in deletedAssets)
            {
                var assetName = Path.GetFileNameWithoutExtension(str);
                string fileName;
                var path = MaterialDatabaseEditor.GetAssetScriptFilePath(assetName, out fileName);

                if(AssetDatabase.DeleteAsset(path))
                {
                    AssetDatabase.Refresh();
                    break;
                }
            }
        }
    }

    [CustomEditor(typeof(MaterialDatabase), true), CanEditMultipleObjects]
    public class MaterialDatabaseEditor : Editor
    {

        #region Static Utility

        [MenuItem("Assets/Create/Pool Everything/Candy Hunt/New Material Database")]
        public static void CreateNewDatabase()
        {
            ScriptableObjectUtils.SaveWithFilePanel(ScriptableObject.CreateInstance<MaterialDatabase>());
        }

        public static string GetAssetScriptFilePath(string assetName, out string fileName)
        {
            fileName = MaterialDatabaseEditor.GetAssetScriptName(assetName);
            var scriptPath = MaterialDatabaseEditor.GetAssetScriptPath();
            var path = scriptPath + fileName + ".cs";
            return path;
        }

        public static string GetAssetScriptName(string assetName)
        {
            return assetName + "ID";
        }

        public static string GetAssetScriptPath()
        {
            return Application.dataPath + "/Scripts/";
        }

        static void CreateMaterialEnums(MaterialDatabase materialDatabase)
        {
            if(!materialDatabase)
            {
                Debug.LogError("Material database object is null, can not create enum ids.");
                return;
            }
            var scriptPath = MaterialDatabaseEditor.GetAssetScriptPath();
            var directoryExists = Directory.Exists(Path.GetFullPath(scriptPath));
            if(!directoryExists)
            {
                AssetDatabase.CreateFolder("Assets", "Scripts");
            }
            string fileName;
            var path = MaterialDatabaseEditor.GetAssetScriptFilePath(materialDatabase.name, out fileName);

            var materials = materialDatabase.materialInfos.Where(x => x.material);
            var materialNames = string.Join(",", materials.Select(x => x.material.name.Replace(' ', '_').Replace('-', '_')).ToArray());
            var fileData = string.Format("public enum {0} {1} {2} {3}",
                fileName, '{', materialNames, '}');
            //Debug.Log("File Data: " + fileData);
            using(StreamWriter scriptWriter = new StreamWriter(path, false))
            {
                scriptWriter.Write(fileData);
            }

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.Refresh();
        } 

        #endregion

        protected ReorderableList m_ReorderableList;

        protected virtual void OnEnable()
        {

            m_ReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("m_MaterialInfos"), true, true, true, true);
            m_ReorderableList.headerHeight = EditorGUIUtility.singleLineHeight + 2;
            m_ReorderableList.drawHeaderCallback = (rect) =>
            {
                var width = rect.width;
                rect.width = width * 0.5f;
                EditorGUI.LabelField(rect, "Materials");
                rect.x = width * 0.5f;
                rect.x += 1.0f;
                rect.y += 1.0f;
                rect.width -= 2;
                rect.height -= 2;
                EditorGUI.DrawRect(rect, Color.black);
                rect.x += 1.0f;
                rect.y += 1.0f;
                rect.width -= 2;
                rect.height -= 2;
                EditorGUI.DrawRect(rect, Color.green);
                rect.y -= 2.0f;
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(rect, "Drop Area...?");
                var evt = Event.current;
                var dropArea = rect;
                dropArea.x += 2;
                dropArea.y += 2;
                dropArea.width -= 2;
                switch(evt.type)
                {
                    case EventType.DragUpdated:
                        if(!dropArea.Contains(evt.mousePosition)) { break; }
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        foreach(var draggedObject in DragAndDrop.objectReferences)
                        {
                            if(draggedObject as Material)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                break;
                            }
                        }
                        break;
                    case EventType.DragPerform:
                        //If drop event is not occuring over drop area break
                        if(!dropArea.Contains(evt.mousePosition)) { break; }
                        //Set default drag mode cursor pointer
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        //If event type is do drop
                        if(evt.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();
                            var currentMaterials = (target as MaterialDatabase).materialInfos
                                .Where(x => x.material != null).Select(x => x.material).ToList();
                            foreach(var draggedObject in DragAndDrop.objectReferences)
                            {
                                //Get the type of dragged object as a GameObject
                                var material = draggedObject as Material;
                                if(material && !currentMaterials.Contains(material))
                                {
                                    AddNewElement(m_ReorderableList, material);
                                }
                            }
                        }
                        Event.current.Use();
                        break;
                    case EventType.DragExited:
                        break;
                }
            };

            m_ReorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 12;
            m_ReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty property = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty materialProperty = property.FindPropertyRelative("material");
                rect.y += 5;
                rect.height = EditorGUIUtility.singleLineHeight;
                var material = materialProperty.objectReferenceValue as Material;
                Texture image = material ? material.mainTexture : null;
                Color color = GUI.color;
                GUI.color = material ? color : Color.red;
                material = EditorGUI.ObjectField(rect, new GUIContent(string.Concat("Material ", index), image), material, typeof(Material), true) as Material;
                //EditorGUI.PropertyField(rect, materialProperty, new GUIContent(string.Concat("Material ", index)), true);
                if(GUI.changed)
                {
                    var currentMaterials = (target as MaterialDatabase).materialInfos.Select(x => x.material).ToList();
                    if(material && !currentMaterials.Contains(material as Material))
                    {
                        materialProperty.objectReferenceValue = material;
                    }
                }
                bool enabled = GUI.enabled;
                GUI.enabled = material ? enabled : false;
                rect.y += EditorGUIUtility.singleLineHeight + 2;
                EditorGUI.PropertyField(rect, property.FindPropertyRelative("fortune"), new GUIContent("Fortune"), true);
                GUI.color = color;
                GUI.enabled = enabled;
            };
            m_ReorderableList.onAddCallback = (ReorderableList list) =>
            {
                AddNewElement(list);
            };
        }

        static void AddNewElement(ReorderableList list, Material material = null, TargetFortune fortune = TargetFortune.None)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("material").objectReferenceValue = material;
            element.FindPropertyRelative("fortune").intValue = (int)fortune;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_ReorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.LabelField("Enum Identifier");
            EditorGUILayout.BeginHorizontal();
            {
                var fileName = MaterialDatabaseEditor.GetAssetScriptName(target.name);
                var assembly = Assembly.GetAssembly(target.GetType());
                var type = assembly.GetType(fileName, false, false);
                if(type != null)
                {
                    System.Enum en = assembly.CreateInstance(fileName, false) as System.Enum;
                    if(en != null)
                    {
                        EditorGUILayout.EnumPopup(en);
                    }
                }
                if(GUILayout.Button("Compile Enum", EditorStyles.miniButtonRight))
                {
                    CreateMaterialEnums(target as MaterialDatabase);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}