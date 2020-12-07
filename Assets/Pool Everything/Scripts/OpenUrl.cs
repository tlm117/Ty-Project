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

using UnityEngine;

namespace PoolEverything.Utility
{
    /// <summary>
    /// Opens a url in a web browser
    /// </summary>
    [AddComponentMenu("Pool Everything/Utility/OpenURL")]
    public class OpenUrl : MonoBehaviour
    {
        /// <summary>
        /// Complete url to navigate to
        /// </summary>
        [SerializeField]
        protected string m_Url;

        /// <summary>
        /// Opens local url in a web browser
        /// </summary>
        public void Navigate()
        {
            NavigateTo(m_Url);
        }

        /// <summary>
        /// Opens url in a web browser
        /// </summary>
        /// <param name="url">Complete url to navigate to</param>
        public void NavigateTo(string url)
        {
            Application.OpenURL(url);
        }
    }

}
