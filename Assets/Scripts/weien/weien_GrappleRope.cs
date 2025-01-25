using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_GrappleRope : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] weien_PlayerController playerController;
    [SerializeField] private int resolution, waveCount, wobbleCount;
    [SerializeField] private float waveSize, animSpeed;
    [SerializeField] public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //line.SetPosition(0, player.position);
    }

    public IEnumerator AnimateRope(Vector3 targetPos)
    {
        line.positionCount = resolution;
        float angle = LookAtAngle(targetPos - player.position);
        float percent = 0;
        while (percent <= 1f)
        {
            percent += Time.deltaTime * animSpeed;
            SetPoints(targetPos, percent, angle);
            yield return null;
        }
        SetPoints(targetPos, 1, angle);
    }

    private void SetPoints(Vector3 targetPos, float percent, float angle)
    {
        Vector3 ropeEnd = Vector3.Lerp(player.position, targetPos, percent);
        float length = Vector2.Distance(player.position, ropeEnd);

        for (int i = 0; i < resolution; i++)
        {
            float xPos = (float)i / resolution * length;
            float reversePercent = (1 - percent);

            float amplitude = Mathf.Sin(reversePercent * wobbleCount * Mathf.PI);

            float yPos = Mathf.Sin((float)waveCount * i / resolution * 2 * Mathf.PI * reversePercent) * amplitude;

            Vector2 pos = RotatePoint(new Vector2(xPos + player.position.x, yPos + player.position.y), player.position, angle);
            if (i >= 0 && i < line.positionCount) {
                line.SetPosition(i, pos);
            }
        }
    }

    Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        Vector2 dir = point - pivot;
        dir = Quaternion.Euler(0, 0, angle) * dir;
        point = dir + pivot;
        return point;
    }

    private float LookAtAngle(Vector2 target)
    {
        return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
    }
}
