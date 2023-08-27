using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactChecker : MonoBehaviour
{
    [SerializeField] private GameObject makeActiveInstead;
    public void PathUnlockBasedOnArtifactCollection()
    {
        if (ChestTracker.Instance.IsAllChestsOpen)
        {
            gameObject.SetActive(true);
        }
        else
        {
            makeActiveInstead.SetActive(true);
        }
    }
}
