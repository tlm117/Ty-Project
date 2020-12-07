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

using PoolEverything;
using PoolEverything.Pools;
using UnityEngine;

namespace CandyHunt
{
    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/PointerShooter")]
    public class PointerShooterNoPool : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        GameObject m_Prefab = null;

        [SerializeField]
        Transform m_Nozzle = null;

        [SerializeField]
        protected ShooterController m_ShooterController;

        protected virtual void Update()
        {
            m_ShooterController.Update();
        }

        public void TryShootProjectile()
        {
            if(m_ShooterController.CanFire)
            {
                ShootProjectile();
            }
        }

        public void ShootProjectile()
        {
            ShootProjectile(GetProjectile());
        }

        public void ShootProjectile(ProjectileBehaviour projectile)
        {
            projectile.AddForce(m_Nozzle);
            m_ShooterController.ResetTimer();
        }

        public virtual ProjectileBehaviour GetProjectile()
        {
            GameObject go = GameObject.Instantiate(m_Prefab) as GameObject;

            var projectile = go.GetComponent<ProjectileBehaviour>();
            if(!projectile)
            {
                projectile = go.AddComponent<ProjectileBehaviour>();
            }
            return projectile;
        }

        public bool CanFire
        {
            get { return m_ShooterController.CanFire; }
        }

    }
}
