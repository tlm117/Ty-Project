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
using UnityEngine.Events;

namespace CandyHunt
{

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/PlayerManager")]
    public class PlayerManager : MonoBehaviour
    {

        public ScoreChanged OnScoreChanged = (s) => { };
        public AmmoChanged OnAmmoChanged = (a) => { };
        public ProjectileFired OnProjectileFired = (pr) => { };

        [SerializeField]
        Aimer[] m_Aimers = new Aimer[] { };
        [SerializeField]
        PointerShooterNoPool[] m_Shooters = new PointerShooterNoPool[] { };

        int m_Ammo;

        int m_Score = 0;

        public int Ammo { get { return m_Ammo; } set { OnAmmoChanged.Invoke(m_Ammo = value); } }

        public int Score { get { return m_Score; } set { OnScoreChanged.Invoke(m_Score = value); } }

        void Awake()
        {
            foreach(var aimer in m_Aimers)
            {
                aimer.aimController.OnAimLocked += AimLock;
            }
        }

        void AimLock()
        {
            foreach(var shooter in m_Shooters)
            {
                if(shooter.CanFire && Ammo > 0)
                {
                    Ammo--;
                    var projectile = shooter.GetProjectile();
                    shooter.ShootProjectile(projectile);
                    OnProjectileFired.Invoke(projectile);
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
            foreach(var aimer in m_Aimers)
            {
                aimer.enabled = enabled;
            }
            foreach(var shooter in m_Shooters)
            {
                shooter.enabled = enabled;
            }
        }

    }
}
