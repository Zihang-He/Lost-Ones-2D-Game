using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenHighContrastEffect : MonoBehaviour
{
	[Range(0.5f, 3f)]
	public float contrast = 1f;

	[Range(-1f, 1f)]
	public float brightness = 0f;

	public Shader highContrastShader;
	private Material highContrastMaterial;

	void OnEnable()
	{
		EnsureMaterial();
	}

	void OnDisable()
	{
		if (highContrastMaterial != null)
		{
			DestroyImmediate(highContrastMaterial);
			highContrastMaterial = null;
		}
	}

	public void SetEnabled(bool enabled, float targetContrast = 1.6f, float targetBrightness = 0f)
	{
		contrast = enabled ? Mathf.Clamp(targetContrast, 0.5f, 3f) : 1f;
		brightness = enabled ? Mathf.Clamp(targetBrightness, -1f, 1f) : 0f;
	}

	void EnsureMaterial()
	{
		if (highContrastShader == null)
		{
			highContrastShader = Shader.Find("Hidden/HighContrastEffect");
		}
		if (highContrastShader != null && (highContrastMaterial == null || highContrastMaterial.shader != highContrastShader))
		{
			highContrastMaterial = new Material(highContrastShader);
			highContrastMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (Mathf.Approximately(contrast, 1f) && Mathf.Approximately(brightness, 0f))
		{
			Graphics.Blit(src, dst);
			return;
		}

		EnsureMaterial();
		if (highContrastMaterial == null)
		{
			Graphics.Blit(src, dst);
			return;
		}

		highContrastMaterial.SetFloat("_Contrast", contrast);
		highContrastMaterial.SetFloat("_Brightness", brightness);
		Graphics.Blit(src, dst, highContrastMaterial);
	}
}


