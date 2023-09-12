using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Biological : BaseInteractactable
{

    [SerializeField] private float m_countdownTime = 5.0f; // Adjust this as needed
    Coroutine m_drillAction;
    [SerializeField] private BoxCollider m_boxCollider;
    bool hasBeenUsed = false;
    CenterConsole m_console;


    private void Start()
    {
        m_shipManager = FindShipManagerInPlayerScene();
        m_console = m_shipManager.m_console;
    }


    public override void Interact()
    {
        //if i can interact with it, im close enough. also, only a drill can interact with this anyway.
        Debug.Log("Beacon collided with drill, calling coroutine");
        m_drillAction = StartCoroutine(m_shipManager.DrillActionCountdown(m_countdownTime, InteractableType));
        hasBeenUsed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //this assumes the giant collider is within range not the interaction range
        //change the ships current event
        //sopund truigger is the sound of the biological
        
        //m_console.AddTextToConsole
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Drill") && m_drillAction != null) // Replace "YourObjectTag" with the tag of your object
        {
            StopCoroutine(m_shipManager.DrillActionCountdown(m_countdownTime, InteractableType));
            hasBeenUsed = false;
        }
    }

    public void RecieveDataFromColliders(ColliderNumber childSignature)
    {
        //process the outer child first since that will be the bigger collider
        if(childSignature == ColliderNumber.ChildTwo)
        {
            //sound trigger for biological
            m_shipManager.DetectChange(EventTriggerType.SoundTrigger);
            //play a sound at console?

        }
        else //its gonna be by child 1, no need to write more.
        {
            m_shipManager.DetectChange(EventTriggerType.SoundTrigger);
        }
    }


    private void TransferCollidersToChildren()
    {
        // Create and configure the first child GameObject.
        GameObject child1 = new GameObject("Child1");
        child1.transform.parent = transform;
        child1.AddComponent<ChildCollider>().Initialize(this, ColliderNumber.ChildOne);

        // Create and configure the second child GameObject.
        GameObject child2 = new GameObject("Child2");
        child2.transform.parent = transform;
        child2.AddComponent<ChildCollider>().Initialize(this, ColliderNumber.ChildTwo);

        // Get the colliders from Object 1.
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();

        // Copy the BoxCollider from Object 1 to child1.
        if (boxCollider != null)
        {
            BoxCollider child1Collider = child1.AddComponent<BoxCollider>();
            child1Collider.center = boxCollider.center;
            child1Collider.size = boxCollider.size;
            child1Collider.isTrigger = true;

            // Remove the BoxCollider from Object 1.
            Destroy(boxCollider);
        }

        // Copy the SphereCollider from Object 1 to child2.
        if (sphereCollider != null)
        {
            SphereCollider child2Collider = child2.AddComponent<SphereCollider>();
            child2Collider.center = sphereCollider.center;
            child2Collider.radius = sphereCollider.radius;
            child2Collider.isTrigger = true;

            // Remove the SphereCollider from Object 1.
            Destroy(sphereCollider);
        }
    }
}

public class ChildCollider : MonoBehaviour
{
    //get and store the biological this is attached to
    public Biological m_biologicalParent;
    public ColliderNumber m_colliderNumber;

    /// <summary>
    /// Intitialises the collider with values assigned from the initializer.
    /// </summary>
    /// <param name="parent">The parent biological script this child object syncs data with</param>
    /// <param name="number">The child this collider is sending data from</param>
    public void Initialize(Biological parent, ColliderNumber number)
    {
        m_biologicalParent = parent;
        m_colliderNumber = number;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drill"))
        {
            m_biologicalParent.RecieveDataFromColliders(m_colliderNumber);
        }
    }
}

public enum ColliderNumber
{ 
    ChildOne,
    ChildTwo,
}



