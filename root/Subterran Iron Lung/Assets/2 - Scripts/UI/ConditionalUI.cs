using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConditionalUI : MonoBehaviour
{

    [SerializeField] public GameObject[] panels;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteLevelOne()
    {
       
        panels[3].SetActive(false);   
    }
    public void CompleteLevelTwo()
    {
        
        panels[4].SetActive(false);
    }
    public void CompleteLevelThree()
    {
        
        panels[5].SetActive(false);   
    }
    public void RetryLevelOne()
    {
        
        panels[0].SetActive(false);
    }
    public void RetryLevelTwo()
    {
        
        panels[1].SetActive(false);
    }
    public void RetryLevelThree()
    {
        
        panels[2].SetActive(false);
    }
}

