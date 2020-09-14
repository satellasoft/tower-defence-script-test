using UnityEngine;
using UnityEngine.UI;

public class AmmoBarUI : MonoBehaviour
{
	public Image ammoBarImage;

	private void Start()
	{
		if (!ammoBarImage)
		{
			Debug.LogError("Ammo bar not defined");
			return;
		}
		this.FillBar();
	}

	public void ChangeBar(float value)
	{
		ammoBarImage.fillAmount = value;
	}

	public void FillBar()
	{
		ammoBarImage.fillAmount = 1;
	}
}
