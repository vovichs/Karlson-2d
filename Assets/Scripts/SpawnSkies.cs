using UnityEngine;

public class SpawnSkies : MonoBehaviour
{
	public GameObject[] skies;

	private int amount = 10;

	private Vector3 startPos;

	public bool constantSpeed;

	private void Start()
	{
		Spawn();
		startPos = base.transform.localPosition;
	}

	private void Spawn()
	{
		amount = UnityEngine.Random.Range(20, 80);
		for (int i = 0; i < amount; i++)
		{
			Vector2 v = new Vector2(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(0, 15));
			GameObject gameObject = UnityEngine.Object.Instantiate(skies[Random.Range(0, skies.Length)], v, Quaternion.identity);
			gameObject.transform.localScale = Vector2.one * UnityEngine.Random.Range(0.75f, 1.5f);
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = Vector2.zero;
			gameObject.transform.localPosition = v;
		}
	}

	private void Update()
	{
		if (constantSpeed)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x + 0.1f * Time.deltaTime, 0f, base.transform.localPosition.z);
		}
		else
		{
			base.transform.localPosition = (Vector2)startPos - CameraMovement.Instance.GetOffset() * 0.2f;
		}
	}
}
