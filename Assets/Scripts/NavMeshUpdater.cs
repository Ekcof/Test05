using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface navMeshSurface; // Reference to the NavMeshSurface component
    public float refreshPeriod = 1f; // Refresh period in seconds

    private void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        StartRefreshTimer();
    }

    private async void StartRefreshTimer()
    {
        while (true)
        {
            await Task.Delay((int)(refreshPeriod * 1000));
            RefreshNavMesh();
        }
    }

    private void RefreshNavMesh()
    {
        navMeshSurface.RemoveData(); // Remove existing NavMesh data
        navMeshSurface.BuildNavMesh(); // Rebuild the NavMesh
    }
}