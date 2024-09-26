using Audio;
using UnityEngine;

public class Window : MonoBehaviour
{
	private bool done;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (done)
		{
			return;
		}
		Rigidbody2D component = other.gameObject.GetComponent<Rigidbody2D>();
		if (other.gameObject.CompareTag("Player"))
		{
			component = other.transform.root.GetComponent<Rigidbody2D>();
		}
		if (component == null)
		{
			return;
		}
		float magnitude = component.velocity.magnitude;
		if (magnitude < 5f && magnitude > -5f)
		{
			MonoBehaviour.print("too slow noob");
			return;
		}
		done = true;
		Transform child = base.transform.GetChild(0);
		child.gameObject.SetActive(value: true);
		child.parent = null;
		if (magnitude < 0f)
		{
			child.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		CameraShake.ShakeOnce(0.3f, 1.5f);
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
		AudioManager.Instance.Play("WindowBreak", num2);
	}
}
