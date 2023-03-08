using UnityEngine;
public interface ISelectable
{
    Transform transform{ get; }
    void SetHighlight(bool flag);
    void Hit();
}
