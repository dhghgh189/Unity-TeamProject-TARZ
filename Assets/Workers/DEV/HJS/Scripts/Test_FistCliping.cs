using System.Collections;
using UnityEngine;

public class Test_FistCliping : MonoBehaviour
{
    [Header("Fist")]
    [SerializeField] GameObject body;
    [SerializeField] float fistSpeed;
    [SerializeField] float fistTime;
    [SerializeField] float width;  // x
    [SerializeField] float height; // y
    [SerializeField] float length; // z
    [SerializeField] float alpha;
    [Header("Materials")]
    [SerializeField] float flow;
    [SerializeField] float curFlow;
    [Space(2)]
    [Header("Hand")]
    [SerializeField] GameObject handObj;
    [SerializeField] Material handMaterial;
    [SerializeField] float handCenter;
    [SerializeField] float handUnvisible;
    [SerializeField] float handVisible;
    [Space(2)]
    [Header("Body")]
    [SerializeField] GameObject bodyObj;
    [SerializeField] Material bodyMaterial;
    [SerializeField] float bodyCenter;
    [SerializeField] float bodyUnvisible = 0f;
    [SerializeField] float bodyVisible = 0f;
    [Space(2)]
    [Header("Fistol")]
    [SerializeField] GameObject fistolObj;
    [SerializeField] Material fistolMaterial;
    [SerializeField] float fistolCenter;
    [SerializeField] float fistolUnvisible = 0f;
    [SerializeField] float fistolVisible = 0f;

    BoxCollider bc;

    [ContextMenu("data")]
    public void SetData()
    {
        bc = handObj.GetComponent<BoxCollider>();
        handCenter = bc.center.y;
        handUnvisible = handCenter - bc.size.y * 0.5f;
        handVisible = handCenter + bc.size.y * 0.5f;
        bc = bodyObj.GetComponent<BoxCollider>();
        bodyCenter = bc.center.y;
        bodyUnvisible = bodyCenter - bc.size.y * 0.5f;
        bodyVisible = bodyCenter + bc.size.y * 0.5f;
        bc = fistolObj.GetComponent<BoxCollider>();
        fistolCenter = bc.center.y;
        fistolUnvisible = fistolCenter - bc.size.y * 0.5f;
        fistolVisible = fistolCenter + bc.size.y * 0.5f;

    }
    private void Start()
    {
        handMaterial.SetFloat("_AlphaValue", Mathf.Clamp(alpha, 0f, 1f));
        bodyMaterial.SetFloat("_AlphaValue", Mathf.Clamp(alpha, 0f, 1f));
        fistolMaterial.SetFloat("_AlphaValue", Mathf.Clamp(alpha, 0f, 1f));

        body.transform.localPosition = Vector3.back * body.transform.localScale.z * 2f;
        flow = body.transform.localScale.z;
        handMaterial.SetFloat("_flow", handUnvisible);
        bodyMaterial.SetFloat("_flow", bodyUnvisible);
        fistolMaterial.SetFloat("_flow", fistolUnvisible);
        length = body.transform.localScale.z * 2f;

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
            curFlow = Remap(Vector3.Magnitude(transform.position - startPos), 0f, length, -flow, flow);
            SetFlow(Remap(curFlow, -flow, flow, handUnvisible, fistolVisible));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetFlow(curFlow + 0.1f);
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
            curFlow = Remap(Vector3.Magnitude(transform.position - endPos), 0, length, flow, -flow);
            SetFlow(Remap(curFlow, flow, -flow, fistolVisible, handUnvisible));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetFlow(curFlow - 0.1f);
        transform.position = startPos;

        Debug.Log("뒤로 움직이기 종료!");
    }

    // 2 ~ 0
    private void SetFlow(float value, bool reverse = true)
    {
        handMaterial.SetFloat("_flow", value);
        bodyMaterial.SetFloat("_flow", value);
        fistolMaterial.SetFloat("_flow", value);
    }

    public float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
    }


}
