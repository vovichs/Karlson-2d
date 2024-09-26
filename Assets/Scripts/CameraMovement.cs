using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Transform player;

	private float speed = 0.4f;

	private Vector2 velSpeed;

	private float defaultZoom = 6f;

	private float zoomSpeed = 1f;

	private float velZoom;

	public float minY;

	private Vector2 startPos;

	public Camera c;

	public static CameraMovement Instance
	{
		get;
		set;
	}

	private void Awake()
	{
		Instance = this;
		startPos = base.transform.position;
		Camera.main.orthographicSize = defaultZoom;
	}

	private void Update()
	{
		if (!(player == null))
		{
			float num = PlayerMovement.Instance.GetRb().velocity.magnitude;
			Vector2 vector = PlayerMovement.Instance.GetRb().velocity * 0.65f;
			Vector2 target = new Vector2(player.transform.position.x + vector.x, player.transform.position.y + vector.y / 1f);
			base.transform.position = Vector2.SmoothDamp(base.transform.position, target, ref velSpeed, speed);
			if (num > 11f)
			{
				num = 11f;
			}
			float num2 = defaultZoom + num / 4f;
			if (num2 > 11f)
			{
				num2 = 11f;
			}
			Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, num2, ref velZoom, zoomSpeed);
			float y = base.transform.position.y;
			if (y < minY)
			{
				y = minY;
			}
			base.transform.position = new Vector3(base.transform.position.x, y, -20f);
		}
	}

	public void SetPlayer(Transform t)
	{
		player = t;
	}

	public Vector2 GetOffset()
	{
		return (Vector2)base.transform.position - startPos;
	}
}
