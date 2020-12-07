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

namespace PoolEverything.Helpers
{
    /// <summary>
    /// Attribute to draw a Pool Manager and index popup to replace the integer field allows for multiple object selections
    /// </summary>
    public class MultiObjectPoolPickerAttribute : PropertyAttribute
    {
        /// <summary>
        /// Name of PoolManager field
        /// </summary>
        public readonly string poolManagerName;
        /// <summary>
        /// Label to replace the default field name
        /// </summary>
        public readonly string label;
        /// <summary>
        /// Constructor
        /// </summary>
        public MultiObjectPoolPickerAttribute(string poolManagerName = "m_PoolManager", string label = "Pool Object")
        {
            this.poolManagerName = poolManagerName;
            this.label = label;
        }
    }
}
