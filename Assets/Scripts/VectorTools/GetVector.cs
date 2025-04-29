using UnityEngine;
using System;

// Use this component to get the world space vector that matches a 3D vector (arrow) model shown in the scene.
// Attach this component to a game object that displays the arrow model.
public class GetVector : MonoBehaviour
{
	public static int DirtyFrames = 0;

	public Vector3 vector {
		get {
			// TransformVector transforms the vector to world space, taking all parent transforms into account.
			// Rotation & scale information is used, but position is ignored (since vectors have no position).
			// Since our vector 3D model points upwards, we start with the vector (0,1,0) (=Vector3.up)
			return transform.TransformVector(new Vector3(0, 1, 0));
		}
	}

	void Update() {
		if (transform.hasChanged) {
			DirtyFrames=3;
			transform.hasChanged=false;
		}
	}
}
