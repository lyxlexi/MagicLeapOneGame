using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class PlaceObject : MonoBehaviour
{
    public GameObject ObjectToPlace;
    private MLInputController controller;
    private GameObject objectPlaced;

    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;
        controller = MLInput.GetController(MLInput.Hand.Left);
    }

    void OnButtonDown(byte controller_id, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            RaycastHit hit;
            if (Physics.Raycast(controller.Position, transform.forward, out hit))
            {
                objectPlaced = Instantiate(ObjectToPlace, hit.point, Quaternion.Euler(hit.normal));
            }
        }
    }

    void OnButtonUp(byte controller_id, MLInputControllerButton button) {
         if (button == MLInputControllerButton.Bumper)
        {
            if (objectPlaced != null){
                Destroy(objectPlaced);
            }
        }
    }
    
    private void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;
        MLInput.Stop();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
