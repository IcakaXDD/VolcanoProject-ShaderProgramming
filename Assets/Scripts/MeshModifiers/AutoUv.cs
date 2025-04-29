using UnityEngine;

public class AutoUv : MonoBehaviour
{
	public Vector2 textureScaleFactor = new Vector2(1, 1);
	public bool UseWorldCoordinates;
	public bool AutoUpdate;
	public bool RecalculateTangents = true;

    void Update()
    {
        if ((transform.hasChanged && UseWorldCoordinates && AutoUpdate) || Input.GetKeyDown(KeyCode.F2)) {
			UpdateUvs();
			transform.hasChanged=false;
		}
    }

	public void UpdateUVs(Mesh mesh) {
		Debug.Log("Updating UVs");

		Vector2[] uv = mesh.uv;
		int[] tris = mesh.triangles;
		Vector3[] verts = mesh.vertices;
		for (int i = 0; i<tris.Length; i+=3) {
			int i1 = tris[i];
			int i2 = tris[i+1];
			int i3 = tris[i+2];
			Vector3 v1 = verts[i1];
			Vector3 v2 = verts[i2];
			Vector3 v3 = verts[i3];
			if (UseWorldCoordinates) {
				v1 = transform.TransformPoint(v1);
				v2 = transform.TransformPoint(v2);
				v3 = transform.TransformPoint(v3);
			}
			Vector3 tangent;
			Vector3 biTangent;

			ComputeTangents(v1, v2, v3, out tangent, out biTangent);
			ComputeTriangleUVs(v1, v2, v3, ref uv[i1], ref uv[i2], ref uv[i3], tangent, biTangent);
		}
		mesh.uv=uv;
		if (RecalculateTangents) {
			mesh.RecalculateTangents();
		}
	}

	public void UpdateUvs() {
		// Clone the shared mesh manually, to prevent the "leaking meshes" error:
		Mesh origMesh = GetComponent<MeshFilter>().sharedMesh; 
		Mesh mesh = (Mesh)Instantiate(origMesh);

		UpdateUVs(mesh);

		GetComponent<MeshFilter>().mesh=mesh;
	}

	void ComputeTangents(Vector3 v1, Vector3 v2, Vector3 v3, out Vector3 tangent, out Vector3 biTangent) {
		// TODO: Use *cross products* to compute:
		// - first, a normal vector for this triangle, and then
		// - a tangent and
		// - a biTangent
		// Hint: For each of these, find two vectors that should be perpendicular to it.

		// Replace this:
		tangent = Vector3.right;
		biTangent = Vector3.up;
	}

	void ComputeTriangleUVs(Vector3 v1, Vector3 v2, Vector3 v3, ref Vector2 uv1, ref Vector2 uv2, ref Vector2 uv3, Vector3 tangent, Vector3 biTangent) {
		// TODO: 
		//   Use *vector projection* to calculate the three UV vectors.
		//   (Hint: What do you need to project onto what? Look at example images)

		// Replace this:
		uv1 = new Vector2(v1.x, v1.y);
		uv2 = new Vector2(v2.x, v2.y);
		uv3 = new Vector2(v3.x, v3.y);
	}
}
