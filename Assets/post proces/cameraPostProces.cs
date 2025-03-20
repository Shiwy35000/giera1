using UnityEngine;

[ExecuteInEditMode]
public class cameraPostProces : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }
   
}
