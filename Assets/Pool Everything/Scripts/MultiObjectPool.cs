#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: PoolEverything
     
*/
#endregion

using UnityEngine;
using System.Collections.Generic;
using PoolEverything.PoolSpawners;
using PoolEverything.PoolRecyclers;

namespace PoolEverything.Helpers
{
    /// <summary>
    /// Example of MultiObjectPoolPicker usage
    /// </summary>
    [AddComponentMenu("Pool Everything/Examples/Multi Object Pool")]
    public class MultiObjectPool : MonoBehaviour
    {
        [SerializeField, PoolManagerPicker]
        PoolManager m_PoolManager = null;

        [SerializeField, PoolManagerPicker]
        PoolManager m_PoolManager2 = null;

        [SerializeField, MultiObjectPoolPicker("m_PoolManager", "Object Id")]
        List<int> m_ObjectIds = null;

        [SerializeField, MultiObjectPoolPicker("m_PoolManager2", "Id")]
        int m_ObjectId = -1;

        [SerializeField]
        string m_ObjectName = string.Empty;

        public PoolManager poolManager { get { return m_PoolManager; } }
        public PoolManager poolManager2 { get { return m_PoolManager2; } }
        public List<int> objectIds { get { return m_ObjectIds; } }
        public int objectId { get { return m_ObjectId; } }
        public string objectName { get { return m_ObjectName; } }

        void Awake()
        {            
            InvokeRepeating("SpawnObject", 2.0f, 1.0f);
        }

        void SpawnObject()
        {
            if (Random.Range(0, 2) < 1)
            {
                var obj = m_PoolManager.RequestActiveObject(Random.Range(0, m_ObjectIds.Count));
                m_ObjectName = string.Format("{0}", obj.gameObject);
            }
            else
            {
                var obj = m_PoolManager2.RequestActiveObject(m_ObjectId);
                m_ObjectName = string.Format("{0}", obj.gameObject);
            }
        }
    }
}
