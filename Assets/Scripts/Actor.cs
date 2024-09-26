using Audio;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public Rigidbody2D rb;

	protected SpriteRenderer sr;

	public Animator animator;

	protected float speed = 1500f;

	protected float maxSpeed = 12f;

	protected bool jumping;

	protected bool dead;

	protected float f = 1f;

	public PhysicsMaterial2D mat;

	protected Vector2 defaultScale;

	protected Vector2 flipScale;

	public GameObject slideFx;

	public Transform arm;

	public Transform armb;

	public Transform torso;

	public Transform head;

	protected Rigidbody2D torsoRb;

	protected Transform gun;

	protected Gun gunScript;

	protected List<Collider2D> playerColliders;

	public GameObject startGun;

	private bool readyToSlide;

	protected int damage;

	public Transform gunpos;

	public bool readyToDie;

	private void Start()
	{
		defaultScale = base.transform.localScale;
		flipScale = new Vector2(defaultScale.x * -1f, 1f);
		IgnoreColiisionWithSelf();
		if (startGun != null)
		{
			GiveGun(UnityEngine.Object.Instantiate(startGun, base.transform.position, Quaternion.identity));
		}
		InitActor();
		Invoke("GetReady", Time.fixedDeltaTime * 4f);
		readyToSlide = true;
	}

	protected virtual void InitActor()
	{
	}

	private void IgnoreColiisionWithSelf()
	{
		playerColliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
		for (int i = 0; i < playerColliders.Count; i++)
		{
			for (int j = i; j < playerColliders.Count; j++)
			{
				Physics2D.IgnoreCollision(playerColliders[i], playerColliders[j], ignore: true);
			}
		}
	}

	public void IgnoreWithOneObject(Collider2D o)
	{
		for (int i = 0; i < playerColliders.Count; i++)
		{
			if (!(playerColliders[i] == null) && !(playerColliders[i] == null) && !(o == null))
			{
				Physics2D.IgnoreCollision(playerColliders[i], o);
			}
		}
	}

	public void Kill()
	{
		if (dead || !readyToDie)
		{
			return;
		}
		animator.enabled = false;
		torso.parent = null;
		dead = true;
		HingeJoint2D hingeJoint2D = torso.gameObject.AddComponent<HingeJoint2D>();
		torsoRb = torso.GetComponent<Rigidbody2D>();
		torsoRb.interpolation = RigidbodyInterpolation2D.Interpolate;
		hingeJoint2D.connectedBody = torsoRb;
		for (int i = 0; i < torso.childCount; i++)
		{
			torso.GetChild(i).gameObject.AddComponent<HingeJoint2D>().connectedBody = torsoRb;
			if (i == 0)
			{
				HingeJoint2D component = torso.GetChild(i).gameObject.GetComponent<HingeJoint2D>();
				component.useLimits = true;
				JointAngleLimits2D limits = component.limits;
				limits.min = -70f;
				limits.max = 70f;
				component.limits = limits;
			}
			Rigidbody2D component2 = torso.GetChild(i).gameObject.GetComponent<Rigidbody2D>();
			component2.velocity = rb.velocity;
			component2.angularVelocity = rb.angularVelocity;
			component2.interpolation = RigidbodyInterpolation2D.Interpolate;
			component2.sharedMaterial = mat;
			if (torso.GetChild(i).childCount > 0)
			{
				torso.GetChild(i).GetChild(0).gameObject.AddComponent<HingeJoint2D>().connectedBody = torso.GetChild(i).GetComponent<Rigidbody2D>();
				torso.GetChild(i).GetChild(0).gameObject.GetComponent<Rigidbody2D>().velocity = rb.velocity;
			}
		}
		rb = torsoRb;
		DeadExtra();
	}

	protected void Crouch()
	{
	}

	protected void CounterMovement()
	{
		if (animator.GetBool("Landed") && !animator.GetBool("Slide"))
		{
			rb.velocity = new Vector2(rb.velocity.x / 2f, rb.velocity.y);
			if ((double)Mathf.Abs(rb.velocity.x) < 0.1)
			{
				rb.velocity = new Vector2(0f, rb.velocity.y);
			}
		}
	}

	private void GetReady()
	{
		readyToDie = true;
	}

	protected void StartSlide()
	{
		if (animator.GetBool("Landed") && !animator.GetBool("Slide") && readyToSlide)
		{
			animator.SetBool("Slide", value: true);
			float num = 0f;
			num = ((!(rb.velocity.x > 0f)) ? (-1f) : 1f);
			rb.AddForce(Vector2.right * 350f * num);
			if (rb.velocity.x < 9f)
			{
				rb.AddForce(Vector2.right * 150f * num);
			}
			slideFx.SetActive(value: true);
			AudioManager.Instance.Play("Slide");
			readyToSlide = false;
			Invoke("GetReadyToSlide", 0.3f);
		}
	}

	private void GetReadyToSlide()
	{
		readyToSlide = true;
	}

	protected void StopSlide()
	{
		animator.SetBool("Slide", value: false);
		if (slideFx != null)
		{
			slideFx.SetActive(value: false);
		}
	}

	protected void StartJump()
	{
		if (!jumping && animator.GetBool("Landed"))
		{
			jumping = true;
			animator.SetBool("Jumping", value: true);
			Invoke("StopJump", Time.fixedDeltaTime * 4f);
			Invoke("ResetJump", Time.fixedDeltaTime * 20f);
		}
	}

	protected void Move(int dir)
	{
		if ((rb.velocity.x < 0f - maxSpeed && dir < 0) || (rb.velocity.x > maxSpeed && dir > 0))
		{
			return;
		}
		base.transform.localScale = new Vector2(defaultScale.x * (float)dir, defaultScale.y);
		float d = 1f;
		if (dir < 0)
		{
			if (rb.velocity.x > 0.5f)
			{
				d = 2f;
			}
		}
		else if (rb.velocity.x < -0.5f)
		{
			d = 2f;
		}
		rb.AddForce(Vector2.right * speed * d * dir * Time.deltaTime * 3.5f);
		if (rb.velocity.x < 0f - maxSpeed)
		{
			rb.velocity = new Vector2(0f - maxSpeed, rb.velocity.y);
		}
		if (rb.velocity.x > maxSpeed)
		{
			rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
		}
	}

	protected virtual void DeadExtra()
	{
	}

	private void Update()
	{
		if (!dead)
		{
			InputUpdate();
		}
	}

	protected void GiveGun(GameObject g)
	{
		if (!(g == null))
		{
			gunScript = (Gun)g.GetComponent(typeof(Gun));
			if (!(gunScript == null) && !gunScript.pickedUp)
			{
				g.transform.position = gunpos.position;
				g.transform.rotation = gunpos.rotation;
				g.transform.parent = gunpos.transform.parent;
				g.transform.localScale = new Vector3(1f, 1f, 1f);
				gun = g.transform;
				UnityEngine.Object.Destroy(gun.GetComponent<Rigidbody2D>());
				gunScript.pickedUp = true;
				gunScript.Pickup();
			}
		}
	}

	protected void DropGun(Vector2 dir, float pow)
	{
		if (!(gun == null))
		{
			gun.transform.parent = null;
			gunScript.pickedUp = false;
			Rigidbody2D rigidbody2D = gun.gameObject.AddComponent<Rigidbody2D>();
			rigidbody2D.velocity = dir.normalized * pow;
			rigidbody2D.angularVelocity = -300f * (pow / 2f);
			rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
			gun = null;
			gunScript = null;
		}
	}

	protected void Shoot(Vector2 dir)
	{
		if (!(gunScript == null))
		{
			gunScript.Fire(playerColliders, dir);
			ShootExtra();
		}
	}

	protected virtual void ShootExtra()
	{
	}

	protected virtual void InputUpdate()
	{
	}

	protected void StopJump()
	{
		animator.SetBool("Jumping", value: true);
		animator.SetBool("Landed", value: false);
		rb.velocity = new Vector2(rb.velocity.x, 0f);
		rb.AddForce(Vector2.up * 450f);
		AudioManager.Instance.Play("Jump");
	}

	protected void ResetJump()
	{
		jumping = false;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!jumping && other.gameObject.layer == LayerMask.NameToLayer("Ground") && other.gameObject.CompareTag("ResetJump"))
		{
			if (!animator.GetBool("Landed"))
			{
				AudioManager.Instance.Play("Walk1");
			}
			animator.SetBool("Landed", value: true);
			animator.SetBool("Jumping", value: false);
		}
	}

	public bool IsDead()
	{
		return dead;
	}

	public void AddDamage()
	{
		if (!dead)
		{
			damage++;
			DeathConditions();
		}
	}

	protected virtual void DeathConditions()
	{
		if (!dead && damage > 0)
		{
			Kill();
		}
	}

	public void AddCollider(Collider2D c)
	{
		playerColliders.Add(c);
	}

	public Rigidbody2D GetTorsoRb()
	{
		return rb;
	}

	public List<Collider2D> GetColliders()
	{
		return playerColliders;
	}
}
