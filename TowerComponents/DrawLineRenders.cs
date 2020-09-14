using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineRenders : MonoBehaviour
{
	public LineRenderer[] lineRenders;
	public bool draw;
	public float maxDistance = 10.0f;

	private Vector3[] positionsLines;
	private RaycastHit hit;

	private void Start()
	{
		if (lineRenders.Length == 0)
			return;

		positionsLines = new Vector3[lineRenders.Length];
		for (int i = 0; i < lineRenders.Length; i++)
			positionsLines[i] = lineRenders[i].transform.position;

		ResetLines();
	}

	void FixedUpdate()
	{
		if (lineRenders.Length == 0 || !draw)
			return;

		for (int i = 0; i < lineRenders.Length; i++)
		{
			//Debug.DrawRay(lineRenders[i].transform.position, transform.TransformDirection(Vector3.forward) * maxDistance);
			if (Physics.Raycast(lineRenders[i].transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance))
			{
				DrayLine(i, hit.point, hit.distance);
			}
			else {
				this.ResetLines();
			}
		}
	}

	private void DrayLine(int index, Vector3 target, float distance) {
		print(distance);
		lineRenders[index].SetPosition(1, -	transform.TransformDirection(target));
		//lineRenders[index].SetPosition(1, new Vector3(0, 0, distance));
	}

	private void ResetLines()
	{
		for (int i = 0; i < lineRenders.Length; i++)
		{
			lineRenders[i].SetPosition(0, Vector3.zero);
			lineRenders[i].SetPosition(1, Vector3.zero);
		}
	}
}
