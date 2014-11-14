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
      if (Input.GetKeyDown(KeyCode.Space))
         Debug.Log(thrustController.GetCurrentOutput());
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
         
         //\todo we should also account for lateral velocity here, and not 'orbit thrust'. 
         myRidgid.AddRelativeForce( new Vector2(0, Mathf.Clamp(thrust, -10f,10f)));
         
         Vector3 myForward = transform.rotation * new Vector3(0, thrust, 0);
         Debug.DrawRay( transform.position, myForward, Color.red, 0, false );

      }
      
      // Attempt to cancel out lateral thrust
      // Calculate Difference in velocities
      Vector3 myVelDiff = rigidbody2D.velocity - target.rigidbody2D.velocity;
      // Project this velocity onto the right transform of this object. This gives us only the local space 'right/left' velocity
      // We also clamp this here to prevent insane AI manuvers
      myVelDiff = Vector3.ClampMagnitude(Vector3.Project(myVelDiff, transform.right),1f);
      // Apply this calculated force by projecting it into local space
      myRidgid.AddRelativeForce((Vector2) transform.InverseTransformDirection(-myVelDiff));
      // For debug help, draw a low showing what we are attempting to do
      Debug.DrawRay( transform.position, myVelDiff, Color.green, 0, false );
   }


}
