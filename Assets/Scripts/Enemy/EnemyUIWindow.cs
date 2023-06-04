using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for showing enemies count in UI
/// </summary>
public class EnemyUIWindow : MonoBehaviour
{
    [SerializeField] private int showTime = 3;
    [SerializeField] private Text uItext;
    private CancellationTokenSource cancellationTokenSource;

    private void Start()
    {
        EventsBus.Subscribe<OnRefreshUIText>(OnRefreshUIText);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventsBus.Unsubscribe<OnRefreshUIText>(OnRefreshUIText);
        if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
        cancellationTokenSource.Dispose();
    }

    private async void OnRefreshUIText(OnRefreshUIText data)
    {
        if (data.className != this.GetType().Name)
            return;

        if (gameObject == null)
            return;

        gameObject.SetActive(true);

        if (uItext == null)
            return;

        uItext.text = data.text;

        cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await Task.Delay(showTime * 1000, cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (gameObject != null)
            gameObject.SetActive(false);
    }

}
