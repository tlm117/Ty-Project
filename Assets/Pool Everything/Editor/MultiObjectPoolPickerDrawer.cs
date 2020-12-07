#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: PoolEverything
     
*/
#endregion

using UnityEditor;
using UnityEngine;
using System.Linq;
using PoolEverything.Helpers;
using PoolEverything;

namespace PoolEverythingEditor
{
    /// <summary>
    /// Custom editor to draw popup to pick a pool manager and pooled object
    /// </summary>
    [CustomPropertyDrawer(typeof(MultiObjectPoolPickerAttribute), true)]
    [CanEditMultipleObjects]
    public class MultiObjectPoolPickerDrawer : PropertyDrawer
    {
        int m_PoolKey = -1;

        /// <summary>
        /// Called to draw the proeprty
        /// </summary>
        /// <param name="position">Rectangle position to draw property</param>
        /// <param name="property">Peoperty passed in by the calss field</param>
        /// <param name="label">Name and toolip of field property</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MultiObjectPoolPickerAttribute attr = attribute as MultiObjectPoolPickerAttribute;
            SerializedProperty poolManagerProperty = property.serializedObject.FindProperty(attr.poolManagerName);
            PoolManager poolManager = poolManagerProperty.objectReferenceValue as PoolManager;
            int poolIndex = property.intValue;
            for (int i = 0; poolManager && poolManager.poolReferences != null && i < poolManager.poolReferences.Length; i++)
            {
                if (poolIndex == i)
                {
                    m_PoolKey = i;
                    break;
                }
            }
            if (poolManager)
            {
                var prefabs = poolManager.poolReferences;
                if (prefabs == null || poolManager.poolReferences.Where(x => x.reference).Count() == 0)
                {
                    var helpRect = position;
                    helpRect.x = 100;
                    helpRect.width -= 86;
                    EditorGUI.HelpBox(helpRect, "No pools in manager.", MessageType.Info);
                    var btnRect = position;
                    btnRect.x += 10;
                    btnRect.width = 74;
                    if (GUI.Button(btnRect, "Add Now"))
                    {
                        Selection.activeObject = poolManager;
                    }
                }
                else
                {
                    var guiContents = prefabs.Select(x => new GUIContent(x != null && x.reference ? x.reference.name : "")).ToArray();
                    m_PoolKey = EditorGUI.Popup(position, new GUIContent(attr.label), m_PoolKey, guiContents);
                }
                if (GUI.changed)
                {
                    property.intValue = m_PoolKey;
                }
            }
            else
            {
                EditorGUI.HelpBox(position, "Select a Pool Manager", MessageType.Info);
                m_PoolKey = -1;
            }
        }
    }

}
