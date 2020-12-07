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

    [AddComponentMenu("Pool Everything/Samples/Candy Hunt/PointerAim")]
    public class PointerAim : Aimer
    {

        void Update()
        {
            if(Input.GetMouseButton(0))
            {
                AimNozzle(Input.mousePosition);
            }
            else if(Input.touchCount == 1)
            {
                var touch = Input.GetTouch(0);
                if(touch.phase != TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    AimNozzle(touch.position);
                }
            }
        }

    }
}
