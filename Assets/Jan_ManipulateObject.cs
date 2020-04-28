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
                        selectedHolder = hit.transform.gameObject;
                        selectedPlanet = selectedHolder.transform.GetChild(0).gameObject;
                        attachPoint.transform.position = hit.transform.position;
                        ShowTextBar(selectedPlanet.name);
                        selectedHolder.GetComponent<FollowOrbit>().PlaySound();
                    }

                }
                trigger = false;
            } 
        } 

        if (controller.TriggerValue < 0.2f)
        {
            trigger = true;
            performDeselectActions();
        }

    }

    void performDeselectActions(){
        if (selectedPlanet != null || selectedHolder != null)
            {
                selectedHolder.GetComponent<FollowOrbit>().StopSound();
                collidedOnTriggerRelease = selectedHolder.GetComponent<FollowOrbit>().Collided();
                Debug.Log(collidedOnTriggerRelease);
                if (collidedOnTriggerRelease) {
                    FollowOrbit fo = selectedHolder.GetComponent<FollowOrbit>();
                    fo.StartFollowOrbit();
                }
                selectedPlanet = null; 
                selectedHolder = null;
                
                if (ObjectToPlace != null) {
                ObjectToPlace.transform.position = new Vector3(-100, 0, 0);
                }
            }
    }


    public void ShowTextBar(string name)
    {

        if (ObjectToPlace != null) {
            ObjectToPlace.transform.SetParent(selectedHolder.transform, false);
            ObjectToPlace.transform.position = selectedPlanet.transform.position;
        }

        //temporary fix - to set up size for the textbox
        string contentToPlace = "The largest planet in our solar system.\n\nA spacecraft can’t land on its surface since there is no solid surface at all!\n\nThe ammonia in the upper layer of its atmosphere swirls around in tremendous storms — the Great Red Spot.";
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, contentToPlace.Length*2);
      
        contentToPlace = "Setup content for " + name + " in script Jan_ManiipulateObject"; 
        
        //add description content here

        switch (name)
        {
            case "Mercury":
                contentToPlace = "Mercury has no moon.\nMercury is the smallest planet. \nMercury is covered with craters, like Earth’s Moon.";
                break;
            case "Venus":
                contentToPlace = "Venus is the hottest planet.\nVenus is most like Earth in size.\nA day on Venus is longer than a year on Venus.";
                break;                    
            case "Earth":
                contentToPlace = "The only planet known to have life.\n\nThe only planet that we know has plate tectonics.\n\nRotates on its axis once every 24 hours.";
                break;
            case "Mars":
                contentToPlace = "Viewed from Earth, Mars is red, due to large amounts of iron in the soil.\nMars is home to the largest volcano in the solar system.\nMars also has the largest canyon in the solar system.";
                break;
            case "Jupiter":
                contentToPlace = "The largest planet in our solar system.\n\nA spacecraft can’t land on its surface since there is no solid surface at all!\n\nThe ammonia in the upper layer of its atmosphere swirls around in tremendous storms — the Great Red Spot.";
                break;
            case "Saturn":
                contentToPlace = "Saturn is famous for its beautiful rings.\nThe second largest planet in the solar system.\nSaturn is the least dense planet in our solar system.";
                break;
            case "Uranus":
                contentToPlace = "Unlike all other planets in the solar system, Uranus is tilted on its side.\nIts rings are almost perpendicular to the planet’s orbit.\nUranus is an icy blue green ball - Clouds of methane filter out red light.";
                break;
            case "Neptune":
                contentToPlace = "Neptune is very cold and has very strong winds.\nNeptune is very similar to Uranus\nIt had a large dark spot that disappeared. Another dark spot appeared on another part of the planet. These dark spots are storms in Neptune’s atmosphere.";
                break;
        }
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
                    attachPoint.transform.position = Vector3.MoveTowards(
                        attachPoint.transform.position, 
                        gameObject.transform.position, 
                        -y * Time.deltaTime);

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
            selectedPlanet.transform.Rotate(-Vector3.up * Time.deltaTime * 1 * ConfigManager.instance.orbitSpeedInDaysPerSecond);
            DeselectIfOutHit();
        }
        UpdateTriggerInfo();
    }

    void DeselectIfOutHit(){
        if (selectedPlanet) {
            RaycastHit hit;
                if (Physics.Raycast(controller.Position, transform.forward, out hit))
                {
                    if (!selectedHolder.Equals(hit.transform.gameObject))
                    {
                        performDeselectActions();
                    }

                }
        }
    }
}
