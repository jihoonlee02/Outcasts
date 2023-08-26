using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactChecker : MonoBehaviour
{
    public void PathUnlockBasedOnArtifactCollection()
    {
        if (ChestTracker.Instance.IsAllChestsOpen)
        {
            gameObject.SetActive(true);
        }
        else
        {

        }
    }
}
