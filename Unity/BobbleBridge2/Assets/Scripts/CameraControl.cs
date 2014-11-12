using UnityEngine;
using System.Collections;


public class CameraControl : MonoBehaviour 
{

   public float mainCameraZoomSpeed = 500;
   public float minCameraZoom = 2;
   public float maxCameraZoom = 40;
    
    // Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
   void FixedUpdate () 
	{
      GameObject player = GameObject.FindWithTag("Player");
      Vector3 playerPostion = player.transform.position;

      // Set Position
      playerPostion.z = -10;

      transform.position = playerPostion;

	  zoomPilotCamera ();
	
	}

   void zoomPilotCamera ( )
   {
      // Initialize the camera zoom based on Mouse Wheel Activity
      float zoomAmount = Input.GetAxis ("Mouse ScrollWheel");
      
      // Scale the zoom Amount by the Zoom Speed
      zoomAmount *= mainCameraZoomSpeed;
      
      // Scale the Camera Zoom Speed on Time Passed
      zoomAmount *= Time.deltaTime;
         
      // Zoom the Camera in or out
      camera.orthographicSize -= zoomAmount;
      if (camera.orthographicSize < minCameraZoom) 
      {
         camera.orthographicSize = minCameraZoom;
      } else if (camera.orthographicSize > maxCameraZoom) 
      {
         camera.orthographicSize = maxCameraZoom;
      }
   }

}
