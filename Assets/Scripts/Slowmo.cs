using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Slowmo : MonoBehaviour
{
	private bool slowmo;

	public PostProcessProfile pp;

	private ChromaticAberration ppch;

	private LensDistortion ppld;

	private Vignette ppvi;

	private float vi;

	private float le;

	private float ch;

	private float dvi = 0.4f;

	private float dle = 35f;

	private float dch = 1f;

	private float vvel;

	private float lvel;

	private float cvel;

	private float speed = 0.1f;

	public static Slowmo Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		Instance = this;
		ppch = pp.GetSetting<ChromaticAberration>();
		ppld = pp.GetSetting<LensDistortion>();
		ppvi = pp.GetSetting<Vignette>();
	}

	private void Update()
	{
		UpdateIntensities();
		ppch.intensity.value = Mathf.SmoothDamp(ppch.intensity.value, ch, ref cvel, speed);
		ppld.intensity.value = Mathf.SmoothDamp(ppld.intensity.value, le, ref lvel, speed);
		ppvi.intensity.value = Mathf.SmoothDamp(ppvi.intensity.value, vi, ref vvel, speed);
	}

	private void UpdateIntensities()
	{
		float num = PlayerMovement.Instance.GetSpeed();
		vi = dvi * (1f - num);
		le = dle * (1f - num);
		ch = dch * (1f - num);
	}
}
