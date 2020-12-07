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

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CandyHunt
{

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/Round Info Canvas")]
    public class RoundInfoCanvas : MonoBehaviour
    {
        [SerializeField]
        Text m_CurrentText = null;
        [SerializeField]
        Text m_MaxText = null;

        public void RoundChanged(int round, int max)
        {
            m_CurrentText.text = round.ToString();
            m_MaxText.text = max.ToString();
        }

    }
}
