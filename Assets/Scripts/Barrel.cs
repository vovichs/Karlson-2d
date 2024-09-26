using Audio;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	public GameObject explosion;

	public void Explode()
	{
		Object.Instantiate(explosion, base.transform.position, Quaternion.identity);
		UnityEngine.Object.Destroy(base.gameObject);
		CameraShake.ShakeOnce(0.4f, 1.6f);
		AudioManager.Instance.Play("Explosion");
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.L))
		{
			Explode();
		}
	}
}
