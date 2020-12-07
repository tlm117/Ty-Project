#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: PoolEverything - SnakeHead
     
*/
#endregion

using System.Collections;
using UnityEngine;

namespace SnakeHead
{
    [AddComponentMenu("Pool Everything/Samples/Snake Head/Sweep Ping Pong")]
    public class SweepPingPong : MonoBehaviour
    {
        [Range(0.0f, 359.0f)]
        [SerializeField]
        float m_Angle = 135.0f;

        [SerializeField]
        [Tooltip("Time in seconds to complete sweep.")]
        float m_Time = 5.0f;

        IEnumerator Start()
        {
            Transform trans = transform;
            float startAngle = trans.rotation.eulerAngles.y;
            var t = 0.0f;
            while(enabled) 
            {
                var rate = 1.0f / m_Time;
                t += Time.deltaTime * rate;
                var angle = Mathf.PingPong(t, 1.0f) * m_Angle - (m_Angle * 0.5f);
                trans.rotation = Quaternion.Euler(trans.rotation.eulerAngles.x, startAngle + angle, trans.rotation.eulerAngles.z);
                yield return null;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.forward * 5.0f);
        }
    }
}
