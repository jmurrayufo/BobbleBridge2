using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour {
   public float shipEngineThrust;
   public float shipTurningThrust;
   
   private ParticleSystem shipsEngines;
   
   // Use this for initialization
   void Start () {
      shipsEngines = gameObject.transform.Find ("Engine Mount/Particle System").gameObject.GetComponent<ParticleSystem>();
   }
   
   // Update is called once per frame
   void Update () {
      if( Input.GetKey(KeyCode.W) )
      {
         gameObject.rigidbody2D.AddRelativeForce( new Vector2(0f,shipEngineThrust) );
         if (shipsEngines.isStopped)
         {
            shipsEngines.Play();
            Debug.Log("Start Paticle Effect");
         }
      }      
      else if( Input.GetKey(KeyCode.S) )
         gameObject.rigidbody2D.AddRelativeForce( new Vector2(0f,-shipEngineThrust) );
      
      else
      {
         if (!shipsEngines.isStopped)
         {
            shipsEngines.Stop();
            Debug.Log("Stop Paticle Effect");
         }
      }
      
      if (Input.GetKey (KeyCode.A))
         gameObject.rigidbody2D.AddTorque ( shipTurningThrust);
      
      if (Input.GetKey (KeyCode.D))
         gameObject.rigidbody2D.AddTorque (-shipTurningThrust);
   }
}
