using UnityEngine;

namespace Runtime.Services
{
    public class HandService
    {
        
       void ChangePositionHands(Transform hands,Vector3 positionToChange)
       {
          hands.gameObject.transform.position = positionToChange;
       }
       void HandBobbing(float currentVelocity)
       {
            
       }
    }
}