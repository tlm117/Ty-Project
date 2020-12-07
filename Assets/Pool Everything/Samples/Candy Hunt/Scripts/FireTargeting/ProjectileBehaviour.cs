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

    public delegate void ProjectileCollision(ProjectileBehaviour sender, Collider other);

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/ProjectileBehaviour")]
    public class ProjectileBehaviour : MonoBehaviour
    {

        [SerializeField]
        float m_Force = 1000.0f;
        [SerializeField]
        [Tooltip("Destroy the GameObject collider trigger or Set active state to false.")]
        bool m_Destroy = false;

        public ProjectileCollision OnProjectileCollision = (s, o) => { };

        public void AddForce(Transform parent)
        {
            var trans = transform;
            trans.position = parent.position;
            trans.rotation = parent.localRotation;
            gameObject.SetActive(true);
            gameObject.GetComponent<Rigidbody>().AddForce(parent.forward * m_Force);
        }

        #region Collision Detection

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnProjectileCollision.Invoke(this, collision.collider);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Respawn"))
            {
                if(m_Destroy)
                {
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        } 

        #endregion
    }
}