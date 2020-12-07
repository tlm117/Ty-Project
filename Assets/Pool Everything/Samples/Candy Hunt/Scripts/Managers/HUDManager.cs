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
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CandyHunt
{

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/HUDManager")]
    public class HUDManager : MonoBehaviour
    {
        [Header("Counter")]
        [SerializeField]
        RectTransform m_CounterPanel = null;
        [SerializeField]
        Text m_CounterText = null;

        [Header("Score")]
        [SerializeField]
        RectTransform m_ScorePanel = null;
        [SerializeField]
        Text m_ScoreText = null;
        [SerializeField]
        Text m_LevelText = null;

        [Header("Ammo")]
        [SerializeField]
        RectTransform m_AmmoPanel = null;
        [SerializeField]
        Text m_AmmoText = null;
        [SerializeField]
        RectTransform m_GameOverPanel = null;

        [Header("Bonus")]
        #region PoolEverything Intergration

        //Added to utilize PoolEverything object pool
        [SerializeField]
        [PoolManagerPicker]
        PoolManager m_PoolManager = null;

        //Added to utilize PoolEverything object pool
        [SerializeField]
        [PoolKeyPicker("Bonus Prefab")]
        int m_BonusIndex = -1;
        //Added to utilize PoolEverything object pool
        [SerializeField]
        [PoolKeyPicker("Plus Prefab")]
        int m_BonusPlusIndex = -1;

        #endregion

        [Header("Sharshooter")]
        //Added to utilize PoolEverything object pool
        [SerializeField]
        [PoolKeyPicker("Sharp Shooter Prefab")]
        int m_SharshooterIndex = -1;
        
        [SerializeField]
        RectTransform m_SharshooterPanel = null;

        void Awake()
        {
            m_CounterPanel.gameObject.SetActive(false);
            m_ScorePanel.gameObject.SetActive(false);
            m_AmmoPanel.gameObject.SetActive(false);
            m_GameOverPanel.gameObject.SetActive(false);
        }

        public void AmmoChanged(int ammo)
        {
            m_AmmoText.text = string.Format("Ammo: {0}", ammo);
        }

        public void ScoreChanged(int score)
        {
            m_ScoreText.text = string.Format("Score: {0}", score);
        }

        public void GameOver()
        {
            m_GameOverPanel.gameObject.SetActive(true);
        }

        public void LevelChanged(int level)
        {
            m_LevelText.text = string.Format("Level: {0}", level);
        }

        public RectTransform ShowBonusEarned(int bonus, Vector3 position, Color color, float lifeTime)
        {
            return ShowEarned(string.Format("+{0}", bonus), position, color, lifeTime);
        }

        public RectTransform ShowMultiplierEarned(int multiplier, Vector3 position, Color color, float lifeTime)
        {
            return ShowEarned(string.Format("X{0}", multiplier), position, color, lifeTime);
        }

        public RectTransform ShowTimeBonusEarned(int bonus, Vector3 position, Color color, float lifeTime)
        {
            return RequestBonusCanvas(m_BonusPlusIndex, string.Format("Time Bonus\n+{0}", bonus), position, color, lifeTime);
        }

        public RectTransform ShowAmmoBonusEarned(int bonus, Vector3 position, Color color, float lifeTime)
        {
            return RequestBonusCanvas(m_BonusPlusIndex, string.Format("Ammo Bonus\n+{0}", bonus), position, color, lifeTime);
        }

        public RectTransform ShowEarned(string textValue, Vector3 position, Color color, float lifeTime)
        {
            return RequestBonusCanvas(m_BonusIndex, textValue, position, color, lifeTime);
        }
        
        //Old Instantiate
        public RectTransform ShowBonusCanvas(GameObject prefab, string textValue, Vector3 position, Color color, float lifeTime)
        {
            var go = Instantiate(prefab, position, Quaternion.identity) as GameObject;
            Destroy(go, lifeTime);
            return PrepareCanvas(go, textValue, position, color);
        }

        #region PoolEverything Intergration

        public RectTransform RequestBonusCanvas(int prefabIndex, string textValue, Vector3 position, Color color, float lifeTime)
        {
            var po = m_PoolManager.RequestActiveObject(prefabIndex);
            if(po == null) return null;
            var go = po.gameObject as GameObject;
            return PrepareCanvas(go, textValue, position, color);
        }

        GameObject RequestSharpshooter()
        {
            GameObject go = m_PoolManager.RequestActiveObject(m_SharshooterIndex).gameObject as GameObject;
            go.transform.SetParent(m_SharshooterPanel);
            return go;
        }

        #endregion

        public void IncrementSharshooter()
        {
            GameObject go = RequestSharpshooter(); 
        
            go.transform.SetParent(m_SharshooterPanel);
        }

        static RectTransform PrepareCanvas(GameObject go, string textValue, Vector3 position, Color color)
        {
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.position = position;
            Text text = go.GetComponentInChildren<Text>();
            text.color = color;
            text.text = textValue;
            return rectTransform;
        }

        public void ReadyCoundown(int second)
        {
            if(second > 0)
            {
                if(!m_CounterPanel.gameObject.activeInHierarchy) m_CounterPanel.gameObject.SetActive(true);
                m_CounterText.text = second.ToString();
            }
            else
            {
                m_CounterPanel.gameObject.SetActive(false);
                m_ScorePanel.gameObject.SetActive(true);
                m_AmmoPanel.gameObject.SetActive(true);
            }
        }

        public static Vector3 GetBonusCanvasPosition(Transform tr, float scaleRatio)
        {
            return tr.position + Vector3.up * tr.localScale.y * scaleRatio;
        }

    }
}
