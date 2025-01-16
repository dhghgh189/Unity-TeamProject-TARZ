using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    [SerializeField] float phaseTime;
    [SerializeField] float glowSize;
    [SerializeField] Color glowColor;

    private List<Material> materials = new List<Material>();

    private void Awake()
    {
        foreach (var render in GetComponentsInChildren<Renderer>())
        {
            materials.AddRange(render.materials);
        }
        foreach (var material in materials)
        {
            // 이전 material 필드 저장
            Texture texture = material.mainTexture;
            Vector2 tiling = material.mainTextureScale;

            // 쉐이더 변경 후 초기화
            material.shader = Shader.Find("Shader Graphs/Building");

            material.SetTexture("_Base_Map", texture);
            material.SetVector("_Tiling", tiling);

            material.SetFloat("_Split_Value", 25f);
            material.SetFloat("_Glow_Size", glowSize);
            material.SetColor("_Glow_Color", glowColor);
        }
    }
    public void StartPhase(bool back, GameObject temp = null)
    {
        StartCoroutine(back ? BackPhase(temp) : Phase());
    }
    IEnumerator Phase()
    {
        foreach (var material in materials)
        {
            material.SetFloat("_Split_Value", 0);
        }

        float currentPhase = 0f;

        while (currentPhase < 25f)
        {
            currentPhase += 25f * (1f / phaseTime) * Time.deltaTime;

            foreach (var material in materials)
            {
                material.SetFloat("_Split_Value", currentPhase);
            }

            yield return null;
        }
    }
    IEnumerator BackPhase(GameObject temp)
    {
        float currentPhase = 25f;

        while (currentPhase > 0)
        {
            currentPhase -= 25f * (1f / phaseTime) * Time.deltaTime;

            foreach (var material in materials)
            {
                material.SetFloat("_Split_Value", currentPhase);
            }

            yield return null;
        }
        temp.SetActive(false);
    }
}
