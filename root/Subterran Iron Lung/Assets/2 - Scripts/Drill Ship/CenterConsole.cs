using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CenterConsole : MonoBehaviour
{
    [SerializeField] private GameObject[] m_console = new GameObject[2]; // Array of console GameObjects.
    [SerializeField] private ShipManager m_shipManager; // Reference to the ShipManager component.
    [SerializeField] private ShipMovement m_shipMovement; // Reference to the ShipMovement component.
    [SerializeField] private GameObject m_topOfMap; // Reference to the top of the map GameObject.

    [SerializeField]
    private GameObject m_drillObject
    {
        get { return gameObject; } // Reference to this GameObject.
    }

    [Header("Screen 1")]
    [SerializeField] private Slider m_energySlider; // Reference to the energy slider UI element.
    [SerializeField] private Slider m_RMPSlider; // Reference to the RPM slider UI element.
    [SerializeField] private TextMeshProUGUI m_screen1TMP_GUI; // Reference to TextMeshProUGUI for Screen 1.

    [Header("Screen 3")]
    [SerializeField] private GameObject m_canvas; // Reference to the canvas GameObject for Screen 3.
    [SerializeField] private GameObject[] m_EventsPanels = new GameObject[4];
    [SerializeField] private TextMeshProUGUI m_consoleLog;
    [SerializeField]
    private EventTriggerType m_shipsCurrentEvent
    {
        get { return m_shipManager.m_currentEvent; }
    }

    /// <summary>
    /// List of console log lines.
    /// </summary>
    private List<string> m_consoleLogs = new List<string>();

    /// <summary>
    /// Maximum number of lines to maintain in the console log.
    /// </summary>
    private const int m_maxLinesOnConsole = 6;

    #region basic methods
    private void Start()
    {
        m_topOfMap = GameObject.FindWithTag("TopOfMap");
    }
    void FixedUpdate()
    {
        UpdateMonitors();
    }
    #endregion

    /// <summary>
    /// combines the updates into a specified sequence so we dont have to handle that in update
    /// </summary>
    void UpdateMonitors()
    {
        UpdateMonitorOne();
        //DetermineRenderTextureUse();
    }

    #region public methods

    /// <summary>
    /// updates the information displayed on monitor three
    /// </summary>
    /// <remarks>This should be called through the ship manager, when events occur instead of just being an update thing</remarks>
    public void UpdateMonitorThree()
    {
        //if the console doesnt have a warning, show the logs for the data
        if (DetermineConsoleOutput(m_shipsCurrentEvent))
        {
            string fullConsoleText = string.Join("", m_consoleLogs.ToArray());
            m_consoleLog.text = fullConsoleText;
        }
        //else gets covered by the method being called anyway
    }


    string preppedText;
    /// <summary>
    /// Adds a new text line to the console log.
    /// </summary>
    /// <param name="text">The text to add to the console log.</param>
    public void AddTextToConsole(string text)
    {
        // Prep text
        preppedText = $"\n{text}";

        // Add text to line
        m_consoleLogs.Add(preppedText);

        // Remove old text if exceeding the maximum lines
        if (m_consoleLogs.Count > m_maxLinesOnConsole)
        {
            m_consoleLogs.RemoveAt(0); // Remove the oldest line
        }
    }
    #endregion


    #region Update Monitors

    string direction;
    float elevation;
    string assembly;

    /// <summary>
    /// Updates the information diplayed on monitor one
    /// </summary>
    private void UpdateMonitorOne()
    {
        //handles  energy, engine rpm,elevation, and direction
        //use the groundplane for this
        direction = GetDirection();
        elevation = -ElevationCalc();

        assembly = $"Elevation - {elevation - 12000} \n Direction - {direction} \n energy Consumption - N/A \n \n Engine RPM - N/A";
        m_screen1TMP_GUI.text = assembly;
    }


    void DetermineRenderTextureUse()
    {
        // Check if the Render Texture's position is within the camera's view.
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);

        // If the viewport point is outside the camera's view, turn off the Render Texture.
        if (viewportPoint.x < 0f || viewportPoint.x > 1f || viewportPoint.y < 0f || viewportPoint.y > 1f)
        {
            m_console[1].SetActive(false);
        }
        else
        {
            m_console[1].SetActive(true);
        }
    }

    #endregion


    /// <summary>
    /// Determines the console output and controls the active state of event panels.
    /// </summary>
    /// <param name="triggerType">The type of event trigger.</param>
    /// <returns>True if event is not a warning, else false</returns>
    private bool DetermineConsoleOutput(EventTriggerType triggerType)
    {
        switch (triggerType)
        {
            case EventTriggerType.EngineMalfunction:
                // Handle Engine Malfunction event
                ToggleGameObjects(m_EventsPanels, false);

                AlertOnConsoleThree(m_EventsPanels[1]);
                return false;

            case EventTriggerType.ReactorMalfunction:
                // Handle Reactor Malfunction event
                ToggleGameObjects(m_EventsPanels, false);
                AlertOnConsoleThree(m_EventsPanels[2]);
                return false;

            case EventTriggerType.Biological:
                // Handle Biological event
                ToggleGameObjects(m_EventsPanels, false);
                Debug.Log("event occurs and calls for console");
                AlertOnConsoleThree(m_EventsPanels[3]);
                return false;

            case EventTriggerType.ConsoleWarning:
                // Handle Console Warning event
                ToggleGameObjects(m_EventsPanels, false);
                m_EventsPanels[0].SetActive(true);
                AddTextToConsole("unknown event, proceed with caution");
                return false;

            case EventTriggerType.Rumble:
                // Handle Rumble event
                ToggleGameObjects(m_EventsPanels, false);
                m_EventsPanels[0].SetActive(true);
                AddTextToConsole("tectonic shift. Please continue the mission.");
                return true;

            case EventTriggerType.SoundTrigger:
                // Handle Sound Trigger event
                ToggleGameObjects(m_EventsPanels, false);
                AddTextToConsole("unknown Lifeform in the vicinity. Proceed with Cuation");
                m_EventsPanels[0].SetActive(true);

                return true;

            case EventTriggerType.ConsoleAndLights:
                // Handle Console and Lights event
                ToggleGameObjects(m_EventsPanels, false);
                m_EventsPanels[0].SetActive(true);
                AddTextToConsole("unknown lifeform approaching. Are you sure you are supposed to be this far down?");
                return true;

            case EventTriggerType.All:
                // Handle All events at once
                ToggleGameObjects(m_EventsPanels, false);
                m_EventsPanels[0].SetActive(true);
                AddTextToConsole("You went too far down...");
                return true;

            case EventTriggerType.None:
                ToggleGameObjects(m_EventsPanels, false);
                m_EventsPanels[0].SetActive(true);
                return true;

        }
        // Handle unknown case
        Debug.LogWarning($"unknown case found? : {triggerType}");
        return false;
    }


    #region Mathmatics behind Update Calls

    /// <summary>
    /// Determines the depth of an object relative to the top of the map
    /// </summary>
    /// <returns>The height difference as an absolute value</returns>
    private float ElevationCalc()
    {
        if (m_topOfMap != null)
        {
            float heightDifference = gameObject.transform.position.y - m_topOfMap.transform.position.y;
            return Mathf.Abs(heightDifference);
        }
        else return 0.0f;
    }

    private float HandleEnergyCalc()
    {
        return 0.0f;
    }

    /// <summary>
    /// Calculates and returns the cardinal direction based on the forward vector of the GameObject.
    /// </summary>
    /// <returns>The cardinal direction (e.g., "North", "Northeast", etc.) as a string.</returns>
    private string GetDirection()
    {
        float angle = Vector3.SignedAngle(gameObject.transform.forward, Vector3.forward, Vector3.up);
        angle += 180; // Shifting the angle to be between 0 and 360 degrees

        if (angle >= 0 && angle < 22.5f)
        {
            return "North";
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            return "Northeast";
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            return "East";
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            return "Southeast";
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            return "South";
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            return "Southwest";
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            return "West";
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            return "Northwest";
        }
        else // Angle is between 337.5 and 360 degrees
        {
            return "North";
        }
    }

    private bool isFlashing = false;
    private void AlertOnConsoleThree(GameObject warningPanel)
    {
        Debug.Log($" flashing state is: {isFlashing}");
        if (!isFlashing)
        {
            StartCoroutine(HandleFlashing(warningPanel));
        }
    }

    IEnumerator HandleFlashing(GameObject panel)
    {
        isFlashing = true; // Mark flashing as active

        while (isFlashing && m_shipsCurrentEvent != EventTriggerType.None)
        {
            panel.SetActive(true);
            yield return new WaitForSeconds(2);

            panel.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        panel.SetActive(false); // Ensure the panel is off when flashing ends
        isFlashing = false; // Mark flashing as inactive
    }

    /// <summary>
    /// Toggles the active state of an array of GameObjects.
    /// </summary>
    /// <param name="objectsToToggle">The array of GameObjects to toggle.</param>
    /// <param name="isActive">The desired active state (true for active, false for inactive).</param>
    private void ToggleGameObjects(GameObject[] objectsToToggle, bool isActive)
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(isActive);
        }
    }

    #endregion

}
