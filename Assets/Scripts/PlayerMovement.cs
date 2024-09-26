using Audio;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Actor
{
	public GameObject availableGun;

	public Transform gunPos;

	public List<GameObject> availableGuns;

	public static PlayerMovement Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		Instance = this;
		availableGuns = new List<GameObject>();
	}

	protected override void InputUpdate()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			MonoBehaviour.print("still goin");
			if (Game.Instance.paused)
			{
				Lobby.Instance.UnPause();
			}
			else
			{
				Lobby.Instance.Pause();
			}
		}
		if (Game.Instance.paused)
		{
			return;
		}
		Vector2 dir = (Vector2)Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - (Vector2)base.transform.position;
		float axis = UnityEngine.Input.GetAxis("Horizontal");
		SlowMo();
		animator.SetFloat("Speed", Mathf.Abs(axis));
		if (!animator.GetBool("Slide"))
		{
			if (axis > 0f)
			{
				Move(1);
			}
			else if (axis < 0f)
			{
				Move(-1);
			}
		}
		if (axis < 0.01f && axis > -0.01f)
		{
			CounterMovement();
		}
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			if (Mathf.Abs(rb.velocity.x) > 6f)
			{
				StartSlide();
			}
			else
			{
				Crouch();
			}
		}
		else if (UnityEngine.Input.GetKeyUp(KeyCode.S))
		{
			StopSlide();
		}
		if (Input.GetButtonDown("Jump"))
		{
			StartJump();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.E))
		{
			PickupGun();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Fire2"))
		{
			if (gun == null)
			{
				return;
			}
			DropGun(dir, 20f);
			AudioManager.Instance.Play("Throw");
		}
		if (Input.GetButton("Fire1"))
		{
			if (gun == null)
			{
				PickupGun();
			}
			else if (gunScript.ready)
			{
				Shoot(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - gun.position);
			}
		}
	}

	private void LateUpdate()
	{
		if (!Game.Instance.paused && !dead && !(arm == null) && !(gun == null))
		{
			Vector2 vector = (Vector2)Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - (Vector2)base.transform.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			arm.rotation = Quaternion.Euler(new Vector3(0f, 0f, num + 90f));
		}
	}

	private void SlowMo()
	{
		if (dead)
		{
			return;
		}
		if (UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.A))
		{
			f += 0.05f * Time.deltaTime * 250f;
			if (f > 1f)
			{
				f = 1f;
			}
			if (f > 0.345f && f < 0.355f)
			{
				AudioManager.Instance.Play("Fastmo");
			}
		}
		else
		{
			f -= 0.01f * Time.deltaTime * 250f;
			if ((double)f < 0.1)
			{
				f = 0.1f;
			}
			if (f > 0.345f && f < 0.355f)
			{
				AudioManager.Instance.Play("Slowmo");
			}
		}
		Time.timeScale = f;
	}

	protected override void DeathConditions()
	{
		AudioManager.Instance.Play("PlayerHit");
		if (damage > 0)
		{
			Kill();
		}
	}

	protected void PickupGun()
	{
		if (availableGuns.Count >= 1)
		{
			if (gun != null)
			{
				DropGun(Vector2.up, 5f);
			}
			GiveGun(availableGuns[0]);
			if (availableGuns[0] == null)
			{
				availableGuns.RemoveAt(0);
			}
			AudioManager.Instance.Play("PickupGun");
		}
	}

	protected override void ShootExtra()
	{
		f += 0.5f;
		if (f > 1f)
		{
			f = 1f;
		}
	}

	public float GetSpeed()
	{
		return f;
	}

	public Rigidbody2D GetRb()
	{
		if (dead)
		{
			return torsoRb;
		}
		return rb;
	}

	protected override void DeadExtra()
	{
		Time.timeScale = 0.3f;
		AudioManager.Instance.Play("Death");
		CameraShake.ShakeOnce(0.3f, 2.3f);
		Game.Instance.PlayerDied();
	}
}
