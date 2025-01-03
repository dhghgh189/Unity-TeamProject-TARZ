using UnityEngine;

public class ThrowBox_Nomal : SpecialInteraction_Script, IThrowBox
{
    private LayerMask NomalBoxLayer;

    void Start()
    {
        NomalBoxLayer = LayerMask.NameToLayer("Monster");
    }

    public void ThrowingBox(GameObject OBJ)
    {
        if (OBJ.layer == NomalBoxLayer)
        {
            OBJ.GetComponent<IDamagable>().TakeDamage(10f);
        }

        Destroy(this.gameObject, 1.5f);
    }
}
