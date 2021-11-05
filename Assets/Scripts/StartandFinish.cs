using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script for the start and finish line
//enabling a timer for the race
public class StartandFinish : MonoBehaviour
{
    
    public float raceTime;
    [SerializeField] private Text rTime;
    private bool startTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        rTime.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer == true)
        {
            raceTime = raceTime + Time.deltaTime;
            //Debug.Log(raceTime);
            rTime.text = "Time: " + raceTime.ToString("F2");
            
        }
    }
    
    //once car passes line then timer will start and when car passes finish timer will stop
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Start"))
        {
            rTime.enabled = true;
            Debug.Log("car passed over the start line");
            startTimer = true;
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            startTimer = false;
            Debug.Log("car passed over the finish line");
        }
    }
}
