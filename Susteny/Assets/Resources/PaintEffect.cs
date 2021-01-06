using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PaintEffect : MonoBehaviour
{
	public int intensity;
	private Material material;

	void Awake()
	{
		// Create a new material with the supplied shader.
		material = new Material(Shader.Find("SMO/Complete/Painting"));
	}

	// OnRenderImage() is called when the camera has finished rendering.
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (intensity == 0)
		{
			Graphics.Blit(src, dst);
			return;
		}

		material.SetInt("_KernelSize", intensity);
		Graphics.Blit(src, dst, material);
	}
}
