using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipManager : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractableType m_interactableType; // The type of interactable object that this is connected to

    /// <summary>
    /// Gets or sets the type of this interactable object.
    /// </summary>
    public InteractableType InteractableType
    {
        get { return m_interactableType; }
        set { m_interactableType = value; }
    }

    /// <summary>
    /// The current event trigger type for the interactable object.
    /// </summary>
    public EventTriggerType m_currentEvent = EventTriggerType.None;

    [SerializeField] private Coroutine m_countdownCoroutine;

    [SerializeField] CenterConsole m_console;
    [SerializeField] Light[] m_consoleLight;

    [Header("Objectives")]
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_controlScheme;

    public Objective m_currentObjective;
    public ObjectiveTask m_currentTask;
    [SerializeField] private List<Objective> m_missionObjectives;

    [Header("Ship States")]
    [SerializeField] private bool m_showControls;
    [SerializeField] private bool m_lightIsFlashing = false;

    //[Header("Audio settings and References")]



    // Start is called before the first frame update
    void Start()
    {
        SetupTasks();
        AssignNextObjectiveAndTask();
        DebugTask(m_currentTask);

        Scene level1Scene = SceneManager.GetSceneByName("Level_1");
        if (level1Scene.isLoaded)
        {
            // "Level_1" scene is open, you can run code in it
            m_showControls = true;
            Debug.Log("cursor should be showing");
            Cursor.visible = true;
            ShowControlScheme(m_showControls);
            m_player.GetComponent<PlayerController>().useControls = false;
        }

        //DEBUG
        //StartCoroutine(AlternateLights());
    }


    // Update is called once per frame
    void Update()
    {
        //DetectChange();
    }


    void EndGame()
    {

    }

 


    private IEnumerator AlternateLights()
    {
        const float interval = 1.0f;

        //preload the alteration
        m_consoleLight[0].enabled = true;
        m_consoleLight[2].enabled = true;

        while (m_lightIsFlashing)
        {
            m_consoleLight[0].enabled = !m_consoleLight[0].enabled; 
            m_consoleLight[1].enabled = !m_consoleLight[1].enabled; 
            m_consoleLight[2].enabled = !m_consoleLight[2].enabled; 
            m_consoleLight[3].enabled = !m_consoleLight[3].enabled; 

            yield return new WaitForSeconds(interval);
        }
        //turn the lights off when its done
        for (int i = 0; i < m_consoleLight.Length; i++)
        {
            m_consoleLight[i].enabled = false;
        }

    }


    //DEBUG

    /// <summary>
    /// Checks if the current event is the same as before. if not, sets the new event to the current and calls an update to the monitor
    /// </summary>
    /// <param name="newEvent">The event we would like to pass to the ship problem handler  </param>
    private void DetectChange(EventTriggerType newEvent)
    {
        if (newEvent != m_currentEvent)
        {
            m_currentEvent = newEvent;
            Debug.Log($"Event changed to: {m_currentEvent}");
            m_console.UpdateMonitorThree();
        }
    }


    int choice;
    int timeOutForEvent;
    private void DetermineShipProblem()
    {
        choice = Random.Range(0, 3);
        timeOutForEvent = Random.Range(10, 26);
        switch (choice)
        {
            case 0:
                //ship engine problem
                m_currentEvent = EventTriggerType.EngineMalfunction;
                StartCountdown(timeOutForEvent);
                m_lightIsFlashing = true;
                StartCoroutine(AlternateLights());
                break;
            case 1:
                //ship reactor problem
                break;
            default:
                break;
        }

    }


    #region Countdown Handlers
    public void StartCountdown(float duration)
    {
        // Stop any existing countdown coroutine.
        if (m_countdownCoroutine != null)
        {
            StopCoroutine(m_countdownCoroutine);
        }

        // Start a new countdown coroutine.
        m_countdownCoroutine = StartCoroutine(CountdownCoroutine(duration));
    }

    // Cancel the countdown
    public void CancelCountdown()
    {
        // Stop the countdown coroutine if it's running
        if (m_countdownCoroutine != null)
        {
            StopCoroutine(m_countdownCoroutine);
            m_countdownCoroutine = null;
            //this method will reset the events
            DetectChange(EventTriggerType.None);
        }
    }

    // Coroutine that counts down for the specified duration
    private IEnumerator CountdownCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // This code executes when the countdown is not canceled in time
        Debug.Log("Loss condition triggered - Timer ran out!");
        EndGame();
    }


    #endregion


    #region UI methods
    public void ShowControlScheme(bool showControls)
    {
        m_controlScheme.SetActive(showControls);
        m_player.GetComponent<PlayerController>().useControls = !showControls;
        Cursor.visible = showControls;
        if (showControls)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    #endregion


    #region Tasks
    //setup of the tasks
    private void SetupTasks()
    {
        foreach (Objective objective in m_missionObjectives)
        {
            for (int index = 0; index < objective.objectiveTasks.Count; index++)
            {
                ObjectiveTask task = objective.objectiveTasks[index];
                task.targetLocation = task.interactableObject.transform.position;

                // If you want to update the modified task back into the list, you would do something like this:
                objective.objectiveTasks[index] = task;
            }
        }
    }

    void DebugTask(ObjectiveTask task)
    {
        // Printing the information using Debug.Log
        Debug.Log("Objective Task Information: \n" +
            $" Objective Object Name: {task.objectiveObjectName} \n" +
            $" Task Type: {task.type} \n" +
            $" Interactable Object: {task.interactableObject} \n" +
            $" Target Location: {task.targetLocation} \n" +
            $" Task Completed: {task.taskCompleted}");
    }

    private void AssignNextObjectiveAndTask()
    {
        foreach (Objective objective in m_missionObjectives)
        {
            if (!objective.objectiveCompleted)
            {
                m_currentObjective = objective;
                foreach (ObjectiveTask task in objective.objectiveTasks)
                {
                    if (!task.taskCompleted)
                    {
                        m_currentTask = task;
                        Debug.Log($"Assigned Objective: {m_currentObjective.objectiveDescription}");
                        Debug.Log($"Assigned task object: {m_currentTask.objectiveObjectName}");
                        return; // Exit the loop once a task is assigned
                    }
                }
            }
        }

        // If no uncompleted objective/task is found
        Debug.Log("No available objectives or tasks.");
    }

    #endregion


    #region interaction calls
    void IInteractable.Interact(ShipManager manager)
    {

    }

    void IInteractable.Interact(ShipManager manager, ManagerToObjectivePacket dataPacket)
    {
        //datapacket tells the object to update certain things like its mesh

    }
    #endregion

}
