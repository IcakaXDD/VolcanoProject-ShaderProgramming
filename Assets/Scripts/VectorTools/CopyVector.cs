using UnityEngine;

[RequireComponent(typeof(SetVector))]
public class CopyVector : MonoBehaviour
{
	public GetVector VectorSource;
	SetVector visualizer;

	private void Start() {
		visualizer=GetComponent<SetVector>();
	}

	void Update()
    {
		visualizer.vector=VectorSource.vector;
    }
}
