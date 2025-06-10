using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class AddressableLoader
{
    /*public static async Task<bool> AddressExists(string address)
    {
        AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(address);
        await handle.Task;

        bool exists = handle.Status == AsyncOperationStatus.Succeeded && handle.Result.Count > 0;
        Addressables.Release(handle);

        return exists;
    }
    public static async Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load addressable asset at {address}");
            return null;
        }
    }*/
    public static bool IsAssetExist(string addressableName, System.Type assetType)
    {
        var locators = Addressables.ResourceLocators;
        foreach (IResourceLocator locator in locators)
        {
            if (locator.Locate(addressableName, assetType, out var locations) && locations.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public static async Task<T> LoadAsset<T>(string key)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;
        T asset = default;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            asset = handle.Result;
        }
        else
        {
             Debug.LogError($"Failed to load asset of type {typeof(T)} with key: {key}");
        }
        Addressables.Release(handle);
        return asset;
    }
    
    public static void ReleaseAsset<T>(AsyncOperationHandle<T> handle) where T : UnityEngine.Object
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
        }
    }
}