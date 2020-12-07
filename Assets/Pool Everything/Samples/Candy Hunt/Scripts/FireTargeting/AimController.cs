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

namespace CandyHunt
{
    [System.Serializable]
    public class AimController
    {
        [SerializeField]
        Camera m_Camera = null;

        [SerializeField]
        float m_AimTime = 0.3f;
        [SerializeField]
        float m_MinLockAngle = 0.05f;

        [SerializeField]
        float m_Distance = 15.0f;

        RaycastHit m_PointerHit;

        bool m_Targeting;

        Vector3 lockPosition { get; set; }

        public float distance { get { return m_Distance; } }

        public PointerTargeting OnAimTargeting = () => { };
        public PointerTargeting OnAimLocked = () => { };
        
        public Camera GetCamera()
        {
            return m_Camera;
        }

        public IEnumerator StartAimTargeting(Transform nozzle, Vector3 position)
        {
            if(m_Targeting)
            {
                yield break;
            }
            m_Targeting = true;
            lockPosition = position;
            OnAimTargeting.Invoke();
            Ray ray = m_Camera.ScreenPointToRay(position);
            if(Physics.Raycast(ray, out m_PointerHit, m_Distance))
            {
                ray.direction = -(nozzle.position - m_PointerHit.point);
            }
            var fromRotation = nozzle.rotation;
            var toRotation = Quaternion.LookRotation(ray.direction, nozzle.up);
            var t = 0.0f;
            var rate = 1.0f / m_AimTime;
            while(t < 1.0f)
            {
                t += Time.deltaTime * rate;
                nozzle.rotation = Quaternion.Slerp(fromRotation, toRotation, t);
                var currentAngle = Vector3.Angle(nozzle.rotation.eulerAngles, toRotation.eulerAngles);
                if(currentAngle < m_MinLockAngle)
                {
                    OnAimLocked.Invoke();
                }
                nozzle.rotation = Quaternion.Euler(nozzle.eulerAngles.x, nozzle.eulerAngles.y, 0.0f);
                yield return null;
            }
            m_Targeting = false;
        }

    }
}
