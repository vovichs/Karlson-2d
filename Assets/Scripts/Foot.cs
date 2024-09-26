using Audio;
using UnityEngine;

public class Foot : MonoBehaviour
{
	private bool ready;

	private void Start()
	{
		ready = true;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (ready && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			AudioManager.Instance.Play("Walk1");
			AudioManager.Instance.Play("Walk2");
			Invoke("Reload", 0.3f);
			ready = false;
		}
	}

	private void Reload()
	{
		ready = true;
	}
}
