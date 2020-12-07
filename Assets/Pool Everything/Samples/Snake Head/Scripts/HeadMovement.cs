#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: PoolEverything - SnakeHead
     
*/
#endregion

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace SnakeHead
{
    [AddComponentMenu("Pool Everything/Samples/Snake Head/Head Movement")]
    public class HeadMovement : BaseLink
    {
        [System.Serializable]
        public enum MoveState
        {
            None = -1,
            Left,
            Right,
            Advance,
            Retreat,
        }

        [SerializeField]
        float m_Speed = 2.0f;
        [SerializeField]
        int m_MaxLinks = 10;
        [SerializeField]
        bool m_EnforceMax = true;
        [SerializeField]
        int m_AreaSize = 10;
        [SerializeField]
        float m_AdvanceDistance = 0.75f;
        [SerializeField]
        MoveState m_MoveState = MoveState.Right;

        float m_TimeStep = 1.0f;

        Vector3 m_AdvancePoint;

        MoveState m_LastState = MoveState.None;

        [SerializeField]
        List<Transform> m_Links;
        List<Vector3> m_Positions;
        Dictionary<MoveState, System.Action<float>> m_StateActions;

        public MoveState moveState
        {
            get { return m_MoveState; }
        }

        public List<Transform> links
        {
            get { return m_Links; }
        }

        void Awake()
        {
            m_Links = new List<Transform>();
            m_Positions = new List<Vector3>();
            m_Positions.Add(transform.position);

            m_StateActions = new Dictionary<MoveState, System.Action<float>>()
            {
                { MoveState.None, (nextStep) => {} },
                { MoveState.Advance, (nextStep) => { AdvanceState(nextStep, 1); } },
                { MoveState.Retreat, (nextStep) => { AdvanceState(nextStep, -1); } },
                { MoveState.Left, (nextStep) => { SideMove(nextStep, -1, transform.position.x - nextStep < -m_AreaSize); } },
                { MoveState.Right, (nextStep) => { SideMove(nextStep, 1, transform.position.x + nextStep > m_AreaSize); } },
            };
        }

        // Use this for initialization
        IEnumerator Start()
        {
            m_LastState = m_MoveState;
            float timer = Time.time + m_TimeStep / m_Speed;
            while(enabled)
            {
                float nextStep = m_Speed * Time.deltaTime;
                if(Input.GetMouseButtonDown(0))
                {
                    Advance();
                }
                if(Input.GetMouseButtonDown(2))
                {
                    Retreat();
                }

                m_StateActions[m_MoveState].Invoke(nextStep);

                if(Time.time > timer)
                {
                    timer = Time.time + m_TimeStep / m_Speed;

                    m_Positions.Insert(0, transform.position);
                    if(m_Positions.Count > m_Links.Count)
                    {
                        m_Positions.RemoveAt(m_Positions.Count - 1);
                    }
                }

                for(int i = 0; i < m_Links.Count; i++)
                {
                    if(i < m_Positions.Count)
                    {
                        if(!m_Links[i]) continue;
                        m_Links[i].position = Vector3.MoveTowards(m_Links[i].position, m_Positions[i], m_Speed * Time.deltaTime);
                    }
                }
                yield return null;
            }
        }

        void SideMove(float nextStep, int direction, bool canAdvance)
        {
            transform.Translate(Vector3.right * nextStep * direction);
            if(canAdvance)
            {
                Advance();
            }
        }

        void AdvanceState(float nextStep, int direction)
        {
            transform.Translate(Vector3.forward * nextStep * direction);
            ChangeToLeftRightDirection();
        }

        void ChangeToLeftRightDirection()
        {
            if(Vector3.Distance(m_AdvancePoint, transform.position) > m_AdvanceDistance)
            {
                m_MoveState = m_LastState == MoveState.Left ? MoveState.Right : MoveState.Left;
            }
        }

        public void Add(Transform trans)
        {
            if(m_EnforceMax && m_Links.Where(x => x).Count() == m_MaxLinks) return;
            ModifyLinks(trans,
                (i) => m_Links[i] == null,
                (i, tr) => m_Links[i] = tr, trans,
                m_Links.Add);
        }

        public void Remove(Transform trans)
        {
            ModifyLinks(trans,
                (i) => m_Links[i] == trans,
                (i, tr) => m_Links[i] = tr, null,
                (tr) => m_Links.Remove(tr));
        }

        void ModifyLinks(Transform trans, 
            System.Func<int, bool> canMod, 
            System.Action<int, Transform> doMod, Transform modTrans,
            System.Action<Transform> elseMod)
        {
            for(int i = 0; i < m_Links.Count; i++)
            {
                if(canMod(i))
                {
                    doMod(i, modTrans);
                    return;
                }
            }
            elseMod(trans);
        }

        public void Advance()
        {
            AdvanceState(MoveState.Advance);
        }

        public void Retreat()
        {
            AdvanceState(MoveState.Retreat);
        }

        void AdvanceState(MoveState newState)
        {
            if(m_MoveState == newState) return;
            m_AdvancePoint = transform.position;
            m_LastState = m_MoveState;
            m_MoveState = newState;
        }

        void OnDrawGizmos()
        {
            var width = m_AreaSize * 0.95f;
            Debug.DrawRay(new Vector3(-width, 0, -10), Vector3.forward * m_AreaSize, Color.red);
            Debug.DrawRay(new Vector3(width, 0, -10), Vector3.forward * m_AreaSize, Color.green);
        }

    }
}
