using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Extrude : MeshCreator {
	public float height = 1;
	public bool ModifySharedMesh = false;

	public override void RecalculateMesh() {
		Curve curve = GetComponent<Curve>();
		if (curve==null)
			return;
		List<Vector3> points = curve.points;
		if (points.Count < 2) {
			Debug.Log("Cannot triangulate polygons with less than 3 vertices");
			return;
		}
		// Copy the inspector array to a list that's going to be modified:
		List<Vector2> polygon = new List<Vector2>();
		for (int i = 0; i<points.Count; i++) {
			polygon.Add(new Vector2(points[i].x, points[i].y));
		}

		// Create a list of indices 0..n-1:
		List<int> indices = new List<int>();
		for (int i = 0; i < polygon.Count; i++) {
			indices.Add(i);
		}
		// This list is going to contain the vertex indices of the triangles: (3 integers per triangle)
		List<int> triangles = new List<int>();

		// Compute the triangulation of [polygon], store it in [triangles]:
		TriangulatePolygon(triangles, polygon, indices);

		MeshBuilder builder = new MeshBuilder();

		// Add front face:
		for (int i = 0; i < points.Count; i++) {
			// TODO: Add uvs
			builder.AddVertex(new Vector3(points[i].x, points[i].y, 0));
		}
		for (int t = 0; t < triangles.Count; t += 3) {
			builder.AddTriangle(triangles[t], triangles[t+1], triangles[t+2]);
			//Debug.Log ("Adding triangle " + triangles [t] + "," + triangles [t + 1] + "," + triangles [t + 2]);
		}
		// Add back face:
		int n = points.Count;
		for (int i = 0; i < points.Count; i++) {
			// TODO: Add uvs
			builder.AddVertex(new Vector3(points[i].x, points[i].y, height));
		}
		for (int t = 0; t < triangles.Count; t += 3) {
			builder.AddTriangle(n+triangles[t+2], n+triangles[t+1], n+triangles[t]);
		}

		// Add sides:
		for (int i = 0; i < points.Count; i++) {
			int j = (i + 1) % points.Count;
			// TODO: Add uvs
			// front vertices:
			int v1 = builder.AddVertex(new Vector3(points[i].x, points[i].y, 0));
			int v2 = builder.AddVertex(new Vector3(points[j].x, points[j].y, 0));
			// back vertices:
			int v3 = builder.AddVertex(new Vector3(points[i].x, points[i].y, height));
			int v4 = builder.AddVertex(new Vector3(points[j].x, points[j].y, height));
			// Add quad:
			builder.AddTriangle(v1, v3, v2);
			builder.AddTriangle(v2, v3, v4);
		}
		ReplaceMesh(builder.CreateMesh(), ModifySharedMesh);
	}


	private bool isClockwise(Vector2 u, Vector2 v,Vector2 w)
	{
		Vector2 diff1= v - u;
		Vector2 diff2= w - v;
		Vector3 d13D = new Vector3(diff1.x, diff1.y, 0);
		Vector3 d23D = new Vector3(diff2.x, diff2.y, 0);

		if(Vector3.Cross(d13D, d23D).z < 0)
		{
			return true;
		}

		return false;
	}

	// *IF* [polygon] respresents a simple polygon (no crossing edges), given in clockwise order, then 
	// this method will return in [triangles] a triangulation of the polygon, using the vertex indices from [indices]
	// If the assumption is not satisfied, the output is undefined.
	void TriangulatePolygon(List<int> triangles, List<Vector2> polygon, List<int> indices) {
		for (int i = 0; i < polygon.Count; i++) {
			int i2 = (i + 1) % polygon.Count;
			int i3 = (i + 2) % polygon.Count;
			Vector2 u = polygon[i];
			Vector2 v = polygon[i2];
			Vector2 w = polygon[i3];

			// TODO: Check whether the polygon corner at point v is less than 180 degrees - if not continue the for loop (with the next value for i)
			//   (Hint: To check whether a triangle is clockwise, you can use the dot or cross product)

			// TODO: Check whether there are no other points of the polygon inside the triangle u,v,w - if not continue the for loop (with the next value for i)
			//   (Hint: You can use three times a clockwise check - make a drawing to figure out the details!)

			// Assuming we have a correct "ear":
			// Add a triangle on u,v,w:
			triangles.Add(indices[i]);
			triangles.Add(indices[i2]);
			triangles.Add(indices[i3]);

			polygon.RemoveAt(i2); // remove v from point list (keep u and w!)
			indices.RemoveAt(i2); // also remove the corresponding index from the index list
			if (polygon.Count < 3)
				return; // The polygon is now fully triangulated

			// continue with a smaller polygon - restart the for loop:
			i=-1;
		}
		throw new Exception("No suitable triangulation found - is the polygon simple and clockwise?");
	}
}
