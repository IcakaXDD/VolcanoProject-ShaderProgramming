using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use this component to display a given vector (direction) in the scene view, using the 3d arrow model.
// Make sure that this game object or a child game object displays the 3d arrow model, and that no other rotations and scales are applied
// in their transforms.
public class SetVector : MonoBehaviour
{
	public Vector3 vector;

    void LateUpdate()
    {
		// Unity's Quaternion.LookRotation associates the identity rotation (=no rotation) with the forward vector (=(0,0,1)).
		// Since our arrow model is pointing up, we need to combine that rotation with the rotation Euler(90,0,0) 
		// (which rotates (0,1,0) to (0,0,1)).
		// Quaternion multiplication is used to combine these rotations (order matters!)
		if (vector.magnitude>0) {
			transform.rotation = Quaternion.LookRotation(vector) * Quaternion.Euler(90, 0, 0);
		}
		transform.localScale = new Vector3(1, vector.magnitude, 1);
    }
}
