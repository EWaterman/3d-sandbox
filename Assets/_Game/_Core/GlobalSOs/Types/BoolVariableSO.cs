using UnityEngine;

[CreateAssetMenu(menuName = "GlobalVariable/Bool")]
public class BoolVariableSO : VariableSO<bool>
{
    public void SetTrue()
    {
        SetValue(true);
    }

    public void SetFalse()
    {
        SetValue(false);
    }
}
