using Audio;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private TrailRenderer trail;

	public GameObject hitFx;

	public GameObject bloodFx;

	private void Awake()
	{
		trail = GetComponentInChildren<TrailRenderer>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Player") || other.gameObject.CompareTag("Detatched"))
		{
			if (other.gameObject.CompareTag("Barrel"))
			{
				((Barrel)other.gameObject.GetComponent(typeof(Barrel))).Explode();
			}
			DestroyBullet(other);
		}
	}

	public void DestroyBullet(Collision2D other)
	{
		GameObject original = hitFx;
		if (other != null && other.gameObject.CompareTag("Detatched"))
		{
			original = bloodFx;
		}
		trail.transform.parent = null;
		trail.time = 0.4f;
		UnityEngine.Object.Destroy(base.gameObject);
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
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
		AudioManager.Instance.Play("BulletHit", num2);
	}
}
