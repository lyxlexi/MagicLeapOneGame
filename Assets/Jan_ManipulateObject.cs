using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class Jan_ManipulateObject : MonoBehaviour
{
    private MLInputController controller;
    GameObject selectedPlanet;
    GameObject selectedHolder;
    public GameObject attachPoint;
    public GameObject controllerObject;
    public GameObject ObjectToPlace;
    public Text contentText;
    public Text planetNameText;
    private bool trigger;
    private RectTransform rectTransform;
    private bool collidedOnTriggerRelease;
    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);
        rectTransform = contentText.GetComponent<RectTransform>();
    }
    void UpdateTriggerInfo()
    {
        if (controller.TriggerValue > 0.8f)
        {
            if (trigger == true)
            {
                RaycastHit hit;
                if (Physics.Raycast(controller.Position, transform.forward, out hit))
                {
                    if (hit.transform.gameObject.tag == "AstronomicalBody")
                    {
                        selectedPlanet = hit.transform.gameObject;
                        selectedHolder = selectedPlanet.transform.parent.gameObject;
                        
                        SphereCollider Jan = selectedPlanet.GetComponent<SphereCollider>();
                        attachPoint.transform.position = hit.transform.position;
                        
                        if (ObjectToPlace != null) {
                            ObjectToPlace.transform.SetParent(selectedHolder.transform, false);
                            ObjectToPlace.transform.position = selectedPlanet.transform.position;
                        }

                        LoadTextToScrollBar(selectedPlanet.name);
                    }

                }
                trigger = false;
            } 
        } 

        if (controller.TriggerValue < 0.2f)
        {
            trigger = true;
            if (selectedPlanet != null)
            {
                SphereCollider Jan = selectedPlanet.GetComponent<SphereCollider>();
                collidedOnTriggerRelease = selectedPlanet.GetComponent<FollowOrbit>().Collided();
                Debug.Log(collidedOnTriggerRelease);
                FollowOrbit fo = selectedPlanet.GetComponent<FollowOrbit>();
                selectedPlanet = null; 
                if (collidedOnTriggerRelease) {
                    fo.StartFollowOrbit();
                }
            }
            if (ObjectToPlace != null) {
                ObjectToPlace.transform.position = new Vector3(-100, 0, 0);
            }
        
        }

    }


    public void LoadTextToScrollBar(string name)
    {
        string contentToPlace = "Setup content for " + name + " in script Jan_ManiipulateObject"; 
        
        //add description content here
        if (name == "Jupiter") {
            contentToPlace = "Gas Giant\nLargest Planet in the Solar System";
        }

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, contentToPlace.Length * 5);
        contentText.text = string.Empty;
        contentText.text += contentToPlace;
        planetNameText.text = name;
        
    }

    private void OnDestroy()
    {
        MLInput.Stop();
    }

    void UpdateTouchPad()
    {
        if (controller.Touch1Active)
        {
            float x = controller.Touch1PosAndForce.x;
            float y = controller.Touch1PosAndForce.y;
            float force = controller.Touch1PosAndForce.z;

            if(force > 0)
            {
                if (x > 0.5 || x < -0.5)
                {
                    selectedPlanet.transform.localScale += selectedPlanet.transform.localScale * x * Time.deltaTime;
                }
                if(y > 0.3 || y < -0.3)
                {
                    attachPoint.transform.position = Vector3.MoveTowards(attachPoint.transform.position, gameObject.transform.position, -y * Time.deltaTime);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = controller.Position;
        transform.rotation = controller.Orientation;

        if (selectedPlanet)
        {
            //move
            selectedHolder.transform.position = attachPoint.transform.position;
            //spin
            selectedPlanet.transform.Rotate(-Vector3.up * Time.deltaTime * 100 * ConfigManager.instance.orbitSpeedInDaysPerSecond);
        }
        UpdateTriggerInfo();
    }
}
