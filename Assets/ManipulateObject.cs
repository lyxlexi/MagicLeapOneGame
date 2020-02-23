using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ManipulateObject : MonoBehaviour
{
    private MLInputController controller;
    GameObject selectedGameObject;
    public Vector3 attachPoint;
    public GameObject controllerObject;
    bool trigger;
    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
    }

    void UpdateTriggerInfo()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
