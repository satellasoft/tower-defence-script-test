using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TowerParticle))]
public class TowerController : MonoBehaviour
{
	#region "Variables"
	[Header("Components")]
	public Tower tower;
	public Transform rayPoint;
	public AmmoBarUI ammoBarUI;
	public TowerRadius towerRadius;

	[Header("Controls")]
	public float timeSearch = 1.5f;
	public bool activeTurrent = true;
	public bool instantiateBullet = true;
	public string enemyTag = "Truck";

	private AudioSource audioSource;
	private Animator animator;
	private SpawnerController spawnerController;
	private GameObject currentEnemy;
	private TowerParticle towerParticle;

	private RaycastHit hit;
	private float time = 0;
	private bool isReloading = false;
	private float currentAmmo;

	//Variaveis que poderão ser alteradas em caso de um update nas armas
	[HideInInspector] public float totalBullets;
	[HideInInspector] public float fillPrice;
	#endregion

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = tower.shotSound;
		animator = GetComponent<Animator>();

		this.spawnerController = GameObject.FindGameObjectWithTag("SpawnerController").GetComponent<SpawnerController>();
		this.towerParticle = GetComponent<TowerParticle>();

		InvokeRepeating("LookToEnemy", 2, this.timeSearch);
		this.time = tower.reloadTime;

		if (this.towerRadius)
		{
			this.towerRadius.radius = tower.maxDistance;
		}
		this.totalBullets = tower.totalBullets;
		this.fillPrice = tower.fillAmmo;
		this.currentAmmo = tower.totalBullets;
		
	}

	private void FixedUpdate()
	{
		if (!currentEnemy || !activeTurrent || isReloading || currentAmmo == 0)
			return;

		this.transform.LookAt(currentEnemy.transform.position, Vector3.up);
		if (time >= tower.reloadTime && !isReloading)
		{
			this.Fire();
			time = 0;
		}
		time += 1 * Time.deltaTime;
	}

	private void Fire()
	{
		if (Physics.Raycast(rayPoint.position, transform.TransformDirection(Vector3.forward), out hit, this.tower.maxDistance))
		{
			if (hit.transform.CompareTag(enemyTag))
			{
				hit.transform.GetComponent<EnemiesController>().SetDamage(Random.Range(tower.attack[0], tower.attack[1]));
				this.towerParticle.FireParticle();
			}
		}

		currentAmmo--;
		isReloading = true;
		animator.SetTrigger("Fire");
		audioSource.Play();
		InstantiateBullet();
		StartCoroutine(Reloading());
		ChangeUIAmmoBar();
	}

	private void ChangeUIAmmoBar()
	{
		ammoBarUI.ChangeBar((currentAmmo / tower.totalBullets));
	}

	private void InstantiateBullet()
	{
		if (!instantiateBullet)
			return;

		Instantiate(tower.prefabBullet, rayPoint.position, rayPoint.rotation);
	}

	private IEnumerator Reloading()
	{
		isReloading = false;
		yield return new WaitForSeconds(tower.reloadTime);
	}

	private void LookToEnemy()
	{
		this.currentEnemy = this.GetCloser();
	}

	private GameObject GetCloser()
	{
		GameObject tempEnemy = null;
		float temptDistance = 999;
		List<GameObject> listEnemies = spawnerController.GetAllActiveEnemies();

		if (listEnemies.Count == 0)
		{
			return tempEnemy;
		}

		foreach (GameObject g in listEnemies)
		{
			if (g != null)
			{
				float d = Vector3.Distance(this.transform.position, g.transform.position);
				if (d < temptDistance && d <= tower.maxDistance)
				{
					temptDistance = d;
					tempEnemy = g;
				}
			}
		}
		return tempEnemy;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, tower.maxDistance);
	}

	public void Reload()
	{
		if (this.currentAmmo == this.totalBullets)
		{
			return;
		}
		this.currentAmmo = this.totalBullets;
		this.ChangeUIAmmoBar();
	}

	public void EnableRadius(bool enable)
	{
		towerRadius.enabled = enable;
	}
}
