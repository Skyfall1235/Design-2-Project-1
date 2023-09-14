using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
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

    [SerializeField] public CenterConsole m_console;
    [SerializeField] private Engine m_engine;
    [SerializeField] Light[] m_consoleLight;
    public AsyncLoader m_loader;

    [Header("Objectives")]
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_controlScheme;
    [SerializeField] TextMeshProUGUI m_countdownInteractionText;
    [SerializeField] private GameObject endPanel;

    [SerializeField] private TextMeshProUGUI m_taskText;
    public Objective m_currentObjective;
    public ObjectiveTask m_currentTask;
    [SerializeField] private List<Objective> m_missionObjectives;

    [Header("Ship States")]
    [SerializeField] private bool m_showControls;
    [SerializeField] private bool m_lightIsFlashing = false;
    [SerializeField] private AudioSource m_consoleSource;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    int lengthOfProblemDuration = 15;

    //[Header("Audio settings and References")]



    // Start is called before the first frame update
    void Start()
    {

        Invoke("SetUpObjectives", 2f);




        // "Level_1" scene is open, you can run code in it
        m_showControls = true;
        Debug.Log("cursor should be showing");
        Cursor.visible = true;
        ShowControlScheme(m_showControls);
        m_player.GetComponent<PlayerController>().useControls = false;

        StartCoroutine(RandomTimerCoroutine());
        //DEBUG
        //StartCoroutine(AlternateLights());
    }

    void SetUpObjectives()
    {
        m_missionObjectives = FindObjectivesLoaderInPlayerScene().m_missionObjectives;
        SetupTasks();
        AssignNextObjectiveAndTask();
        DebugTask(m_currentTask);
    }

    

    public Objectives_loader FindObjectivesLoaderInPlayerScene()
    {
        // Loop through all active scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            // Check if the scene is named "Player"
            if (scene.name == "Level_1")
            {
                // Find the ShipManager component in the "Player" scene
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    Objectives_loader loader = obj.GetComponent<Objectives_loader>();
                    if (loader != null)
                    {
                        // ShipManager found, return it
                        return loader;
                    }
                }
            }
        }

        // If no "Player" scene or ShipManager is found, return null
        return null;
    }




    // Update is called once per frame
    void Update()
    {
        //DetectChange();
    }


    void EndGame()
    {
        m_player.GetComponent<PlayerController>().useControls = false;
        endPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToMenu()
    {
        m_loader.ReturnToMenu();
    }

    public void CompleteCurrentTaskInObjective()
    {
        m_currentTask.taskCompleted = true;
        AssignNextObjectiveAndTask();
    }


    private IEnumerator AlternateLights()
    {
        const float interval = 1.0f;
        GlobalMethods.PlaySoundAtLocation(SoundType.SoundEffect, "Warnings", 0, m_consoleSource, 0.7f);
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
            m_consoleSource.Stop();
        }

    }


    private IEnumerator RandomTimerCoroutine()
    {
        while (true)
        {
            
            // Generate a random waiting time between 1 and 5 seconds (adjust the range as needed).
            float randomWaitTime = Random.Range(lengthOfProblemDuration, lengthOfProblemDuration + 25);

            Debug.Log(randomWaitTime);

            // Wait for the random time.
            yield return new WaitForSeconds(randomWaitTime);

            // Call the specified method.
            DetermineShipProblem();
        }
    }

    /// <summary>
    /// Checks if the current event is the same as before. if not, sets the new event to the current and calls an update to the monitor
    /// </summary>
    /// <param name="newEvent">The event we would like to pass to the ship problem handler  </param>
    public void DetectChange(EventTriggerType newEvent)
    {
        Debug.Log($" current event:{m_currentEvent}  New Event: {newEvent}");
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
        choice = 0;
        timeOutForEvent = Random.Range(15, 15+25);
        switch (choice)
        {
            case 0:
                //ship engine problem
                m_engine.m_hasEngineProblem = true;
                StartCountdown(timeOutForEvent, EventTriggerType.EngineMalfunction);
                m_lightIsFlashing = true;
                StartCoroutine(AlternateLights());
                break;
            case 1:
                //ship reactor problem
                StartCountdown(timeOutForEvent, EventTriggerType.ReactorMalfunction);
                m_lightIsFlashing = true;
                StartCoroutine(AlternateLights());
                break;
            default:
                break;
        }

    }


    #region Countdown Handlers
    public void StartCountdown(float duration, EventTriggerType type)
    {
        DetectChange(type);
        // Stop any existing countdown coroutine.
        if (m_countdownCoroutine != null)
        {
            StopCoroutine(m_countdownCoroutine);
        }
        Debug.Log("starts the blinking lights");
        
        // Start a new countdown coroutine.
        m_countdownCoroutine = StartCoroutine(CountdownCoroutine(duration));
        
    }

    // Cancel the countdown
    public void CancelCountdown()
    {
        Debug.Log("canceled countdown");
        // Stop the countdown coroutine if it's running
        if (m_countdownCoroutine != null)
        {
            StopCoroutine(m_countdownCoroutine);
            m_countdownCoroutine = null;
            //this method will reset the events
            DetectChange(EventTriggerType.None);
            m_lightIsFlashing = false;
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

                if (task.interactableObject != null)
                {
                    task.targetLocation = task.interactableObject.transform.position;

                    // If you want to update the modified task back into the list, you would do something like this:
                    objective.objectiveTasks[index] = task;
                }
                else
                {
                    Debug.LogWarning("InteractableObject is null for ObjectiveTask at index " + index);
                    // Handle the case where the interactableObject is null, e.g., skip the task or log a warning.
                }
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

    void UpdateTaskText()
    {
        if (m_taskText != null)
        {
            if (m_currentObjective.Equals(default(Objective)) || m_currentTask.Equals(default(ObjectiveTask)))
            {
                // Handle the case where m_currentObjective or m_currentTask is the default value
                m_taskText.text = "No available objectives or tasks.";
            }
            else
            {
                // Construct the task text with relevant information
                string taskInfo = $"Objective: {m_currentObjective.objectiveDescription}\n\n" +
                    $"Current task: {m_currentTask.objectiveObjectName}\n\n" +
                    $"Completion Status: {(m_currentTask.taskCompleted ? "Completed" : "Incomplete")}";

                // Set the task text in your TextMeshPro component
                m_taskText.text = taskInfo;
            }
        }
    }

    //to go to next objective, we just need to flip the completed bool
    private int currentObjectiveIndex = 0;

    /// <summary>
    /// Assigns the next available Objective and Task.
    /// </summary>
    private void AssignNextObjectiveAndTask()
    {
        while (currentObjectiveIndex < m_missionObjectives.Count)
        {
            Objective objective = m_missionObjectives[currentObjectiveIndex];

            if (!objective.objectiveCompleted)
            {
                m_currentObjective = objective;
                bool allTasksCompleted = true; // Assume all tasks are completed initially

                // Use a separate variable to track the current task
                ObjectiveTask currentTask = new();

                foreach (ObjectiveTask task in objective.objectiveTasks)
                {
                    if (!task.taskCompleted)
                    {
                        currentTask = task; // Assign the current task
                        UpdateTaskText();
                        Debug.Log($"Assigned Objective: {m_currentObjective.objectiveDescription}");
                        Debug.Log($"Assigned task object: {currentTask.objectiveObjectName}");
                        return; // Exit the loop once a task is assigned
                    }
                    else
                    {
                        allTasksCompleted = false; // At least one task is not completed
                    }
                }

                // Check if all tasks within the objective are completed
                if (allTasksCompleted)
                {
                    m_currentObjective.objectiveCompleted = true; // Mark the objective as completed
                    Debug.Log($"Objective completed: {m_currentObjective.objectiveDescription}");
                    currentObjectiveIndex++; // Move to the next objective
                }

                // If the current task was completed, assign the next task
                if (currentTask.taskCompleted)
                {
                    m_currentTask = currentTask;
                }
            }
            else
            {
                currentObjectiveIndex++; // Move to the next objective if the current one is already completed
            }
        }

        // If no uncompleted objective/task is found
        Debug.Log("No available objectives or tasks.");
    }




    bool isInteracting = false;

    /// <summary>
    /// Initiates a countdown for a specific interaction type.
    /// </summary>
    /// <param name="time">The duration of the countdown in seconds.</param>
    /// <param name="type">The type of interaction (e.g., drilling).</param>
    /// <returns>An IEnumerator for managing the countdown.</returns>
    public IEnumerator DrillActionCountdown(float time, InteractableType type)
    {
        float currentTimeOnDrillActionCountdown = time;
        while (currentTimeOnDrillActionCountdown > 0)
        {
            isInteracting = true;
            yield return new WaitForSeconds(1.0f); // Wait for one second
            currentTimeOnDrillActionCountdown -= 1.0f;
            UpdateCountdownText(currentTimeOnDrillActionCountdown, type);
        }
        isInteracting = false;
        UpdateCountdownText(currentTimeOnDrillActionCountdown, type);
        // Countdown completed
        Debug.Log("Countdown completed!");
        currentTimeOnDrillActionCountdown = 0f;
        m_currentTask.taskCompleted = true;
        CompleteCurrentTaskInObjective();
    }


    /// <summary>
    /// Updates the countdown interaction text based on the remaining time.
    /// </summary>
    /// <param name="time">The remaining time in seconds.</param>
    /// <param name="type">The type of interaction (e.g., drilling).</param>
    private void UpdateCountdownText(float time, InteractableType type)
    {
        Debug.Log("updating interaction countdown");
        if (m_countdownInteractionText != null)
        {
            if(isInteracting)
            {
                string timeRemaining = time.ToString("F1");
                string typeOfInteraction = type.ToString();
                m_countdownInteractionText.text = $"Processing {typeOfInteraction}\nRemaining time:\nT-{timeRemaining} seconds"; // Display time with one decimal place
            }
            else
            {
                m_countdownInteractionText.text = $"";
            }
        }
    }


    #endregion


    #region interaction calls
    void IInteractable.Interact(ShipManager manager)
    {
        
    }

    void IInteractable.Interact(ShipManager manager, ManagerToObjectivePacket dataPacket)
    {
        //datapacket tells the object to update certain things like its mesh
        StartCoroutine(DrillActionCountdown(dataPacket.timeRequiredForInteraction, dataPacket.interactableType));
    }
    #endregion

}
