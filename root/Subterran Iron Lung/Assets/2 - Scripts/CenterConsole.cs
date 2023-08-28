using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CenterConsole : MonoBehaviour
{

    [SerializeField] private GameObject[] console = new GameObject[2]; // Array of console GameObjects.
    [SerializeField] private ShipManager shipManager; // Reference to the ShipManager component.
    [SerializeField] private ShipMovement shipMovement; // Reference to the ShipMovement component.
    [SerializeField] private GameObject topOfMap; // Reference to the top of the map GameObject.

    [SerializeField]
    private GameObject drillObject
    {
        get { return gameObject; } // Reference to this GameObject.
    }

    [Header("Screen 1")]
    [SerializeField] private Slider energySlider; // Reference to the energy slider UI element.
    [SerializeField] private Slider RMPSlider; // Reference to the RPM slider UI element.
    [SerializeField] private TextMeshProUGUI screen1TMP_GUI; // Reference to TextMeshProUGUI for Screen 1.

    [Header("Screen 3")]
    [SerializeField] private GameObject canvas; // Reference to the canvas GameObject for Screen 3.
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
        UpdateMonitorThree();
    }

    void UpdateMonitorOne()
    {
        //handles  energy, engine rpm,elevation, and direction
        //use the groundplane for this
        string direction = GetDirection();
        //string 
    }
    void UpdateMonitorThree()
    {

    }






    private string GetDirection()
    {
        float angle = Vector3.SignedAngle(gameObject.transform.position, Vector3.forward, Vector3.up);

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
