using Audio;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public bool pickedUp;

	public bool meleeWeapon;

	public GameObject bullet;

	public GameObject muzzle;

	public GameObject blood;

	public bool ready = true;

	public float fireRate = 0.2f;

	public float bulletSpeed;

	public int bullets;

	public float spread;

	private float gunLength;

	private void Start()
	{
		Collider2D component = GetComponent<Collider2D>();
		gunLength = component.bounds.size.x * 2f;
		if (Game.Instance.started)
		{
			PlayerMovement.Instance.IgnoreWithOneObject(component);
			WinCondition.Instance.AddGunCollider(component);
		}
	}

	public void Fire(List<Collider2D> c, Vector2 dir)
	{
		if (!ready || !pickedUp || meleeWeapon)
		{
			return;
		}
		ready = false;
		CameraShake.ShakeOnce(0.15f, 0.8f);
		float z = Mathf.Atan2(dir.y, dir.x) * 57.29578f;
		Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, z));
		List<Collider2D> list = new List<Collider2D>();
		for (int i = 0; i < bullets; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(bullet, base.transform.position + (Vector3)dir.normalized * gunLength, rotation);
			Collider2D component = gameObject.GetComponent<Collider2D>();
			for (int j = 0; j < c.Count; j++)
			{
				if (!(c[j] == null) && !(component == null))
				{
					Physics2D.IgnoreCollision(c[j], component, ignore: true);
				}
			}
			gameObject.layer = LayerMask.NameToLayer("Bullet");
			Rigidbody2D component2 = gameObject.GetComponent<Rigidbody2D>();
			Vector2 b = new Vector2(UnityEngine.Random.Range(0f - spread, spread), UnityEngine.Random.Range(0f - spread, spread) * 30f);
			component2.AddForce((dir.normalized * 120f + b) * bulletSpeed);
			Collider2D component3 = gameObject.GetComponent<Collider2D>();
			foreach (Collider2D item in list)
			{
				Physics2D.IgnoreCollision(item, component3);
			}
			list.Add(component3);
		}
		Object.Instantiate(muzzle, base.transform.position + (Vector3)dir.normalized * gunLength, Quaternion.identity);
		float x = CameraMovement.Instance.transform.position.x;
		float x2 = base.transform.position.x;
		float num = Mathf.Abs(x - x2);
		if (num < 0.1f)
		{
			num = 0.1f;
		}
		float num2 = 1.5f / num;
		if (num2 > 1.5f)
		{
			num2 = 1.5f;
		}
		if (num > 15f)
		{
			num2 = 0f;
		}
		AudioManager.Instance.Play("Gunshot", num2);
		Invoke("Reload", fireRate);
	}

	private void Reload()
	{
		ready = true;
	}

	public void Pickup()
	{
		ready = false;
		Invoke("Reload", fireRate);
	}

	public void Drop()
	{
		base.transform.GetChild(0).gameObject.SetActive(value: true);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (meleeWeapon)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
			{
				((Bullet)other.gameObject.GetComponent(typeof(Bullet))).DestroyBullet(null);
			}
			else if (other.gameObject.layer == LayerMask.NameToLayer("Player") && pickedUp)
			{
				if (other.gameObject.CompareTag("Player"))
				{
					return;
				}
				MonoBehaviour.print("KILLING");
				Actor actor = (Actor)other.transform.root.GetComponent(typeof(Actor));
				if (actor == null || actor.IsDead() || !actor.readyToDie)
				{
					return;
				}
				MonoBehaviour.print("KILLING2");
				actor.Kill();
				actor.GetTorsoRb().velocity = (other.transform.position - base.transform.position).normalized * 50f + Vector3.up * 1f;
				AudioManager.Instance.Play("GunHitPlayer", 1.5f);
				CameraShake.ShakeOnce(0.15f, 1f);
				Object.Instantiate(blood, other.transform.position, Quaternion.identity);
			}
			else if (other.gameObject.CompareTag("Barrel"))
			{
				((Barrel)other.gameObject.GetComponent(typeof(Barrel))).Explode();
				Rigidbody2D component = GetComponent<Rigidbody2D>();
				if (component == null)
				{
					return;
				}
				component.velocity = (PlayerMovement.Instance.transform.position - base.transform.position) * 1.5f + Vector3.up;
			}
		}
		if (other.gameObject.layer != LayerMask.NameToLayer("Player") || other.gameObject.CompareTag("Player"))
		{
			return;
		}
		Rigidbody2D component2 = GetComponent<Rigidbody2D>();
		if (!(component2 == null))
		{
			float magnitude = component2.velocity.magnitude;
			Actor actor2 = (Actor)other.transform.root.GetComponent(typeof(Actor));
			if (!(actor2 == null) && !actor2.IsDead() && actor2.readyToDie)
			{
				actor2.Kill();
				actor2.GetTorsoRb().velocity = component2.velocity * 2f + Vector2.up * 3f;
				AudioManager.Instance.Play("GunHitPlayer", 1.5f);
				CameraShake.ShakeOnce(0.15f, 1f);
				Object.Instantiate(blood, other.transform.position, Quaternion.identity);
				component2.velocity = (PlayerMovement.Instance.transform.position - base.transform.position) * 1.5f + Vector3.up;
			}
		}
	}

	public Collider2D GetCollider()
	{
		return GetComponent<Collider2D>();
	}
}
