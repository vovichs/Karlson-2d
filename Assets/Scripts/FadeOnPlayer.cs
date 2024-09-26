using UnityEngine;

public class FadeOnPlayer : MonoBehaviour
{
	private SpriteRenderer sr;

	private Color c;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		c = sr.color;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.transform.root.name == "Player")
		{
			sr.color = new Color(c.r, c.g, c.b, 0f);
		}
	}
}
