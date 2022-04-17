using UnityEngine;
using System.Collections.Generic;

public class LightFlickerEffect : MonoBehaviour
{
    [SerializeField] private new Light light;
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField][Range(1, 50)] private int smoothing = 5;

    private Queue<float> smoothQueue;
    private float lastSum = 0;

    public void Reset()
    {
        smoothQueue.Clear();
        lastSum = 0;
    }

    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        if (light == null)
        {
            light = GetComponent<Light>();
        }
    }

    void Update()
    {
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        light.intensity = lastSum / (float)smoothQueue.Count;
    }
}