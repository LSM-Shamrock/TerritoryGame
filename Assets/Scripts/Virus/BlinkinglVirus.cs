using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkinglVirus : Virus
{
    [SerializeField] float MinSize = 0.3f;
    [SerializeField] float MaxSize = 1f;
    [SerializeField] float showDuration;
    [SerializeField] float hideDuration;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(RepeatBlink());
    }

    private IEnumerator RepeatBlink()
    {
        var collider = GetComponentInChildren<Collider2D>();
        var renderer = GetComponentInChildren<Renderer>();
        while (true)
        {
            collider.enabled = true;
            renderer.enabled = true;
            yield return ShowRandomScale();
            yield return new WaitForSeconds(showDuration);
            yield return HideScale();
            collider.enabled = false;
            renderer.enabled = false;
            yield return new WaitForSeconds(hideDuration);
        }
    }

    private IEnumerator ShowRandomScale()
    {
        var size = Random.Range(MinSize, MaxSize);
        var scale = Vector3.one * size;
        var showTime = 1f;
        var showTimer = 0f;
        while (showTimer < showTime)
        {
            showTimer += Time.deltaTime;
            if (showTimer > showTime) showTimer = showTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, scale, showTimer);
            yield return null;
        }
    }

    private IEnumerator HideScale()
    {
        var scale = transform.localScale;
        var showTime = 1f;
        var showTimer = 0f;
        while (showTimer < showTime)
        {
            showTimer += Time.deltaTime;
            if (showTimer > showTime) showTimer = showTime;
            transform.localScale = Vector3.Lerp(scale, Vector3.zero, showTimer);
            yield return null;
        }
    }
}
