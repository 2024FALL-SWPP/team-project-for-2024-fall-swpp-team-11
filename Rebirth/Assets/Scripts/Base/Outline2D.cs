using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Outline2D : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // 인스펙터에서 설정할 수 있는 thickness 값들
    [SerializeField, Tooltip("On Focus 시 적용할 두께 값")]
    private float focusThickness = 1f;
    private float defocusThickness = 0f;

    private static readonly int ThicknessProperty = Shader.PropertyToID("_Thickness");
    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(ThicknessProperty, defocusThickness);
        spriteRenderer.SetPropertyBlock(mpb);
    }

    public void SetOutline()
    {
        if (spriteRenderer == null) return;

        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.TWO_DIMENSION)
        {
            Debug.Log("Setting Outline");
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(ThicknessProperty, focusThickness);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }

    public void UnsetOutline()
    {
        if (spriteRenderer == null) return;

        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.TWO_DIMENSION)
        {
            Debug.Log("Unsetting Outline");
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(ThicknessProperty, defocusThickness);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
