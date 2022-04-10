using UnityEngine;
using UnityEngine.Assertions;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private int currentIndex = 0;
    public Transform CurrentPoint { get => points[currentIndex]; }
    public Transform[] Points { get => points; }

    private void Awake()
    {
        Assert.AreNotEqual(0, points.Length);
    }

    public Transform NextPointInPath()
    {
        return points[++currentIndex % points.Length];
    }
}
