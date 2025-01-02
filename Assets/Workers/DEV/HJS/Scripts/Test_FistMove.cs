using System.Collections;
using UnityEngine;

public class Test_FistMove : MonoBehaviour
{
    [Header("Fist")]
    [SerializeField] GameObject body;
    [SerializeField] float fistSpeed;
    [SerializeField] float fistTime;
    [SerializeField] float width;  // x
    [SerializeField] float height; // y
    [SerializeField] float length; // z
    [Header("Shader")]
    [SerializeField] Material material;
    [SerializeField] float flow;
    [SerializeField] float curFlow;
    [SerializeField] float ReturnTime;


    private void Start()
    {
        body.transform.localPosition = Vector3.back * body.transform.localScale.z * 0.5f;
        flow = body.transform.localScale.z * 0.5f;
        SetFlow("_flow", flow + 0.1f);
        length = body.transform.localScale.z;
        StartCoroutine(MoveReturnRoutine());
    }

    private IEnumerator MoveForwardRoutine(Vector3 startPos, Vector3 endPos)
    {
        yield return Util.GetDelay(1f);
        Debug.Log("앞으로 움직이기 시작!");

        // 거리가 0 ~ distance 만큼 간다
        // 해당 flow 값을 4.1 ~ -4.1 로 변경해야한다
        // 
        float elapsedTime = 0f;
        while ((elapsedTime * fistSpeed) < fistTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime * fistSpeed) / fistTime);
            curFlow = Remap(Vector3.Magnitude(transform.position - startPos), 0f, length, flow, -flow);
            SetFlow("_flow", curFlow);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (curFlow >= flow - 0.2f) SetFlow("_flow", -(flow + 0.1f));
        transform.position = endPos;

        Debug.Log("앞으로 움직이기 종료!");
    }

    private IEnumerator MoveReturnRoutine()
    {
        // 거리(최대 : Length만큼) = 속도 * 시간
        float distance = Mathf.Min(fistSpeed * fistTime, length);

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.forward * distance;

        yield return MoveForwardRoutine(startPos, endPos);

        yield return Util.GetDelay(1f);

        Debug.Log("뒤로 움직이기 시작!");

        float elapsedTime = 0f;
        while ((elapsedTime * fistSpeed) < fistTime)
        {
            transform.position = Vector3.Lerp(endPos, startPos, (elapsedTime * fistSpeed) / fistTime);
            curFlow =  Remap(Vector3.Magnitude(transform.position - startPos), length, 0f, -flow, flow);
            SetFlow("_flow", curFlow);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (curFlow >= -flow + 0.2f) SetFlow("_flow", (flow + 0.1f));
        transform.position = startPos;

        Debug.Log("뒤로 움직이기 종료!");
    }

    private void SetFlow(string name, float value)
    {
        material.SetFloat(name, value);
    }

    public float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
    }

}
