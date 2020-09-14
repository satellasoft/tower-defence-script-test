using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParticle : MonoBehaviour
{
	public ParticleSystem[] particles;

	public void FireParticle()
	{
		if (particles.Length == 0)
			return;

		for (int i = 0; i < particles.Length; i++)
			particles[i].Play();
	}

	public void StopParticle()
	{
		if (particles.Length == 0)
			return;

		for (int i = 0; i < particles.Length; i++)
			particles[i].Stop();
	}
}