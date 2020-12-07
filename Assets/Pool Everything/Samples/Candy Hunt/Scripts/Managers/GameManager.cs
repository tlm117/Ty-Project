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

using PoolEverything.Limits;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CandyHunt
{
    public enum GameStates
    {
        None = -1,
        Playing,
        GameOver
    }

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/GameManager")]
    public class GameManager : MonoBehaviour
    {
        public static GameStateChanged OnGameStateChanged = (s) => { };
        public static LevelChanged OnLevelChanged = (lvl) => { };
        public static RoundChanged OnRoundChanged = (r) => { };

        [Header("Stage")]
        [SerializeField]
        StageManager m_StageManager = null;
        [SerializeField]
        GameStates m_GameState = GameStates.None;
        [SerializeField]
        [LimitValue(LimitEditor.MinMaxSlider/*, 1f, 12f*/)]
        LimitedInt m_Level = new LimitedInt(1, 10, 1, 12, 1);
        [SerializeField]
        [LimitValue(LimitEditor.MinMaxSlider/*, 0f, 20.0f*/)]
        LimitedInt m_Round = new LimitedInt(0, 9, 0, 20, 0);
        [SerializeField]
        int m_PointsPerHit = 10;
        [SerializeField]
        float m_AmmoMultiplier = 1.4f;
        [SerializeField]
        int m_MovePlatformLevel = 4;
        int m_LevelHits = 0;
        int m_RoundHits = 0;
        [SerializeField]
        int m_MinimumBonusHits = 1;

        [Header("Player")]
        [SerializeField]
        PlayerManager m_PlayerManager = null;
        [SerializeField]
        int m_StartAmmo = 20;

        [Header("Targets")]
        [SerializeField]
        TargetsManager m_TargetsManager = null;
        [SerializeField]
        [LimitValue(LimitEditor.MinMaxSlider)]
        LimitedFloat m_ShowTime = new LimitedFloat(3.0f, 5.0f, 2.0f, 7.0f);
        [SerializeField]
        [LimitValue(LimitEditor.MinMaxSlider)]
        LimitFloat m_HideTime = new LimitFloat(0.5f, 2.0f, 0.2f, 3.0f);

        [Header("HUD")]
        [SerializeField]
        HUDManager m_HUDManager = null;
        [SerializeField]
        RoundInfoCanvas m_RoundInfoCanvas = null;
        [SerializeField]
        int m_CountDown = 3;
        [SerializeField]
        float m_ShowBonusDestroy = 2;

        public GameStates GameState
        {
            get { return m_GameState; }
            set { OnGameStateChanged.Invoke(m_GameState = value); }
        }
        public int Level
        {
            get { return m_Level.current; }
            set { OnLevelChanged.Invoke(m_Level.current = value); }
        }
        public int Round
        {
            get { return m_Round.current; }
            set { OnRoundChanged.Invoke(m_Round.current = value); }
        }

        void Awake()
        {
            if(!m_HUDManager.gameObject.activeInHierarchy)
            {
                m_HUDManager.gameObject.SetActive(true);
            }
            m_TargetsManager.Initialize();

            m_TargetsManager.OnReadyCoundown += (second) =>
            {
                m_HUDManager.ReadyCoundown(second);
                if(second == 0)
                {
                    m_TargetsManager.RoundState = RoundStates.Inactive;
                }
            };
            int levelTimeBonus = 0;
            m_TargetsManager.OnRoundTimeLeft += (timeRemainig) =>
            {
                if(timeRemainig > 0.0f)
                {
                    float level_round = (((float)Level / (float)m_Level.maximum) + ((float)Round / (float)m_Round.maximum)) * 0.5f;

                    float offset = 1 - m_ShowTime.current / m_ShowTime.maximum;
                    float adjustment = m_ShowTime.maximum * offset;
                    float timeBonus = (adjustment + timeRemainig + level_round) * m_PointsPerHit;
                    int bonus = Mathf.CeilToInt(timeBonus / 5) * 5;
                    levelTimeBonus += bonus;
                }
            };
            var goodsUsed = 0;
            m_TargetsManager.OnRoundStateChanged += (rs) =>
            {
                if(GameState == GameStates.GameOver) return;

                if(rs == RoundStates.Ready)
                {
                    goodsUsed += m_TargetsManager.roundTargetBehaviours.Where(x => x.fortune == TargetFortune.Good).Count();
                } 
                else if(rs == RoundStates.None)
                {
                    if(Round == m_Round.maximum)
                    {
                        var ammoUsed = m_StartAmmo - m_PlayerManager.Ammo;
                        //Hit all goods
                        if(m_LevelHits == goodsUsed)
                        {
                            if(ammoUsed == m_LevelHits)
                            {
                                //Hit all fired at
                                m_HUDManager.IncrementSharshooter();
                            }
                        }
                        var earnedAmmoRaw = ((float)m_LevelHits * m_AmmoMultiplier);
                        var earnedAmmo = Mathf.CeilToInt(earnedAmmoRaw);

                        var position = Vector3.zero + Vector3.up * 2.0f - Vector3.forward * 5.0f;
                        m_HUDManager.ShowTimeBonusEarned(levelTimeBonus, position, Color.yellow, m_ShowBonusDestroy);

                        position = Vector3.zero + Vector3.up * 3.0f - Vector3.forward * 4.0f;
                        m_HUDManager.ShowAmmoBonusEarned(earnedAmmo, position, Color.yellow, m_ShowBonusDestroy);
                        m_PlayerManager.Score = Mathf.Clamp(m_PlayerManager.Score + levelTimeBonus, 0, int.MaxValue);
                        levelTimeBonus = 0;
                        
                        m_PlayerManager.Ammo += earnedAmmo;
                        m_StartAmmo = m_PlayerManager.Ammo;
                        m_LevelHits = 0;
                        goodsUsed = 0;
                        Level++;
                        Round = m_Round.minimum;
                    }
                    m_RoundHits = 0;
                    Round++;
                    m_TargetsManager.RoundState = RoundStates.Inactive;
                }
            };

            m_PlayerManager.OnAmmoChanged += (ammo) =>
            {
                m_HUDManager.AmmoChanged(ammo);
                if(m_PlayerManager.Ammo <= 0)
                {
                    GameState = GameStates.GameOver;
                    m_TargetsManager.RoundState = RoundStates.None;
                }
            };

            m_PlayerManager.OnScoreChanged += (score) =>
            {
                m_HUDManager.ScoreChanged(score);
            };

            m_PlayerManager.OnProjectileFired += (projectile) =>
            {
                projectile.OnProjectileCollision += (sender, other) =>
                {
                    var targetBehaviour = other.gameObject.GetComponentInParent<TargetBehaviour>();
                    if(targetBehaviour)
                    {
                        // Commented to use recycler component
                        projectile.gameObject.SetActive(false);
                        if(targetBehaviour.activeState == ActiveState.Ready)
                        {
                            targetBehaviour.ChangeState(ActiveState.Flatten);

                            if(targetBehaviour.fortune == TargetFortune.Good)
                            {
                                m_PlayerManager.Score += m_PointsPerHit;
                                m_RoundHits++;
                                m_LevelHits++;
                                if(m_RoundHits > m_MinimumBonusHits)
                                {
                                    m_PlayerManager.Score += m_PointsPerHit * m_RoundHits;

                                    var tr = targetBehaviour.gameObject.transform;
                                    var position = HUDManager.GetBonusCanvasPosition(tr, 0.5f);
                                    m_HUDManager.ShowBonusEarned(m_PointsPerHit * m_RoundHits, position, Color.yellow, m_ShowBonusDestroy);

                                    position = HUDManager.GetBonusCanvasPosition(tr, 1.6f);
                                    m_HUDManager.ShowMultiplierEarned(m_RoundHits, position, Color.green, m_ShowBonusDestroy);
                                }
                                else
                                {
                                    var tr = targetBehaviour.gameObject.transform;
                                    var position = HUDManager.GetBonusCanvasPosition(tr, 0.5f);
                                    m_HUDManager.ShowBonusEarned(m_PointsPerHit, position, Color.yellow, m_ShowBonusDestroy);
                                }
                            }
                            else if(targetBehaviour.fortune == TargetFortune.Bad)
                            {
                                m_PlayerManager.Score -= m_PointsPerHit;
                                m_RoundHits = 0;

                                var tr = targetBehaviour.gameObject.transform;
                                var position = HUDManager.GetBonusCanvasPosition(tr, 0.5f);
                                m_HUDManager.ShowBonusEarned(m_PointsPerHit, position, Color.red, m_ShowBonusDestroy);
                            }

                            m_PlayerManager.Score = Mathf.Clamp(m_PlayerManager.Score, 0, int.MaxValue);
                        }
                    }
                    else
                    {
                        m_RoundHits = 0;
                    } 
                };
            };

            OnLevelChanged += GameManager_OnLevelChanged;
            OnRoundChanged += GameManager_OnRoundChanged;
        }

        void GameManager_OnLevelChanged(int level)
        {
            m_HUDManager.LevelChanged(level);
            m_TargetsManager.LevelChanged(level);
            m_RoundInfoCanvas.RoundChanged(Round, m_TargetsManager.targetBehaviours.Length);
            if(level >= m_MovePlatformLevel)
            {
                m_StageManager.time = 1 + ((float)(m_Level.maximum - level) * 0.75f);
            }
            else
            {
                m_StageManager.time = 0.0f;
            }
        }

        void GameManager_OnRoundChanged(int round)
        {
            m_TargetsManager.RoundStarted(round,
                m_ShowTime.current = Random.Range(m_ShowTime.minimum, m_ShowTime.maximum),
                Random.Range(m_HideTime.minimum, m_HideTime.maximum));
            m_RoundInfoCanvas.RoundChanged(round, m_TargetsManager.targetBehaviours.Length);
        }

        IEnumerator Start()
        {
            while(true)
            {
                switch(m_GameState)
                {
                    case GameStates.None:
                        m_PlayerManager.SetEnabled(false);
                        OnLevelChanged(Level);
                        OnRoundChanged(Round);
                        m_PlayerManager.Ammo = m_StartAmmo;
                        GameState = GameStates.Playing;
                        break;
                    case GameStates.Playing:
                        for(int countDown = m_CountDown; countDown > 0; countDown--)
                        {
                            m_TargetsManager.OnReadyCoundown.Invoke(countDown);
                            yield return new WaitForSeconds(1);
                        }
                        m_TargetsManager.OnReadyCoundown.Invoke(0);

                        m_PlayerManager.SetEnabled(true);
                        while(GameState == GameStates.Playing)
                        {
                            if(Level == m_Level.maximum)
                            {
                                GameState = GameStates.GameOver;
                            }
                            yield return null;
                        }
                        m_PlayerManager.SetEnabled(false);
                        break;
                    case GameStates.GameOver:
                        yield return new WaitForSeconds(2);
                        m_TargetsManager.RoundState = RoundStates.None;
                        m_HUDManager.GameOver();
                        while(GameState == GameStates.GameOver)
                        {
                            yield return null;
                        }
                        break;
                    default:
                        break;
                }
                yield return null;
            }
        }

        public void RestartLevel()
        {
            OnLevelChanged -= GameManager_OnLevelChanged;
            OnRoundChanged -= GameManager_OnRoundChanged;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
