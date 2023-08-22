using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    const float standardDrillRotation = 0;
    float currentElevation = 0;
    [SerializeField] const float drillRotationSpeed = 0;




    //negatives to turn left, positives to turn right
    void TurnDrill(float rotation)
    {
        //lerp the rotation toward the angle and the rotation speed
    }
}
