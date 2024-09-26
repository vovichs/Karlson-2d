using Audio;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
	public GameObject replace;

	private bool done;

	public GameObject bloodFx;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
		{
			UnityEngine.Object.Destroy(other.gameObject);
			Object.Instantiate(bloodFx, other.transform.position, Quaternion.identity);
			AudioManager.Instance.Play("EnemyHit");
			RemoveBodypart(other.gameObject);
			if (base.transform.childCount > 0 && base.transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("Player") && base.transform.GetChild(0).gameObject.CompareTag("Player"))
			{
				((BodyPart)base.transform.GetChild(0).GetComponent(typeof(BodyPart))).RemoveBodypart(other.gameObject);
			}
		}
	}

	public void RemoveBodypart(GameObject other)
	{
		if (done)
		{
			return;
		}
		done = true;
		Actor actor = (Actor)base.transform.root.GetComponent(typeof(Actor));
		Vector2 velocity = other.gameObject.GetComponent<Rigidbody2D>().velocity;
		if (base.gameObject.name != "Torso")
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(replace, base.transform.position, base.transform.rotation);
			gameObject.tag = "Detatched";
			Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
			component.interpolation = RigidbodyInterpolation2D.Interpolate;
			component.AddForce(velocity * 200f);
			Collider2D component2 = gameObject.GetComponent<Collider2D>();
			PlayerMovement.Instance.IgnoreWithOneObject(component2);
			if (actor != null)
			{
				actor.AddCollider(component2);
				actor.IgnoreWithOneObject(component2);
			}
		}
		if (actor != null)
		{
			actor.AddDamage();
			if (base.gameObject.name == "Head")
			{
				actor.Kill();
			}
			if (actor.IsDead())
			{
				actor.GetTorsoRb().velocity = velocity * 2f + Vector2.up * 3f;
			}
		}
		if (base.gameObject.name != "Torso")
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
