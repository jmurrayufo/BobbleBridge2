using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour {
   public float shipEngineThrust;
   public float shipTurningThrust;
   public float dragFactor;
   
   private ParticleSystem shipsEngines;
   private Rigidbody2D myRidgidBody2D;
   
   // Use this for initialization
   void Start () 
   {
      shipsEngines = gameObject.transform.Find ("Engine Mount/Particle System").gameObject.GetComponent<ParticleSystem>();
      myRidgidBody2D = gameObject.GetComponent<Rigidbody2D>();
      myRidgidBody2D.drag = 0.001f;
   }
   
   
   void OnGUI()
   {
      GUILayout.BeginVertical();
      GUILayout.Label("Speed:" + myRidgidBody2D.velocity.magnitude);
      GUILayout.Label("Rotation:" + myRidgidBody2D.rotation);
      GUILayout.Label("Lin Drag:" + myRidgidBody2D.drag);
      GUILayout.EndHorizontal();
   }
   
   
   // Update is called once per frame
   void Update () 
   {
   
      // This in network mode, we will only send commands. For now, just return.
      if (Network.isClient)
      {
         // TODO: This controller should send network commands to the server when we are a client
         return;
      }
      
      myRidgidBody2D.drag = Mathf.Clamp(
         Mathf.Exp( myRidgidBody2D.velocity.magnitude/dragFactor )-1,
         0.001f,
         5.0f
         );
         
      if (Input.GetKey(KeyCode.W))
      {
         gameObject.rigidbody2D.AddRelativeForce( new Vector2(0f,shipEngineThrust) );
         if (shipsEngines.enableEmission==false)
         {
            //shipsEngines.Play();
            shipsEngines.enableEmission=true;
            Debug.Log("Start Paticle Effect");
         }
      }
            
      else if(Input.GetKey(KeyCode.S))
         gameObject.rigidbody2D.AddRelativeForce( new Vector2(0f,-shipEngineThrust) );
      
      else
      {
         if (shipsEngines.enableEmission==true)
         {
            shipsEngines.enableEmission=false;
            Debug.Log("Stop Paticle Effect");
         }
      }
      
      if (Input.GetKey (KeyCode.A))
         gameObject.rigidbody2D.AddTorque ( shipTurningThrust);
      
      if (Input.GetKey (KeyCode.D))
         gameObject.rigidbody2D.AddTorque (-shipTurningThrust);
   }
}
