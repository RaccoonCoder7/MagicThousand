using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSlider : MonoBehaviour
{
    public enum LineType
    {
        I, L, D, C
    };
    public LineType lineType;

    private Transform startTr;
    private Transform secondTr;
    private Transform endTr;

    private LineRenderer lr;
    private float height = 0.025f;
    private float scale;
    private float prevPercent = 0;

    private ShapeControl shapeControl;

    private void Start()
    {
        scale = transform.localScale.x;
        startTr = transform.Find("start").GetComponent<Transform>();
        if (lineType == LineType.L)
        {
            secondTr = transform.Find("second").GetComponent<Transform>();
        }
        if (lineType == LineType.D)
        {
            secondTr = transform.Find("second").GetComponent<Transform>();
            endTr = transform.Find("end").GetComponent<Transform>();
        }
        lr = GetComponent<LineRenderer>();
        shapeControl = transform.parent.GetComponent<ShapeControl>();
        // 로컬스케일이 커짐에 따라 hieght, 및 각 함수의 return부분의 상수 값 변환이 필요할수도 있음.
    }

    public void SetColors(Vector3 hitPos)
    {
        float percent = 0f;
        float distance = Vector3.Distance(hitPos, startTr.position);
        float length = Mathf.Sqrt((distance * distance) - (height * height));

        switch (lineType)
        {
            case LineType.I:
                percent = drawI(length, hitPos);
                break;
            case LineType.L:
                percent = drawL(length, hitPos);
                break;
            case LineType.D:
                percent = drawD(length, hitPos);
                break;
            case LineType.C:
                break;
        }
        if (percent > 0.9f)
        {
            percent = 1;
        }
        if (prevPercent > percent || prevPercent + 0.3f < percent)
        {
            return;
        }
        prevPercent = percent;

        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Fixed;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.black, percent), new GradientColorKey(Color.gray, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
        );
        lr.colorGradient = gradient;

        if (percent >= 1)
        {
            shapeControl.NextShape();
            prevPercent = 0;
        }
    }

    private float drawI(float length, Vector3 hitPos)
    {
        return (length / (1.9f * scale));
    }

    private float drawL(float length, Vector3 hitPos)
    {
        if (length >= scale)
        {
            float secondDistance = Vector3.Distance(hitPos, secondTr.position);
            float secondLength = Mathf.Sqrt((secondDistance * secondDistance) - (height * height));
            length = scale + secondLength;
        }
        return (length / (2.7f * scale));
    }

    private float drawD(float length, Vector3 hitPos)
    {
        if (length > 1.7f)
        {
            float secondDistance = Vector3.Distance(hitPos, secondTr.position);
            float secondLength = Mathf.Sqrt((secondDistance * secondDistance) - (height * height));
            length = 1.7f + secondLength;
        }
        return length / (2.75f * scale);
    }
}