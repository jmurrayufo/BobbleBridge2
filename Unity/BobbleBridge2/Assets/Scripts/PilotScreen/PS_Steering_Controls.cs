using UnityEngine;
using System.Collections;

public class PS_Steering_Controls : MonoBehaviour {

   //! \brief 
   public Texture compassTexture;

   //! \brief Store the actaul numeric setting of the slider.
   private float thrustSliderSetting;

   //! \brief Store the +/- sign of the slider to detect zero crossing, and lock controls when moving past zero.
   private int thrustSign;

   //! \brief Store Minimum legal setting of throttle.
   static private float thrustMinLegalSetting;

   //! \brief Store maximum legal setting of throttle.
   static private float thrustMaxLegalSetting;


   //! \brief Object to be controlled. Should have a "ShipControl" object on it!
   public GameObject playersShip;

   //! \brief Shortcut to the ShipControl object, prevents calls to GetComponent.
   private ShipControl playerControlScript;

   //! \brief Store actural numeric value of heading indicator.
   private float headingCompassSetting;

   //! \brief Indicate if heading assist control is currently in use. 
   private bool headingAssistEnabled;


	//! Use this for initialization
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
   
   //! \TODO This section needs to be reworked into a custom call at some point. This will be owned by the global GUI object.
   void OnGUI()
   {
      // Build a Rect to save the direction controls in.
      //! \todo This variable could be saved to prevent constatnt Rect allocations!
      Rect rightSideRect = new Rect(
         Screen.width*.9f,
         Screen.height*.2f,
         Screen.width*.1f,
         Screen.height*.6f
         );
      // Save defaults for "no change". 
      float thrustNewSliderSetting = thrustSliderSetting;
      float headingNewCompassSetting = headingCompassSetting;

      // Start GUILayout area.
      GUILayout.BeginArea(rightSideRect);

      // Draw vertical slider for ships thrust, and force it to be 100 px tall.
      thrustNewSliderSetting = GUILayout.VerticalSlider(
         thrustSliderSetting,
         thrustMaxLegalSetting,
         thrustMinLegalSetting,
         GUILayout.Height(100)
         );

      // Draw Compass
      GUILayout.Box(compassTexture, GUILayout.Width(50), GUILayout.Height(50));
      if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
      {
         Vector2 error = Event.current.mousePosition - GUILayoutUtility.GetLastRect().center;
         //Debug.Log ( Mathf.Atan2(error.x, -error.y));
         headingNewCompassSetting = Mathf.Atan2(error.x, -error.y) * Mathf.Rad2Deg;

      }


      // End GUILayout area. We are done drawing elements. 
      GUILayout.EndArea();

      // We now handle Keyboard input. 

      // W will accelerate the ship forward. This may result in forward thrust, or reduction of negative thrust.
      if (Input.GetKey(KeyCode.W))
      {
         // Adjust the throttle 'up'. 
         thrustNewSliderSetting = Mathf.Clamp(
            thrustSliderSetting + .25f * Time.fixedDeltaTime, 
            thrustMinLegalSetting, 
            thrustMaxLegalSetting
            );

         // If we are already holding at zero, we can be positive now.
         if (thrustSign == 0)
            thrustSign = 1;
         // If we are in the negative range, we CANNOT exceed zero. This provides a 'locking' behavior at zero.
         else if (thrustSign == -1 && thrustNewSliderSetting > 0)
            thrustNewSliderSetting = 0;
      }

      // Detect if we should allow crossing zero the next time forward is pressed.
      if (Input.GetKeyUp (KeyCode.W) && thrustSign == -1 && thrustNewSliderSetting == 0)
         thrustSign = 0;
   
      // As above, but for decreasing thrust.
      if(Input.GetKey(KeyCode.S))
      {
         // Adjust the throttle 'down'.
         thrustNewSliderSetting = Mathf.Clamp(
            thrustSliderSetting - .25f * Time.fixedDeltaTime, 
            thrustMinLegalSetting, 
            thrustMaxLegalSetting
            );
         
         // If we are already holding at zero, we can be positive now.
         if (thrustSign == 0)
            thrustSign = -1;
         // If we are in the negative range, we CANNOT exceed zero. This provides a 'locking' behavior at zero.
         else if (thrustSign == 1 && thrustNewSliderSetting < 0)
            thrustNewSliderSetting = 0;
      }
   
      // Detect if we should allow crossing zero the next time reverse is pressed.
      if (Input.GetKeyUp (KeyCode.S) && thrustSign == 1 && thrustNewSliderSetting == 0)
         thrustSign = 0;

      // Zero thrust
      if(Input.GetKeyDown(KeyCode.Z))
      {
         thrustNewSliderSetting = 0f;
         thrustSign = 0;
      }

      // We now handle changes to heading.

      // If the Mouse inputs changed the heading, we are using the Heading 
      //    Assist controls. Set them here.
      if ( headingNewCompassSetting != headingCompassSetting )
      {
         headingAssistEnabled = true;
      }
      // If the GUI didn't change settings, we need to check for left and right 
      //    inputs. Both will disable the heading assit control.
      else if(Input.GetKey(KeyCode.A))
      {
         headingNewCompassSetting -= 25f * Time.fixedDeltaTime;
         headingAssistEnabled = false;
      }
      else if(Input.GetKey(KeyCode.D))
      {
         headingNewCompassSetting += 25f * Time.fixedDeltaTime;
         headingAssistEnabled = false;
      } 

      // If we had a new throttle setting, we save it and tell the ship the input.
      if ( thrustNewSliderSetting != thrustSliderSetting )
      {
         thrustSliderSetting = thrustNewSliderSetting;
         //! \TODO change the ship thrust controls to understand netowrking here.
         playerControlScript.SetThrust(thrustSliderSetting);
      }

      // If we changed the heading OR turned, we will save it and tell the ship here. 
      if (headingNewCompassSetting != headingCompassSetting)
      {
         // If heading assist is enabled, we tell the ship to hold a heading.
         if (headingAssistEnabled)
         {
            // Wrap the heading value back to (-180,180) inclusive
            if ( headingNewCompassSetting > 180f )
               headingNewCompassSetting -= 360f;
            else if ( headingNewCompassSetting < -180f)
               headingNewCompassSetting += 360f;
            
            headingCompassSetting = headingNewCompassSetting;
            playerControlScript.SetHeading( headingCompassSetting );
         }
         // If we arn't in heading assist mode, we need to tell the player to turn by a given amount.
         else
         {
            //! \TODO Should the ship accept different turn amounts?
            playerControlScript.ActivateTurn( headingNewCompassSetting - headingCompassSetting );
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
