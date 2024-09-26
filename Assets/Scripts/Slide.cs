using Audio;
using UnityEngine;

public class Slide : MonoBehaviour
{
	private bool done;

	public GameObject bloodFx;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.CompareTag("Enemy"))
		{
			return;
		}
		Rigidbody2D rb = PlayerMovement.Instance.GetRb();
		Vector2 velocity = rb.velocity;
		done = true;
		Enemy enemy = (Enemy)other.gameObject.transform.root.GetComponent(typeof(Enemy));
		if (!(enemy == null) && !enemy.IsDead())
		{
			Vector2 vector = Vector2.up * 7f + velocity * 0.2f;
			if (other.gameObject.name != "Foot")
			{
				vector = velocity * 1f;
			}
			enemy.GetTorsoRb().velocity = vector;
			enemy.Kill();
			enemy.GetTorsoRb().angularVelocity = 10000f;
			rb.AddForce(Vector2.right * 150f);
			CameraShake.ShakeOnce(0.15f, 0.8f);
			Object.Instantiate(bloodFx, base.transform.position + -base.transform.up * 0.3f + (Vector3)vector * 0.02f, Quaternion.identity);
			AudioManager.Instance.Play("Jump");
			AudioManager.Instance.Play("Jump");
		}
	}
}
