using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
	public bool alerted;

	private Transform target;

	private bool facingRight;

	public float triggerHappy;

	private bool ready = true;

	private float desiredAim;

	private float aimVel;

	private float lastAim;

	private Vector2 aimDir;

	private int spawnDir;

	public LayerMask whatIsPlayer;

	private void Awake()
	{
		spawnDir = (int)base.transform.localScale.x;
	}

	protected override void InitActor()
	{
		if (Game.Instance.started)
		{
			WinCondition.Instance.IncrementDesired();
		}
	}

	private void LateUpdate()
	{
		if (!Game.Instance.started || !alerted || arm == null || dead || gun == null)
		{
			return;
		}
		if (target == null)
		{
			target = PlayerMovement.Instance.head;
			return;
		}
		Vector2 vector = (Vector2)target.position + Vector2.down * 0.4f - (Vector2)base.transform.position;
		desiredAim = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		float num = Mathf.LerpAngle(lastAim, desiredAim, Time.time * 0.01f);
		arm.rotation = Quaternion.Euler(new Vector3(0f, 0f, num + 90f));
		lastAim = num;
		aimDir = vector;
		Vector3 position = target.position;
		Vector3 position2 = base.transform.position;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(gun.position + (Vector3)aimDir.normalized * 0.3f, gun.right * spawnDir, 10f, whatIsPlayer);
		if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.CompareTag("Player") && ready)
		{
			ready = false;
			Invoke("EnemyShoot", Random.Range(0.3f, 1.1f));
		}
		if (!facingRight)
		{
			if (target.position.x > base.transform.position.x)
			{
				base.transform.localScale = new Vector2(0f - defaultScale.x, defaultScale.y);
				facingRight = true;
			}
		}
		else if (target.position.x < base.transform.position.x)
		{
			MonoBehaviour.print("SWITCHING LEFT");
			base.transform.localScale = new Vector2(defaultScale.x, defaultScale.y);
			facingRight = false;
		}
	}

	private void EnemyShoot()
	{
		ready = true;
		if (!dead)
		{
			Shoot(aimDir);
		}
	}

	protected override void DeadExtra()
	{
		List<Collider2D> colliders = PlayerMovement.Instance.GetColliders();
		for (int i = 0; i < playerColliders.Count; i++)
		{
			for (int j = 0; j < colliders.Count; j++)
			{
				if (!(playerColliders[i] == null) && !(colliders[j] == null))
				{
					Physics2D.IgnoreCollision(playerColliders[i], colliders[j]);
				}
			}
		}
		WinCondition.Instance.AddEnemy();
		List<Collider2D> gunColliders = WinCondition.Instance.GetGunColliders();
		for (int k = 0; k < gunColliders.Count; k++)
		{
			IgnoreWithOneObject(gunColliders[k]);
		}
		if (PlayerMovement.Instance.transform != null)
		{
			DropGun((PlayerMovement.Instance.transform.position - base.transform.position) / 4f + Vector3.up * 1.5f, 8f);
		}
	}

	public void Alert()
	{
		alerted = true;
	}
}
