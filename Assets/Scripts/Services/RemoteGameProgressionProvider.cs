using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

public class RemoteGameProgressionProvider : IGameProgressionProvider
{
    private string _remoteData = string.Empty;
    bool isSending;

    private async Task SaveFilesInCloud()
    {
        isSending = true;
        await Task.Delay(500);

        try
        {
            await CloudSaveService.Instance.Data
                .ForceSaveAsync(new Dictionary<string, object> { { "data", _remoteData } });
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        isSending = false;
    }

    public async Task<bool> Initialize()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync();
        savedData.TryGetValue("data", out _remoteData);
        return true;
    }

    public string Load()
    {
        return _remoteData;
    }

    public void Save(string text)
    {
        _remoteData = text;

        if (!isSending) SaveFilesInCloud().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }
}