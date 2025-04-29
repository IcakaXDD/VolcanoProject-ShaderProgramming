using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handout {
	public class CreateStairs : MonoBehaviour {
		public int numberOfSteps = 10;
		// The dimensions of a single step of the staircase:
		public float width=3;
		public float height=1;
		public float depth=1;

		MeshBuilder builder;

		void Start () {
			builder = new MeshBuilder ();
			CreateShape ();
			GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
		}

		/// <summary>
		/// Creates a stairway shape in [builder].
		/// </summary>
		void CreateShape() {
			builder.Clear ();

			/**
			// V1: single step, hard coded:
			// bottom:
			int v1 = builder.AddVertex (new Vector3 (2, 0, 0), new Vector2 (1, 0));	
			int v2 = builder.AddVertex (new Vector3 (-2, 0, 0), new Vector2 (0, 0));
			// top front:
			int v3 = builder.AddVertex (new Vector3 (2, 1, 0), new Vector2 (1, 0.5f));	
			int v4 = builder.AddVertex (new Vector3 (-2, 1, 0), new Vector2 (0, 0.5f));
			// top back:
			int v5 = builder.AddVertex (new Vector3 (2, 1, 1), new Vector2 (0, 1));	
			int v6 = builder.AddVertex (new Vector3 (-2, 1, 1), new Vector2 (1, 1));

			builder.AddTriangle (v1, v2, v3);
			builder.AddTriangle (v2, v3, v4);
			builder.AddTriangle (v3, v4, v5);
			builder.AddTriangle (v4, v6, v5);
			/
			/**/
			// V2, with for loop:
			for (int i = 0; i < numberOfSteps; i++) {
				Vector3 offset = new Vector3 (0, height * i, depth * i);

                // Front Face Vertices
                int v1 = builder.AddVertex(offset + new Vector3(-width, 0, 0), new Vector2(0, 0));
                int v2 = builder.AddVertex(offset + new Vector3(width, 0, 0), new Vector2(1, 0));
                int v3 = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 1));
                int v4 = builder.AddVertex(offset + new Vector3(-width, height, 0), new Vector2(0, 1));

                // Top Face Vertices 
                int v5 = builder.AddVertex(offset + new Vector3(-width, height, 0), new Vector2(0, 0));
                int v6 = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 0));
                int v7 = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1, 1));
                int v8 = builder.AddVertex(offset + new Vector3(-width, height, depth), new Vector2(0, 1));

                // Right Side Vertices
                int v9 = builder.AddVertex(offset + new Vector3(width, 0, 0), new Vector2(0, 0));
                int v10 = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(0, 1));
                int v11 = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1, 1));
                int v12 = builder.AddVertex(offset + new Vector3(width, 0, depth), new Vector2(1, 0));

                // Left Side Vertices
                int v13 = builder.AddVertex(offset + new Vector3(-width, 0, 0), new Vector2(1, 0));
                int v14 = builder.AddVertex(offset + new Vector3(-width, height, 0), new Vector2(1, 1));
                int v15 = builder.AddVertex(offset + new Vector3(-width, height, depth), new Vector2(0, 1));
                int v16 = builder.AddVertex(offset + new Vector3(-width, 0, depth), new Vector2(0, 0));

				//Back Face Verticies 
				int v17 = builder.AddVertex(offset+new Vector3(width,height, depth), new Vector2(1, 1));
                int v18 = builder.AddVertex(offset + new Vector3(-width, height, depth), new Vector2(0, 1));
				int v19 = builder.AddVertex(offset+ new Vector3(-width, 0, depth), new Vector2(0, 0));
				int v20 = builder.AddVertex(offset + new Vector3(width, 0, depth), new Vector2(1, 0));

				//Bottom Face Verticies
				//v1
				int v21 = builder.AddVertex(offset + new Vector3(-width, 0, 0), new Vector2(0, 0));
				//v2
				int v22 = builder.AddVertex(offset+ new Vector3(width,0,0), new Vector2(1, 0));
				//v16
				int v23 = builder.AddVertex(offset+ new Vector3(-width,0,depth), new Vector2(0,1));
				//v12
				int v24 = builder.AddVertex(offset+ new Vector3(width,0,depth), new Vector2(1,1));

                // Front Face (clockwise winding for outward normals)
                builder.AddTriangle(v1, v3, v2);
                builder.AddTriangle(v1, v4, v3);

                // Top Face
                builder.AddTriangle(v5, v7, v6);
                builder.AddTriangle(v5, v8, v7);

                // Right Side
                builder.AddTriangle(v9, v10, v11);
                builder.AddTriangle(v9, v11, v12);

                // Left Side
                builder.AddTriangle(v13, v15, v14);
                builder.AddTriangle(v13, v16, v15);

				//Back Side
				builder.AddTriangle(v19, v20, v18);
				builder.AddTriangle(v18,v20,v17);

				//Bottom Side
				builder.AddTriangle(v21,v22,v23);
				builder.AddTriangle(v22, v24, v23);

                // TODO 5: Fix the normals by *not* reusing a single vertex in multiple triangles with different normals (solve it by creating more vertices at the same position)
            }
			/**/
		}
		
	}
}