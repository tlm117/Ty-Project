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
using UnityEngine.Events;

namespace CandyHunt
{
    public delegate void PointerTargeting();
    public delegate void ReadyCoundown(int second);

    public delegate void RoundChanged(int round);

    public delegate void GameStateChanged(GameStates gameState);

    public delegate void LevelChanged(int level);
    public delegate void ScoreChanged(int score);
    public delegate void AmmoChanged(int ammo);

    public delegate void RoundTimeLeft(float timeRemaining);
    public delegate void RoundStateChanged(RoundStates roundState);

    public delegate void ProjectileFired(ProjectileBehaviour projectileBehaviour);
}
