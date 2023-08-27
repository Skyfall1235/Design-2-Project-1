using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterConsole : MonoBehaviour
{

    [SerializeField] GameObject[] console = new GameObject[2];
    [SerializeField] ShipManager shipManager;
    [SerializeField] ShipMovement shipMovement;
    [SerializeField]
    GameObject drillObject
    {
        get { return gameObject; }
    }


    [SerializeField] Slider energySlider;
    [SerializeField] Slider RMPSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateMonitors()
    {
        UpdateMonitorOne();
        UpdateMonitorTwo();
        UpdateMonitorThree();
    }

    void UpdateMonitorOne()
    {
        //handles  energy, engine rpm,elevation, and direction
        //use the groundplane for this
        string direction = GetDirection(shipMovement.groundPad);
        //string 
    }
    void UpdateMonitorTwo()
    {

    }
    void UpdateMonitorThree()
    {

    }




    private string GetDirection(GameObject pad)
    {
        Vector3 forwardDirection = pad.transform.forward;
        float angle = Vector3.SignedAngle(forwardDirection, Vector3.forward, Vector3.up);

        if (angle < -45 && angle >= -135)
        {
            return "West";
        }
        else if (angle >= -45 && angle < 45)
        {
            return "North";
        }
        else if (angle >= 45 && angle < 135)
        {
            return "East";
        }
        else
        {
            return "South";
        }
    }

    
}
