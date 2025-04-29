using UnityEngine;

// Use this component to visualize the difference between vectors Left and Right. (Left - Right)
// Drag two game objects with a GetVector component into the inspector fields to make it work.
[RequireComponent(typeof(SetVector))]
public class Difference : MonoBehaviour
{
	public GetVector Left;
	public GetVector Right;
	SetVector visualizer;

	private void Start() {
		visualizer=GetComponent<SetVector>();
	}

	void Update()
    {
        if (visualizer!=null) {
			visualizer.vector = Left.vector - Right.vector;
			visualizer.transform.localPosition = Right.vector;

		}
    }
}
