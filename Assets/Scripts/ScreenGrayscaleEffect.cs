using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenGrayscaleEffect : MonoBehaviour
{
    [Range(0f, 1f)]
    public float desaturation = 0f; // 0 = off, 1 = full desaturation (B&W)

    [Range(0.5f, 2f)]
    public float contrast = 1f; // 1 = unchanged, >1 increases contrast

	public Shader grayscaleShader;

	private Material grayscaleMaterial;

	void OnEnable()
	{
		EnsureMaterial();
	}

	void OnDisable()
	{
		if (grayscaleMaterial != null)
		{
			DestroyImmediate(grayscaleMaterial);
			grayscaleMaterial = null;
		}
	}

    public void SetEnabled(bool enabled, float targetDesaturation = 1f, float targetContrast = 1.2f)
	{
        desaturation = enabled ? Mathf.Clamp01(targetDesaturation) : 0f;
        contrast = enabled ? Mathf.Clamp(targetContrast, 0.5f, 2f) : 1f;
	}

	void EnsureMaterial()
	{
		if (grayscaleShader == null)
		{
			grayscaleShader = Shader.Find("Hidden/GrayscaleEffect");
		}
		if (grayscaleShader != null && (grayscaleMaterial == null || grayscaleMaterial.shader != grayscaleShader))
		{
			grayscaleMaterial = new Material(grayscaleShader);
			grayscaleMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
        if (desaturation <= 0f)
		{
			Graphics.Blit(src, dst);
			return;
		}

		EnsureMaterial();
		if (grayscaleMaterial == null)
		{
			Graphics.Blit(src, dst);
			return;
		}

        grayscaleMaterial.SetFloat("_Desaturation", desaturation);
        grayscaleMaterial.SetFloat("_Contrast", contrast);
		Graphics.Blit(src, dst, grayscaleMaterial);
	}
}


