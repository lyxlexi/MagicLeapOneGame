using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jan_changeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(Application.loadedLevel == 0)
            {
                Application.LoadLevel(1);
            }
            else
            {
                Application.LoadLevel(0);
            }
        }
    }
}
