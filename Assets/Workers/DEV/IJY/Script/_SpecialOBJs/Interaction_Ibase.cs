using UnityEngine;

public interface Interaction_Ibase_Activate
{
    public void Activate();
}

public interface Interaction_Ibase_GrabAct
{
    public void Activate_Grab();
}

public enum Box_Type
{
    None,
    Nomal, Bomb,
    Size
}