using UnityEngine;
using System.Collections;


public class PID {
   public float pVal, iVal, dVal;
   public float integral;
   public float lastError;
   private float currentOutput;
   

   // Constructor
   public PID(float pVal, float iVal, float dVal) {
      this.pVal = pVal;
      this.iVal = iVal;
      this.dVal = dVal;
   }
   

   // Step the controller and return the new current value
   public float Update(float setpoint, float actual, float timeStep) {
      float present = setpoint-actual;
      integral += present*timeStep;
      float deriv = (present-lastError)/timeStep;
      lastError = present;
      currentOutput = present*pVal + integral*iVal + deriv*dVal;
      return currentOutput;
   }


   // Get the current value without causing an update
   public float GetCurrentOutput()
   {
      return currentOutput;
   }


   // Reset the PID controller to a base state
   public void Reset( )
   {
      lastError = 0;
      integral = 0;
   }


}
