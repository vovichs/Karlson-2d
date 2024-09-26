using UnityEngine;

public class WarnEnemies : MonoBehaviour
{
	public Enemy[] enemies;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!(other.gameObject.transform.root.name == "Player") && !other.CompareTag("Bullet"))
		{
			return;
		}
		for (int i = 0; i < enemies.Length; i++)
		{
			if (enemies[i] != null)
			{
				enemies[i].Alert();
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
