using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour {
   public float shipEngineThrust; //\todo Move this variable to a subsystem
   public float shipTurningThrust; //\todo Move this variable to a subsystem
   public float dragFactor; //\todo Move this variable to a subsystem

   private float headingControlSetting;
   private bool headingAssistControlEnabled;
   private float thrustControlSetting;
   private bool thrustZeroCrossingLocked; // Maybe should be GUI controlled?

   private ParticleSystem shipsEngines;
   private Rigidbody2D myRidgidBody2D;
   
   // Use this for initialization
   void Start () 
   {
      shipsEngines = gameObject.transform.Find ("Engine Mount/Particle System").gameObject.GetComponent<ParticleSystem>();
      myRidgidBody2D = gameObject.GetComponent<Rigidbody2D>();
      myRidgidBody2D.drag = 0.001f;

      headingControlSetting = 0f;
      headingAssistControlEnabled = true;
      thrustControlSetting = 0f;

   }

   
   // Update is called once per frame
   void FixedUpdate () 
   {
   
      // This in network mode, we will only send commands. For now, just return.
      if (Network.isClient)
      {
         // TODO: This controller should send network commands to the server when we are a client
         return;
      }

      // More go == more drag! 
      // \todo Update this later with extra user input!
      myRidgidBody2D.drag = Mathf.Clamp(
         Mathf.Exp( myRidgidBody2D.velocity.magnitude/dragFactor )-1,
         0.001f,
         5.0f
         );

      if (this.thrustControlSetting != 0f)
      {
         gameObject.rigidbody2D.AddRelativeForce( new Vector2(0f,shipEngineThrust * thrustControlSetting) );
         if (shipsEngines.enableEmission==false)
         {
            //shipsEngines.Play();
            shipsEngines.enableEmission=true;
         }
      }      
      else
      {
         if (shipsEngines.enableEmission==true)
         {
            shipsEngines.enableEmission=false;
         }
      }

      if (headingAssistControlEnabled)
      {  
         float shipHeading = 360f - gameObject.transform.rotation.eulerAngles.z;
         float headingError;

         if (shipHeading > 180f)
            shipHeading -= 360f;

         headingError = shipHeading - headingControlSetting;

         // Bound the heading error to the (-180,180) range
         if (headingError > 180)
            headingError -= 360;
         else if(headingError < -180)
            headingError += 360;

         if ( Mathf.Abs(headingError) > 1f )
         {
            gameObject.rigidbody2D.AddTorque( headingError > 0 ? shipTurningThrust : -shipTurningThrust );
         }
      }
      else
      {
         // Burn off built up turn amounts!
         if (this.headingControlSetting < 0)
         {
            gameObject.rigidbody2D.AddTorque(shipTurningThrust);
         }
         else if(this.headingControlSetting > 0)
         {
            gameObject.rigidbody2D.AddTorque(-shipTurningThrust);
         }
         this.headingControlSetting = 0f;
      }



      if (Input.GetKey (KeyCode.Escape))
         Application.Quit();

   }

   public void SetThrust(float newThrustSetting)
   {
      this.thrustControlSetting = newThrustSetting;
   }

   public void SetHeading(float newHeadingSetting)
   {
      this.headingAssistControlEnabled = true;
      this.headingControlSetting = newHeadingSetting;
   }

   public void ActivateTurn(float newTurning)
   {
      this.headingAssistControlEnabled = false;
      this.headingControlSetting += newTurning;
   }
}
