using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class ServicesInitializer 
{
    private string _environmentName;

    public ServicesInitializer(string environmentName)
    {
        _environmentName = environmentName;
    }

    public async Task Initialize(CancellationTokenSource cancellationToken)
    {
        InitializationOptions options = new InitializationOptions();
        if (!string.IsNullOrEmpty(_environmentName))
        {
            options.SetEnvironmentName(_environmentName);
        }

        var task = UnityServices.InitializeAsync(options);

        while(task.Status != TaskStatus.Faulted && task.Status != TaskStatus.RanToCompletion)
        {
            await Task.Delay(200, cancellationToken.Token);
        }
        UnityEngine.Debug.Log(task.Status);
    }
}
