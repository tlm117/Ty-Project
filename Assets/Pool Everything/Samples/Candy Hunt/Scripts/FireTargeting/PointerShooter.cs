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
    public class PointerShooter : PointerShooterNoPool
    {
        #region PoolEverything Intergration

        //Added to utilize PoolEverything object pool
        [SerializeField]
        [PoolManagerPicker]
        PoolManager m_PoolManager = null;

        //Added to utilize PoolEverything object pool
        [SerializeField]
        [PoolKeyPicker]
        int m_PrefabIndex = -1;

        public PoolManager poolManager { get { return m_PoolManager; } }

        public override ProjectileBehaviour GetProjectile()
        {
            PooledInfo pooledInfo = m_PoolManager.RequestActiveObject(m_PrefabIndex);
            GameObject go = pooledInfo.gameObject;

            var projectile = go.GetComponent<ProjectileBehaviour>();
            if(!projectile)
            {
                projectile = go.AddComponent<ProjectileBehaviour>();
            }
            return projectile;
        }

        #endregion
    }
}
