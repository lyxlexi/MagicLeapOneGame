using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MeshConstructor : MonoBehaviour {

	public int segments;
	public float xRadius;
	public float yRadius;
	LineRenderer line;
	// Use this for initialization
	void Awake () {
		line = gameObject.GetComponent<LineRenderer>();
		line.positionCount = (segments);
		line.useWorldSpace = false;
		line.loop = false;
		CreatePoints();
		CreateMesh();
	}

	void CreateMesh(){
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		if (meshCollider != null) {
			Debug.Log("StartingMesh");	
			Mesh mesh = new Mesh();
			line.BakeMesh(mesh, false);
			meshCollider.sharedMesh = mesh;
			Debug.Log("Mesh");
		}
	}

	void CreatePoints()
	{
		float x = 0f;
		float y = 0f;
		float z = 0f;

		float angle = 20f;
		float delta = 360f/segments;

		for(int i = 0; i < segments; i++)
		{
			x = Mathf.Sin(Mathf.Deg2Rad * angle * xRadius);
			y = Mathf.Cos(Mathf.Deg2Rad * angle * yRadius);
			line.SetPosition(i, new Vector3(x,y,z));

			angle += delta;
		}
	}
}

