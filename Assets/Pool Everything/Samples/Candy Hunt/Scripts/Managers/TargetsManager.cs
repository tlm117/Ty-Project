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
using System.Linq;
using UnityEngine;

namespace CandyHunt
{

    public enum RoundStates
    {
        None = ActiveStateID.None,
        Inactive = ActiveStateID.Inactive,
        Raise = ActiveStateID.Raise,
        Ready = ActiveStateID.Ready,
        Flatten = ActiveStateID.Flatten,
    }

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/TargetsManager")]
    public class TargetsManager : MonoBehaviour
    {
        
        public ReadyCoundown OnReadyCoundown = (s) => { };
        public RoundTimeLeft OnRoundTimeLeft = (t) => { };
        public RoundStateChanged OnRoundStateChanged = (r) => { };

        [SerializeField]
        MaterialDatabase m_FortuneDatabase = null;
        [SerializeField]
        MaterialDatabase m_StateDatabase = null;
        [SerializeField]
        RoundStates m_RoundState = RoundStates.None;

        float m_ShowTime;
        float m_HideTime;

        TargetBehaviour[] m_TargetBehaviours;

        MaterialInfo m_DefaultInfo;
        IEnumerable<MaterialInfo> m_UsableMaterialInfos;
        MaterialInfo[] m_GoodMaterialInfos;
        MaterialInfo[] m_BadMaterialInfos;
        TargetBehaviour[] m_RoundTargetBehaviours;

        int m_ShowAmount = 0;
        int m_Level = 0;

        bool isInitialized = false;

        public TargetBehaviour[] targetBehaviours
        {
            get { return m_TargetBehaviours; }
        }

        public RoundStates RoundState
        {
            get { return m_RoundState; }
            set { OnRoundStateChanged.Invoke(m_RoundState = value); }
        }

        public TargetBehaviour[] roundTargetBehaviours
        {
            get { return m_RoundTargetBehaviours; }
        }

        protected virtual void Awake()
        {
            Initialize();
        }
            
        IEnumerator Start()
        {
            int lastLevel = m_Level;
            while(enabled)
            {                
                switch(RoundState)
                {
                    case RoundStates.None:
                        yield return null;
                        break;
                    case RoundStates.Inactive:
                        if(lastLevel != m_Level)
                        {
                            float levelChangeTime = 3.0f;
                            float levelChangeTimer = Time.timeSinceLevelLoad + levelChangeTime;
                            lastLevel = m_Level;
                            while(levelChangeTimer > Time.timeSinceLevelLoad)
                            {
                                yield return null;
                            }
                        }
                        float hideTimer = Time.timeSinceLevelLoad + m_HideTime;
                        while(hideTimer > Time.timeSinceLevelLoad)
                        {
                            yield return null;
                        }

                        m_RoundTargetBehaviours = m_TargetBehaviours
                            .Where(x => x.activeState == ActiveState.Inactive)
                            .OrderBy(x => System.Guid.NewGuid())
                            .Take(m_ShowAmount)
                            .ToArray();
                        RoundState = RoundStates.Raise;
                        break;
                    case RoundStates.Raise:
                        var goods = m_Level;
                        var halfLength = (m_RoundTargetBehaviours.Length / 2) + 1;
                        if(goods >= halfLength)
                        {
                            goods = Random.Range(0, goods / 2) + halfLength;
                        }
                        goods = Mathf.Clamp(goods, 1, m_RoundTargetBehaviours.Length);
                        List<MaterialInfo> matInfos = new List<MaterialInfo>(m_GoodMaterialInfos.OrderBy(x => System.Guid.NewGuid()).Take(goods));
                        matInfos.AddRange(m_BadMaterialInfos.OrderBy(x => System.Guid.NewGuid()).Take(m_RoundTargetBehaviours.Length - goods));

                        SetTargetsState(m_RoundTargetBehaviours, ActiveState.Raise, matInfos.ToArray());
                        RoundState = RoundStates.Ready;
                        yield return null;
                        break;
                    case RoundStates.Ready:
                        float showTimer = Time.timeSinceLevelLoad + m_ShowTime;
                        while(showTimer > Time.timeSinceLevelLoad)
                        {
                            if(m_RoundTargetBehaviours
                                .Where(x => x.activeState == ActiveState.Raise || x.activeState == ActiveState.Ready)
                                .Count(x => x.fortune == TargetFortune.Good) == 0)
                            {
                                break;
                            }
                            yield return null;
                        }
                        OnRoundTimeLeft.Invoke(Mathf.Clamp(showTimer - Time.timeSinceLevelLoad, 0, m_ShowTime));
                        RoundState = RoundStates.Flatten;
                        break;
                    case RoundStates.Flatten:
                        SetTargetsState(m_RoundTargetBehaviours, ActiveState.Flatten, m_DefaultInfo);
                        RoundState = RoundStates.None;
                        yield return null;
                        break;
                }
            }
        }

        void SetTargetsState(TargetBehaviour[] showTargetBehaviours, ActiveState activeState, params MaterialInfo[] matInfos)
        {
            int matCounter = 0;
            UpdateActiveStateMaterials();
            foreach(var targetBehaviour in showTargetBehaviours)
            {
                targetBehaviour.SetFortuneMaterialInfo(matInfos[matCounter++]);
                targetBehaviour.ChangeState(activeState);
                if(matCounter == matInfos.Length) matCounter = 0;
            }
            UpdateActiveStateMaterials();
        }

        void UpdateActiveStateMaterials()
        {
            foreach(var targetBehaviour in m_TargetBehaviours)
            {
                var stateInt = (int)targetBehaviour.activeState;
                targetBehaviour.SetStateMaterialInfo(m_StateDatabase.materialInfos[stateInt]);
            }
        }

        public void Initialize()
        {
            if(isInitialized) return;
            m_TargetBehaviours = GetComponentsInChildren<TargetBehaviour>(true);
            m_DefaultInfo = m_FortuneDatabase.materialInfos[(int)FortuneMaterialDatabaseID.Sprites_Default];

            m_UsableMaterialInfos = m_FortuneDatabase.materialInfos.Where(x => x.material != m_DefaultInfo.material && x.fortune != TargetFortune.None);

            m_GoodMaterialInfos = m_UsableMaterialInfos
                .Where(x => ((int)x.fortune) % 2 == 0)
                .ToArray();

            m_BadMaterialInfos = m_UsableMaterialInfos
                .Where(x => ((int)x.fortune) % 2 != 0)
                .ToArray();

            foreach(var targetBehaviour in m_TargetBehaviours)
            {
                targetBehaviour.ChangeState(ActiveState.Flatten);
            }
            isInitialized = true;
        }

        public void LevelChanged(int level)
        {
            m_Level = level;
        }

        public void RoundStarted(int showAmount, float showTime, float hideTime)
        {
            m_ShowAmount = showAmount;
            m_ShowTime = showTime;
            m_HideTime = hideTime;

            m_ShowAmount = m_TargetBehaviours.Length;
        }
    }
}