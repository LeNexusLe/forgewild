using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public void OnPopupHideEnd()
    {
        gameObject.SetActive(false);
    }
}
