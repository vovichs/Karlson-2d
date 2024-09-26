using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Transform camTransform;

	private static float shakeDuration = 0f;

	private static float shakeAmount = 0.7f;

	private float vel;

	private Vector3 vel2 = Vector3.zero;

	private Vector3 originalPos;

	private void Awake()
	{
		if (camTransform == null)
		{
			camTransform = base.transform;
		}
		originalPos = camTransform.localPosition;
	}

	public static void ShakeOnce(float lenght, float strength)
	{
		shakeDuration = lenght;
		shakeAmount = strength;
	}

	private void Update()
	{
		if (shakeDuration > 0f)
		{
			Vector3 target = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
			camTransform.localPosition = Vector3.SmoothDamp(camTransform.localPosition, target, ref vel2, 0.05f);
			shakeDuration -= Time.deltaTime;
			shakeAmount = Mathf.SmoothDamp(shakeAmount, 0f, ref vel, 0.7f);
		}
		else
		{
			camTransform.localPosition = originalPos;
		}
	}
}
