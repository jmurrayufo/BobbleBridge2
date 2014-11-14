using UnityEngine;
using System.Collections;

public class AIMovementController : MonoBehaviour {


   public GameObject target;
   public float followDistance;
   private Rigidbody2D myRidgid;
   private float myFaceError;
   private float distanceToTarget;
   private PID thrustController;
	

   // Use this for initialization
	void Start () {
      myRidgid = GetComponent<Rigidbody2D>();
      thrustController = new PID(1,0,20);
	}
	

	// Update is called once per frame
	void Update () {
   }

   // \TODO AI smarts are in the FixedUpdate function. They need to be pulled out to help support porting of network code.
   void FixedUpdate() 
   {
      Vector3 targetPos;
      Vector3 vectorToTarget;
      int sign;
      // TODO This could get moved to another function to make it cleaner
      targetPos = target.transform.position;
      vectorToTarget = targetPos - transform.position;
      sign = (Vector3.Cross( vectorToTarget, transform.up ).z < 0 ) ? -1 : 1;
      myFaceError = Vector3.Angle( vectorToTarget, transform.up ) * sign;

      if (target!=null)
      {
         distanceToTarget = (transform.position - target.transform.position).magnitude;
      }

      // \TODO we could slightly adjust heading to help cancel out lateral velocity as well. 
      // Apply Torque to fix heading
      myRidgid.AddTorque( -Mathf.Clamp( myFaceError, -0.1f, 0.1f) );

      // Don't thrust when we are not even facing the target
      if (Mathf.Abs (myFaceError) > 15)
      {
         // Not aiming correctly, do nothing!
      }

      // Handle thrust with a PID controller!
      else
      {
         float thrust = -thrustController.Update(followDistance, distanceToTarget, Time.deltaTime);

         myRidgid.AddRelativeForce( new Vector2(0, Mathf.Clamp(thrust, -10f,10f)));

      }
   }


}
