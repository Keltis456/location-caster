using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(3840, 2160, true);
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
