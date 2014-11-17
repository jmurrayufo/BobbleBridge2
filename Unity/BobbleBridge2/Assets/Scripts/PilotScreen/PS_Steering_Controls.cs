using UnityEngine;
using System.Collections;

public class PS_Steering_Controls : MonoBehaviour {


   private float thrustSliderSetting;
   private int thrustSign;
   static private float thrustMinLegalSetting;
   static private float thrustMaxLegalSetting;

   public GameObject playersShip;
   private ShipControl playerControlScript;

   private float headingSliderSetting;
   private bool headingAssistEnabled;


	// Use this for initialization
	void Start () {
      thrustSliderSetting = 0f;
      thrustMinLegalSetting = -.1f;
      thrustMaxLegalSetting = 1f;
      thrustSign = 0;
      playerControlScript = (ShipControl) playersShip.GetComponent("ShipControl");
      Debug.Log ("got:" + playerControlScript);
	}
	

	// Update is called once per frame
	void Update () {
	
	}
   
   
   void OnGUI()
   {
      Rect rightSideRect = new Rect(
         Screen.width*.9f,
         Screen.height*.2f,
         Screen.width*.1f,
         Screen.height*.6f
         );
      float thrustNewSliderSetting = thrustSliderSetting;
      float headingNewSliderSetting = headingSliderSetting;

      GUILayout.BeginArea(rightSideRect);
      thrustNewSliderSetting = GUILayout.VerticalSlider(
         thrustSliderSetting,
         thrustMaxLegalSetting,
         thrustMinLegalSetting,
         GUILayout.Height(100)
         );

      headingNewSliderSetting = GUILayout.HorizontalSlider(
         headingSliderSetting,
         -180f,
         180f,
         GUILayout.Height(10),
         GUILayout.Width(50)
         );

      GUILayout.EndArea();

      if (Input.GetKey(KeyCode.W))
      {
         thrustNewSliderSetting = Mathf.Clamp(
            thrustSliderSetting + .25f * Time.fixedDeltaTime, 
            thrustMinLegalSetting, 
            thrustMaxLegalSetting
            );
         if (thrustSign == 0)
            thrustSign = 1;
         else if (thrustSign == -1 && thrustNewSliderSetting > 0)
            thrustNewSliderSetting = 0;
      }

      if (Input.GetKeyUp (KeyCode.W) && thrustSign == -1)
         thrustSign = 0;

      if(Input.GetKey(KeyCode.S))
      {
         thrustNewSliderSetting = Mathf.Clamp(
            thrustSliderSetting - .25f * Time.fixedDeltaTime, 
            thrustMinLegalSetting, 
            thrustMaxLegalSetting
            );
         if (thrustSign == 0)
            thrustSign = -1;
         else if (thrustSign == 1 && thrustNewSliderSetting < 0)
            thrustNewSliderSetting = 0;
      }
   
   if (Input.GetKeyUp (KeyCode.S) && thrustSign == 1)
         thrustSign = 0;

      if(Input.GetKeyDown(KeyCode.Z))
         thrustNewSliderSetting = 0f;

      if ( headingNewSliderSetting != headingSliderSetting )
      {
         headingAssistEnabled = true;
      }
      else if(Input.GetKey(KeyCode.A))
      {
         headingNewSliderSetting -= 25f * Time.fixedDeltaTime;
         headingAssistEnabled = false;
      }
      else if(Input.GetKey(KeyCode.D))
      {
         headingNewSliderSetting += 25f * Time.fixedDeltaTime;
         headingAssistEnabled = false;
      } 


      if ( thrustNewSliderSetting != thrustSliderSetting )
      {
         thrustSliderSetting = thrustNewSliderSetting;
         playerControlScript.SetThrust(thrustSliderSetting);
      }


      if (headingNewSliderSetting != headingSliderSetting)
      {

         if (headingAssistEnabled)
         {
            
            // Wrap the heading value back to (-180,180) inclusive
            if ( headingNewSliderSetting > 180f )
               headingNewSliderSetting -= 360f;
            else if ( headingNewSliderSetting < -180f)
               headingNewSliderSetting += 360f;
            
            headingSliderSetting = headingNewSliderSetting;
            playerControlScript.SetHeading( headingSliderSetting );
         }
         else
         {
            playerControlScript.ActivateTurn( headingNewSliderSetting - headingSliderSetting );
         }

      }


      /*
      GUILayout.BeginVertical();
      GUILayout.Label("Speed:" + myRidgidBody2D.velocity.magnitude);
      GUILayout.Label("Rotation:" + myRidgidBody2D.rotation);
      GUILayout.Label("Lin Drag:" + myRidgidBody2D.drag);
      GUILayout.EndHorizontal();
      */
   }

}
