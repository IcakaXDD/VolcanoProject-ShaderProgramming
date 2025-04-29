using UnityEngine;
using TMPro;

// Use this component to visualize the vector projection of vector [Projectee] onto vector [Onto].
// Drag two game objects with a GetVector component into the inspector fields to make it work.
[RequireComponent(typeof(SetVector))]
public class Projection : MonoBehaviour
{
	public GetVector Projectee;
	public GetVector Onto;
	public TextMeshProUGUI text;
	SetVector visualizer;

	private void Start() {
		visualizer=GetComponent<SetVector>();
	}

	void Update()
    {
        if (visualizer!=null) {
			// vector projection:
			float dot = Vector3.Dot(Projectee.vector, Onto.vector);
			visualizer.vector = dot * Onto.vector;
			if (text!=null) {
				text.text = $"Dot product between\n{Projectee.vector}&\n{Onto.vector}:\n{dot:0.000}";
			}
		}
    }
}
