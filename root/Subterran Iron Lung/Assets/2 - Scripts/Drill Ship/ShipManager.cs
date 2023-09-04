using System;
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

    [SerializeField] CenterConsole m_console;
    [SerializeField] Light[] m_consoleLight;
    [Header("Objectives")]
    public Objective m_currentObjective;
    public ObjectiveTask m_currentTask;
    [SerializeField] List<Objective> m_missionObjectives;
    [SerializeField] GameObject controlScheme;
    [SerializeField] GameObject player;
    public bool showControls;
    [SerializeField] private bool lightIsFlashing = false;


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
            showControls = true;
            Debug.Log("cursor should be showing");
            Cursor.visible = true;
            ShowControlScheme(showControls);
            player.GetComponent<PlayerController>().useControls = false;
        }

        //DEBUG
        StartCoroutine(AlternateLights());
    }


    // Update is called once per frame
    void Update()
    {
        DetectChange();
    }

    public void ShowControlScheme(bool showControls)
    {
        controlScheme.SetActive(showControls);
        player.GetComponent<PlayerController>().useControls = !showControls;
        Cursor.visible = showControls;
        if(showControls)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        

    }


    private IEnumerator AlternateLights()
    {
        const float interval = 1.0f;

        //preload the alteration
        m_consoleLight[0].enabled = true;
        m_consoleLight[2].enabled = true;

        while (lightIsFlashing)
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
    private void DetectChange()
    {
        EventTriggerType newEvent = m_currentEvent; // Start with the current event

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            newEvent = EventTriggerType.EngineMalfunction;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newEvent = EventTriggerType.ReactorMalfunction;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            newEvent = EventTriggerType.Biological;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            newEvent = EventTriggerType.None;
        }

        if (newEvent != m_currentEvent)
        {
            m_currentEvent = newEvent;
            Debug.Log($"Event changed to: {m_currentEvent}");
            m_console.UpdateMonitorThree();
        }
    }


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

    void IInteractable.Interact(ShipManager manager)
    {

    }

    void IInteractable.Interact(ShipManager manager, ManagerToObjectivePacket dataPacket)
    {
        //datapacket tells the object to update certain things like its mesh

    }

}
