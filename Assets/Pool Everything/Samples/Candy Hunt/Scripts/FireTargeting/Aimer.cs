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

    public abstract class Aimer : MonoBehaviour
    {
        [SerializeField]
        protected AimController m_AimController;

        public AimController aimController
        {
            get { return m_AimController; }
        }

        protected Transform m_Trans;

        protected virtual void Awake()
        {
            m_Trans = transform;
        }

        public void AimNozzle(Vector3 position)
        {
            StartCoroutine(m_AimController.StartAimTargeting(m_Trans, position));
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, transform.forward * m_AimController.distance);
        }
    }
}
