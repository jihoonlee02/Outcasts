using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRend;
    [SerializeField]
    private SpriteRenderer spriteRend;
    [SerializeField]
    private Transform targetDir;
    [SerializeField]
    private float smoothSpeed;

    private Vector3[] tailPositions;
    private Vector3[] tailPositionsFlipped;
    private Vector3[] segmentPoses;
    private Vector3[] segmentVel;
    // Start is called before the first frame update
    private void Start()
    {
        int length = lineRend.positionCount;
        segmentPoses = new Vector3[length];
        segmentVel = new Vector3[length];

        tailPositions = new Vector3[length];
        tailPositionsFlipped = new Vector3[length];
        for (int i = 0; i < length; i++) {
            tailPositions[i] = lineRend.GetPosition(i);
            tailPositionsFlipped[i] = new Vector3(-tailPositions[i].x, tailPositions[i].y, tailPositions[i].z);
            segmentPoses[i] = tailPositions[i];
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3[] correctTailP = spriteRend.flipX ? tailPositions : tailPositionsFlipped;
        segmentPoses[0] = targetDir.position + correctTailP[0];
        for (int i = 1; i < segmentPoses.Length; i++) {
            //segmentPoses[i] = Vector3.Lerp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * (tailPositions[i]-tailPositions[i-1]).magnitude, smoothSpeed);
            // Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i-1]).normalized * targetDist;
            // segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentVel[i], smoothSpeed);
            segmentPoses[i] = ((segmentPoses[i] - segmentPoses[i-1]) * ((correctTailP[i]-correctTailP[i-1]).magnitude/(segmentPoses[i] - segmentPoses[i-1]).magnitude)) + segmentPoses[i-1];
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetDir.position + correctTailP[i], ref segmentVel[i], smoothSpeed);
        }
        lineRend.SetPositions(segmentPoses);
    }
}
