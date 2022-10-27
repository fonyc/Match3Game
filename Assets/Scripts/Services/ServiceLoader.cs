using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ServiceLoader : MonoBehaviour
{
    [SerializeField]
    private bool IsDevBuild = true;

    [SerializeField]
    private SceneLoader sceneLoader;

    private CancellationTokenSource _cancellationTaskSource;

    void Awake()
    {
        _cancellationTaskSource = new();
        LoadServicesCancellable().ContinueWith(task =>
                Debug.LogException(task.Exception),
            TaskContinuationOptions.OnlyOnFaulted);
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        _cancellationTaskSource.Cancel();
    }

    private async Task LoadServicesCancellable()
    {
        await LoadServices();
    }

    private async Task LoadServices()
    {
        ServiceLocator.UnregisterAll();

        string environmentId = IsDevBuild ? "development" : "production";
        ServicesInitializer servicesInitializer = new ServicesInitializer(environmentId);

        GameConfigService gameConfig = new GameConfigService();
        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        LoginGameService loginService = new LoginGameService();
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        AdsGameService adsService = new AdsGameService("4928657", "Rewarded_Android");
        UnityIAPGameService iapService = new UnityIAPGameService();
        GameProgressionService gameProgression = new GameProgressionService();
        IGameProgressionProvider gameProgressionProvider = new GameProgressionProvider();

        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(adsService);
        ServiceLocator.RegisterService(analyticsService);
        ServiceLocator.RegisterService<IIAPGameService>(iapService);


        await servicesInitializer.Initialize(_cancellationTaskSource);
        await loginService.Initialize();
        await remoteConfig.Initialize();
        await analyticsService.Initialize();
        await iapService.Initialize(new Dictionary<string, string>
        {
            ["100gems"] = "com.fonangames.timelessheroes.100gems",
            ["500gems"] = "com.fonangames.timelessheroes.500gems"
        });
        bool adsInitialized = await adsService.Initialize(Application.isEditor);
        await gameProgressionProvider.Initialize();
        gameConfig.Initialize(remoteConfig);
        gameProgression.Initialize(gameConfig, gameProgressionProvider);

        sceneLoader.ChangeScene(1);
    }
}
