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
using System.Collections.Generic;
using UnityEngine;

namespace CandyHunt
{

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/AutoAim")]
    public class AutoAim : Aimer
    {

        [SerializeField]
        TargetsManager m_TargetsManager = null;
        [SerializeField]
        bool m_UseAutoAim = false;

        IEnumerator Start()
        {
            Stack<TargetBehaviour> targetBehaviours = new Stack<TargetBehaviour>();
            while(true)
            {
                if(m_UseAutoAim)
                {
                    if(targetBehaviours.Count == 0)
                    {
                        foreach(var targetBehaviour in m_TargetsManager.targetBehaviours)
                        {
                            if(targetBehaviour.activeState == ActiveState.Ready)
                            {
                                if(targetBehaviour.fortune == TargetFortune.Good)
                                {
                                    targetBehaviours.Push(targetBehaviour);
                                }
                            }
                        }
                    }
                    else
                    {
                        while(targetBehaviours.Count > 0 && m_TargetsManager.RoundState == RoundStates.Ready)
                        {
                            var tb = targetBehaviours.Pop();
                            var cam = m_AimController.GetCamera();
                            var point = cam.WorldToScreenPoint(tb.transform.position);
                            AimNozzle(point);
                            while(tb.activeState == ActiveState.Ready)
                            {
                                var dir = tb.transform.position - m_Trans.position;
                                RaycastHit hit;
                                if(!Physics.Raycast(m_Trans.position, dir, out hit, m_AimController.distance, LayerMask.NameToLayer("Target")))
                                {
                                    AimNozzle(point);
                                }
                                yield return null;
                            }
                        }
                        targetBehaviours.Clear();
                    }
                }

                yield return null;
            }
        }
    }
}
