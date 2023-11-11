using UnityEngine;

public class LineGraph : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int resolution = 100; // Number of points on the graph
    [SerializeField] private float graphWidth = 10.0f; // Width of the graph in world units
    [SerializeField] private float graphHeight = 5.0f; // Height of the graph in world units
    [SerializeField] private AnimationCurve curve;

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.positionCount = resolution;
        DrawGraph();
    }

    private void DrawGraph()
    {
        float stepSize = graphWidth / (resolution - 1);

        for (int i = 0; i < resolution; i++)
        {
            float x = i * stepSize - graphWidth / 2.0f;
            float y = curve.Evaluate((x + graphWidth / 2.0f) / graphWidth) * graphHeight;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    public void ChangeResolution(int newResolution)
    {
        resolution = newResolution;

        // Update the LineRenderer's positionCount
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = resolution;
        }

        // Redraw the graph with the new resolution
        DrawGraph();
    }
}