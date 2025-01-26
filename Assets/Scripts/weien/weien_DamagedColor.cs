using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_DamagedColor : MonoBehaviour
{
    private SpriteRenderer sr;
    Color defaultColor;
    public float colorDuration = .2f;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
    }

    IEnumerator HitColor()
    {
        sr.color = new Color(1f, .2f, .2f);
        yield return new WaitForSeconds(colorDuration);
        sr.color = defaultColor;
    }
}
