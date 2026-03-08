using UnityEngine;

public class FreezeAfterTracking : MonoBehaviour
{
    [Header("Drag SkeletonRoot here")]
    public Transform heartRoot;   // keep name if you already use it (it can be SkeletonRoot)

    private bool locked = false;
    private Transform imageTarget;

    void Awake()
    {
        imageTarget = transform; // this script is on ImageTarget
    }

    //This will now show up in the OnTargetFound dropdown
    public void LockInWorldSpace()
    {
        if (locked) return;
        if (heartRoot == null) return;

        // detach from ImageTarget so it stops following it
        heartRoot.SetParent(null, true);

        locked = true;
        Debug.Log("Skeleton locked in world space.");
    }

    // Optional: if you ever want a reset button later
    public void ResetToTarget()
    {
        if (heartRoot == null) return;

        heartRoot.SetParent(imageTarget, true);
        locked = false;
        Debug.Log("Skeleton re-attached to ImageTarget.");
    }
}