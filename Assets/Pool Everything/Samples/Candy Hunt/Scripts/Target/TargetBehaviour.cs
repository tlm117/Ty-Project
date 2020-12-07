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

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/TargetBehaviour")]
    public class TargetBehaviour : MonoBehaviour
    {

        [SerializeField]
        Renderer m_FrontRenderer = null;
        [SerializeField]
        Renderer m_RearRenderer = null;

        ActiveState m_ActiveState = ActiveState.None;

        TargetFortune m_Fortune = TargetFortune.None;

        Transform m_Trans;

        public ActiveState activeState { get { return m_ActiveState; } }

        public TargetFortune fortune { get { return m_Fortune; } }

        //public TargetCollision OnTargetCollision = (c) => { };

        protected virtual void Awake()
        {
            m_Trans = transform;
        }

        IEnumerator Start()
        {
            while(enabled)
            {
                switch(m_ActiveState)
                {
                    case ActiveState.None:
                        yield return StartCoroutine(IdleTarget(ActiveState.None));
                        break;
                    case ActiveState.Inactive:
                        yield return StartCoroutine(IdleTarget(ActiveState.Inactive));
                        break;
                    case ActiveState.Raise:
                        yield return StartCoroutine(RotateTarget(Quaternion.Euler(0, 0, 0), 0.3f, ActiveState.Ready));
                        break;
                    case ActiveState.Ready:
                        yield return StartCoroutine(IdleTarget(ActiveState.Ready));
                        break;
                    case ActiveState.Flatten:
                        yield return StartCoroutine(RotateTarget(Quaternion.Euler(90, 0, 0), 0.2f, ActiveState.Inactive));
                        break;
                }
            }
        }

        //#region Collision Detection

        //void OnCollisionEnter(Collision collision)
        //{
        //    OnTargetCollision.Invoke(collision);
        //} 

        //#endregion

        #region State Coroutines

        public void ChangeState(ActiveState changeState)
        {
            m_ActiveState = changeState;
        }

        IEnumerator IdleTarget(ActiveState idleState)
        {
            while(m_ActiveState == idleState)
            {
                yield return null;
            }
        }

        IEnumerator RotateTarget(Quaternion toRotation, float time, ActiveState finishState)
        {
            var fromRotation = m_Trans.rotation;
            var t = 0.0f;
            var rate = 1.0f / time;
            while(t < 1.0f)
            {
                t += Time.deltaTime * rate;
                m_Trans.rotation = Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }
            m_ActiveState = finishState;
        }

        #endregion

        #region Set Material Info Visuals

        public void SetFortuneMaterialInfo(MaterialInfo materialInfo)
        {
            m_Fortune = materialInfo.fortune;
            m_FrontRenderer.material = materialInfo.material;
        }

        public void SetStateMaterialInfo(MaterialInfo materialInfo)
        {
            m_RearRenderer.material = materialInfo.material;
            if(m_Fortune == TargetFortune.Bad)
            {
                m_RearRenderer.material = null;
            }
        } 

        #endregion
    }
}