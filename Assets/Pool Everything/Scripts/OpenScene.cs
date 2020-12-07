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

using UnityEditor.SceneManagement;
using UnityEngine;

namespace PoolEverything.Utility
{
    /// <summary>
    /// Opens a scene in the editor
    /// </summary>
    [AddComponentMenu("Pool Everything/Utility/OpenScene")]
    public class OpenScene : MonoBehaviour
    {
        /// <summary>
        /// Complete scene name to open
        /// </summary>
        [SerializeField]
        protected string m_SceneName;

        /// <summary>
        /// Opens the local scene name in the editor
        /// </summary>
        public void OpenSceneName()
        {
#if UNITY_EDITOR         
            OpenSceneName(m_SceneName);
#endif
        }

        /// <summary>
        /// Opens the local scene in the editor
        /// </summary>
        /// <param name="sceneName">Complete scene name to open</param>
        public void OpenSceneName(string sceneName)
        {
#if UNITY_EDITOR
            EditorSceneManager.OpenScene(sceneName);
#endif
        }
    }

}
