using UnityEngine;

public class DestroySelf : MonoBehaviour
{
	public float dtime;

	public GameObject explosion;

	private void Start()
	{
		Object.Instantiate(explosion, base.transform.position, Quaternion.identity);
		Invoke("DestroyGo", dtime);
	}

	private void DestroyGo()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
