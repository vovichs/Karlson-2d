using UnityEngine;

public class GunDetectPlayer : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			PlayerMovement playerMovement = (PlayerMovement)other.transform.root.GetComponent(typeof(PlayerMovement));
			if (!(playerMovement == null) && !playerMovement.availableGuns.Contains(base.gameObject.transform.parent.gameObject))
			{
				playerMovement.availableGuns.Add(base.gameObject.transform.parent.gameObject);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			PlayerMovement playerMovement = (PlayerMovement)other.transform.root.GetComponent(typeof(PlayerMovement));
			if (!(playerMovement == null) && playerMovement.availableGuns.Contains(base.gameObject.transform.parent.gameObject))
			{
				playerMovement.availableGuns.Remove(base.gameObject.transform.parent.gameObject);
			}
		}
	}
}
