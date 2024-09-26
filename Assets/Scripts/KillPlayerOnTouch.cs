using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Actor actor = (Actor)other.transform.root.GetComponent(typeof(Actor));
		if (!(actor == null))
		{
			actor.Kill();
		}
	}
}
