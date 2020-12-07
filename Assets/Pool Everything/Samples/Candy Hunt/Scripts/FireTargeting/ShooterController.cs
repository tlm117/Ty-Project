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
    [System.Serializable]
    public class ShooterController
    {
        [SerializeField]
        [Tooltip("Value reperesents rate of fire per second.  ie. 2 will be (2 per/sec) or (1 each 1/2 sec).")]
        float m_RateOfFire = 2.0f;

        float m_DelayTimer = 0.0f;

        public bool TryFire(bool fireAttempt)
        {
            Update();
            if(fireAttempt)
            {
                if(CanFire)
                {
                    m_DelayTimer = 0.0f;
                    return true;
                }
            }
            return false;
        }

        public bool CanFire
        {
            get
            {
                return m_DelayTimer >= (1 / m_RateOfFire);
            }
        }

        public void Update()
        {
            m_DelayTimer += Time.deltaTime;
        }

        public void ResetTimer()
        {
            m_DelayTimer = 0.0f;
        }
    }

}
