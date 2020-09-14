using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class TowerRadius : MonoBehaviour
{
	[Range(0.1f, 100f)]
	public float radius = 1.0f;

	[Range(0.1f, 100f)]
	public float radiusOffset = 8.0f;

	[Range(3, 256)]
	public int numSegments = 128;

	[Range(0.1f, 10.0f)]
	public float width = 0.5f;

	public Material lineMaterial;

	LineRenderer lineRenderer;

	void Start()
	{
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.material = lineMaterial;
		lineRenderer.useWorldSpace = false;
	}

	public void ResetDraw()
	{
		this.lineRenderer.enabled = false;
	}

	public void Draw()
	{
		this.lineRenderer.enabled = true;
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
		lineRenderer.positionCount = numSegments + 1;
		float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;

		float theta = 0f;

		for (int i = 0; i < numSegments + 1; i++)
		{
			float x = (radius * 2) * Mathf.Cos(theta);
			float z = (radius * 2) * Mathf.Sin(theta);
			Vector3 pos = new Vector3(x, 0, z);
			lineRenderer.SetPosition(i, pos);
			theta += deltaTheta;
		}
	}
}
