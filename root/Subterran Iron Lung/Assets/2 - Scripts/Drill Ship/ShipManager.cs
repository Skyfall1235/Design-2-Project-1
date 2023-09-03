using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        SetupTasks();
        AssignNextObjectiveAndTask();
        DebugTask(m_currentTask);
    }


    // Update is called once per frame
    void Update()
    {
        DetectChange();
    }

    //DEBUG
    private void DetectChange()
    {
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_currentEvent = EventTriggerType.EngineMalfunction;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                m_currentEvent = EventTriggerType.ReactorMalfunction;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                m_currentEvent = EventTriggerType.Biological;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                m_currentEvent = EventTriggerType.None;
            }

            // Call a method to handle the changed event
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
