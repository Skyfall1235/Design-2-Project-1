using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class Section: 2023FA.SGD.212.4174
//Date: 8/19/2023
//Names: Samuel,Wyatt,Ray

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the input from the player
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Calculates the movement direction
        Vector3 movement = transform.TransformDirection(new Vector3(moveHorizontal, 0.0f, moveVertical)).normalized;
        
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
}
