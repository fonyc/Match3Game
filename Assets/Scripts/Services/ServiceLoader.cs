using System.Threading.Tasks;
using UnityEngine;

public class ServiceLoader : MonoBehaviour
{
    [SerializeField]
    private bool IsDevBuild = true;

    [SerializeField]
    private SceneLoader sceneLoader;

    private TaskCompletionSource<bool> _cancellationTaskSource;

    void Awake()
    {
        _cancellationTaskSource = new();
        LoadServicesCancellable().ContinueWith(task =>
                Debug.LogException(task.Exception),
            TaskContinuationOptions.OnlyOnFaulted);
    }

    private void OnDestroy()
    {
        _cancellationTaskSource.SetResult(true);
    }

    private async Task LoadServicesCancellable()
    {
        await Task.WhenAny(LoadServices(), _cancellationTaskSource.Task);
    }

    private async Task LoadServices()
    {
        string environmentId = IsDevBuild ? "development" : "production";

        ServicesInitializer servicesInitializer = new ServicesInitializer(environmentId);

        //Create services
        GameConfigService gameConfig = new GameConfigService();
        //GameProgressionService gameProgression = new GameProgressionService();

        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        LoginGameService loginService = new LoginGameService();
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        AdsGameService adsService = new AdsGameService("4928657", "Rewarded_Android");
        //UnityIAPGameService iapService = new UnityIAPGameService();
        //IGameProgressionProvider gameProgressionProvider = new GameProgressionProvider();
        //LocalizationService localizationService = new LocalizationService();

        //Register services
        ServiceLocator.RegisterService(gameConfig);
        //ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(adsService);
        ServiceLocator.RegisterService(analyticsService);
        //ServiceLocator.RegisterService<IIAPGameService>(iapService);
        //ServiceLocator.RegisterService(localizationService);

        //Initialize services
        await servicesInitializer.Initialize();
        await loginService.Initialize();
        await remoteConfig.Initialize();
        await analyticsService.Initialize();
        //await iapService.Initialize(new Dictionary<string, string>
        //{
        //    ["test1"] = "es.jacksparrot.match3.test1"
        //});
        await adsService.Initialize(Application.isEditor);
        //await gameProgressionProvider.Initialize();
        //localizationService.Initialize("Spanish", true);

        gameConfig.Initialize(remoteConfig);
        //gameProgression.Initialize(gameConfig, gameProgressionProvider);

        sceneLoader.ChangeScene(1);
    }
}
