using UnityEngine;

// Use this component to visualize the cross product of two vectors.
// Drag two game objects with a GetVector component into the inspector fields to make it work.
[RequireComponent(typeof(SetVector))]
public class CrossProduct : MonoBehaviour
{
	public GetVector Vector1;
	public GetVector Vector2;
	SetVector visualizer;

	private void Start() {
		visualizer=GetComponent<SetVector>();
	}

	void Update()
    {
        if (visualizer!=null) {
			visualizer.vector=Vector3.Cross(Vector1.vector,Vector2.vector);
		}
    }
}
