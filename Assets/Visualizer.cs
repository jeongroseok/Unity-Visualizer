using UnityEngine;
using System.Collections;

public class Visualizer : MonoBehaviour
{
	public int count = 1024;
	public Material material;
	public FFTWindow window;

	private float[] spectrum;
	private Transform[] transforms;

	void Start()
	{
		spectrum = new float[count];
		transforms = new Transform[count];
		for (int i = 0; i < count; i++)
		{
			float angle = 360.0f / count * i * Mathf.Deg2Rad;
			var current = transforms[i] = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			current.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 10;
			current.localScale = Vector3.one * 0.1f;
			current.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
			current.parent = transform;
			current.GetComponent<Renderer>().material = material;
		}
	}

	void Update()
	{
		AudioListener.GetSpectrumData(spectrum, 0, window);
		float min = float.MaxValue, max = 0;
		float total = 0;
		for (int i = 0; i < count; i++)
		{
			total += spectrum[i];
			min = Mathf.Min(min, spectrum[i]);
			max = Mathf.Max(max, spectrum[i]);
		}
		for (int i = 0; i < count; i++)
		{
			transforms[i].localScale = new Vector3((spectrum[i] + spectrum[count - i - 1]) * max * 1000, 1, 1) * 0.1f;
		}
		material.color = Color.Lerp(material.color, new Color(spectrum[(count / 3) * 1], spectrum[(count / 3) * 2], spectrum[(count / 3) * 3], 1) * 1000, Time.deltaTime);
		transform.Rotate(transform.forward, Time.deltaTime * total);
	}
}
