using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour , IInteractable
{
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IInteractable.Interact(ShipManager manager)
    {

    }

    void IInteractable.Interact(ManagerToObjectivePacket dataPacket, ShipManager manager)
    {
        //datapacket tells the object to update certain things like its mesh

    }
}
