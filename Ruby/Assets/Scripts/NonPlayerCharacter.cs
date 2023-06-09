using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime=4.0f;
    public GameObject dialogBox;
    float timeDisplayed;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timeDisplayed = -1.0f; 
    }

    // Update is called once per frame
    void Update()
    {
    

        if (timeDisplayed >= 0)
        {
            timeDisplayed -= Time.deltaTime;
            if (timeDisplayed < 0)
            {
                dialogBox.SetActive(false);
            }
        }   
    }

    public void DisplayDialog()
    {
        timeDisplayed = displayTime;
        dialogBox.SetActive(true);
    }
}
