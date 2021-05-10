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
    private float rayLeght = 20f;

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
            //mainCamera.transform.Rotate(mainCamera.transform.rotation.x, mainCamera.transform.rotation.y + 10, mainCamera.transform.rotation.z);
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
                mainCamera.transform.Rotate(-vector3.y/5 , 0, 0f, Space.Self);
                mainCamera.transform.Rotate(0, vector3.x/5, 0f, Space.World);
                mousePosition = newMousePosition;
            }
        }

        //teleport camera
        //check raycast

        var mask = LayerMask.GetMask("floor");
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out vision, mask))
        {
            if (vision.collider.tag == "floor")
            {
                newPossionRaycast = vision.point;
                if (newPossionRaycast != currentPositionRaycast)
                {
                    stick.transform.position = vision.point;
                    stick.SetActive(true);
                    currentPositionRaycast = newPossionRaycast;
                }        
            }
        }
        else
        {
            stick.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(stick.activeSelf)
            {
                mainCamera.transform.position = new Vector3(currentPositionRaycast.x, mainCamera.transform.position.y, currentPositionRaycast.z);
            }
        }

        /*
        if(Input.GetMouseButtonDown(0))
        {
            var mask = LayerMask.GetMask("floor");
            if ((Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out vision, mask)))
            {
                if(vision.collider.tag == "floor")
                {
                    Debug.Log("san nha: " + vision.point);
                    mainCamera.transform.position = new Vector3(vision.point.x, mainCamera.transform.position.y, vision.point.z);
                }
            }
        }
        */
    }
}
