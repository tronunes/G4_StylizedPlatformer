using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterAnimation : MonoBehaviour
{
	public float amplitude = 0.2f;
	public float length = 1f;
	public float speed = 1f;

	private float offset = 0f;
	private MeshFilter meshFilter;


	private void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
	}

	private void Update()
	{
		offset += speed * Time.deltaTime;

		Vector3[] vertices = meshFilter.mesh.vertices;
		for(int i = 0; i < vertices.Length; ++i)
		{
			vertices[i].y = transform.position.y - GetWaveHeight(transform.position.x + vertices[i].x);
		}

		meshFilter.mesh.vertices = vertices;
		meshFilter.mesh.RecalculateNormals();
	}

	private float GetWaveHeight(float x)
	{
		return (this.transform.position.y - (amplitude * Mathf.Sin(x / length + offset)));
	}
}
