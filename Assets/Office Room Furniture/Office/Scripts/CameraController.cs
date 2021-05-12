using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera;
    public GameObject _gameObject;
    private bool checkMouseDrag = false;
    private Vector3 currentPositionRaycast;
    private Vector3 newPossionRaycast;
    private Vector3 mousePosition;
    private Vector3 newMousePosition;

    private RaycastHit vision;

    private GameObject curentRaycastObject;

    GameObject stick = null;
    void Start()
    {
        stick = Instantiate(_gameObject);
        stick.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //rotate camera
        if(Input.GetMouseButtonDown(1))
        {
            checkMouseDrag = true;
            mousePosition = Input.mousePosition;
            newMousePosition = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(1))
        {
            checkMouseDrag = false;
        }
        if(checkMouseDrag)
        {
            newMousePosition = Input.mousePosition;
            if(newMousePosition != mousePosition)
            {
                Vector3 vector3 = newMousePosition - mousePosition;
                Transform transform = mainCamera.transform;
                transform.Rotate(-vector3.y / 5, 0, 0f, Space.Self);
                transform.Rotate(0, vector3.x / 5, 0f, Space.World);
                mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, transform.rotation, 1000f);
                //mainCamera.transform.Rotate(-vector3.y/5 , 0, 0f, Space.Self);
                //mainCamera.transform.Rotate(0, vector3.x/5, 0f, Space.World);
                mousePosition = newMousePosition;
            }
        }

        //teleport camera
        //check raycast plane

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out vision))
        {
            if (vision.collider.tag == "floor")
            {
                newPossionRaycast = vision.point;
                if (newPossionRaycast != currentPositionRaycast)
                {
                    stick.transform.position = new Vector3(vision.point.x, vision.point.y + 0.2f, vision.point.z);
                    stick.SetActive(true);
                    currentPositionRaycast = newPossionRaycast;
                }        
            }

            //raycast object
            if (vision.collider.tag == "object")
            {
                curentRaycastObject = vision.collider.gameObject;
                curentRaycastObject.GetComponent<Outline>().OutlineWidth = 5f;
                stick.SetActive(false);
            }
            else
            {
                if(curentRaycastObject != null)
                {
                    curentRaycastObject.GetComponent<Outline>().OutlineWidth = 0f;
                }        
            }

        }
        else
        {
            stick.SetActive(false);
            if (curentRaycastObject != null)
            {
                curentRaycastObject.GetComponent<Outline>().OutlineWidth = 0f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(stick.activeSelf)
            {
                mainCamera.transform.position = new Vector3(currentPositionRaycast.x, mainCamera.transform.position.y, currentPositionRaycast.z);
            }
        }
    }
}
