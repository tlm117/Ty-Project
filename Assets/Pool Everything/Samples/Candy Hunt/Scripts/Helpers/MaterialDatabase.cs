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

using UnityEngine;

namespace CandyHunt
{
    public class MaterialDatabase : ScriptableObject
    {
        [SerializeField]
        MaterialInfo[] m_MaterialInfos;

        public MaterialInfo[] materialInfos
        {
            get { return m_MaterialInfos; }
            set { m_MaterialInfos = value; }
        }
    }
}
