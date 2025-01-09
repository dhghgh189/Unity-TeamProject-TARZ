using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] AnimationCurve opacityCurve;
    [SerializeField] AnimationCurve YPosCurve;
    [SerializeField] AnimationCurve scaleCurve;

    private TMP_Text tmp;
    private Camera cam;

    private Vector3 originScale;

    private DamagePopUpManager damagePopUpManager;
    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        cam = Camera.main;
        originScale = transform.localScale;

        damagePopUpManager = GetComponentInParent<DamagePopUpManager>();
    }
    private void Update()
    {
        transform.forward = cam.transform.forward;
    }

    public void PopUpInitAndStart(Vector3 pos, string text, Color color)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        tmp.text = text;
        tmp.color = new Color(color.r, color.g, color.b, 0);
        transform.localScale = originScale;
        StartCoroutine(OnDamagePopUp());
    }

    IEnumerator OnDamagePopUp()
    {
        float time = 0;

        Color opacityColor = tmp.color;
        Vector3 origin = transform.position;
        while (time < 1)
        {
            opacityColor.a = opacityCurve.Evaluate(time);
            tmp.color = opacityColor;

            transform.localScale = originScale * scaleCurve.Evaluate(time);

            transform.position = origin + new Vector3(0, YPosCurve.Evaluate(time), 0);

            time += Time.deltaTime;
            yield return null;
        }
        damagePopUpManager.ReturnPool(this);
    }
}
