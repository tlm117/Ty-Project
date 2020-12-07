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

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/StageManager")]
    public class StageManager : MonoBehaviour
    {
        [SerializeField]
        Transform m_Platforms = null;
        [SerializeField]
        Transform m_Targets = null;
        [SerializeField]
        Transform m_RoundCanvas = null;

        [SerializeField]
        Vector3 m_Offset = Vector3.zero;
        [SerializeField]
        float m_Time = 0.0f;

        float m_Speed = 1.0f;

        public float time
        {
            set { m_Time = value; }
        }

        IEnumerator Start()
        {
            Vector3 platformsPosition = m_Platforms.position;
            Vector3 targetsPosition = m_Targets.position;
            Vector3 roundCanvasPosition = m_RoundCanvas.position;
            var t = 0.5f;
            while(enabled)
            {

                m_Time = Mathf.Clamp(m_Time, 0.0f, 10.0f);
                if(m_Time > 0)
                {
                    var lowOffset = m_Offset - (m_Offset * 2.0f);
                    var rate = 1.0f / m_Time;

                    t += Time.deltaTime * rate;
                    var pt = Mathf.PingPong(t * m_Speed, 1.0f);
                    m_Platforms.position = Vector3.Lerp(platformsPosition + lowOffset, platformsPosition + m_Offset, pt);
                    m_Targets.position = Vector3.Lerp(targetsPosition + lowOffset, targetsPosition + m_Offset, pt);
                    m_RoundCanvas.position = Vector3.Lerp(roundCanvasPosition + lowOffset, roundCanvasPosition + m_Offset, pt);
                }

                yield return null;
            }
        }
    }
}
