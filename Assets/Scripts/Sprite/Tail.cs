using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField]
    private int length;
    [SerializeField]
    private LineRenderer lineRend;
    [SerializeField]
    private Transform targetDir;
    [SerializeField]
    private float targetDist;
    [SerializeField]
    private float smoothSpeed;

    private Vector3[] segmentPoses;
    private Vector3[] segmentVel;
    // Start is called before the first frame update
    private void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVel = new Vector3[length];
    }

    // Update is called once per frame
    private void Update()
    {
        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < segmentPoses.Length; i++) {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * targetDist, ref segmentVel[i], smoothSpeed);
            // Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i-1]).normalized * targetDist;
            // segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentVel[i], smoothSpeed);
        }
        lineRend.SetPositions(segmentPoses);
    }
}
