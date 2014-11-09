using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour 
{
   // Braces go in the same column that will close them!

   // Lines use 3 SPACES as indent, NOT TABS.

   // This is an un-initlized public float. It can be seen in the editor, and will default to 0.0f.  
   public float foo;

   // This is a public int, but it is hidden in the Unity inspector by a [HideInInspector] tag. Each variable like this 
   //    would need to be tagged to be hidden. 
   [HideInInspector]
   public int bar;

   // This is not hidden like above.
   public int bar2;


   // Public functions should start with an uppercase!
   void Start ()
   {
      int localVar; //
   }


   // Two spaces before a functions
   // Update is called once per frame.
   /*
    * \brief This is the priefing
    * \detail This is a long detailed description,
    * it can even be a multi line.
    * \bug This would be a bug report
    */ 
   void Update () 
   {

   }
}
