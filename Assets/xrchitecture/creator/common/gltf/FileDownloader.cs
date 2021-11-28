using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public static class FileDownloader
{

    public static string FilePath = $"{Application.persistentDataPath}/files/";

    public static async void DownloadFile(string url, Action<string> onSuccess, Action<float> onProgress = null, Action<string> onError = null)
    {   
        string result = await FileDownloader.GetFileRequest(url, (progress) => {
            if (onProgress != null) {
                onProgress(progress);
            }
        }, (error) => {
            if (onError != null) {
                onError(error);
            }
        });
        
        Debug.Log(result);
        onSuccess(result);
    }

    public static void DownloadFile(string url, bool skipCache, Action<string> onSuccess, Action<float> onProgress = null, Action<string> onError = null)
    {   
        string path = GetFilePath(url);
        if (!skipCache && File.Exists(path))
        {
            Debug.Log("Found file locally, loading..." + path);
            onSuccess(path);
            return;
        }
        DownloadFile(url, onSuccess, onProgress, onError);
    }

    public static string GetFilePath(string url)
    {
        try {
            Uri uri = new Uri(url);
            string fileName = System.IO.Path.GetFileName(uri.LocalPath);
            if (fileName != null && fileName != "") {
                return $"{FileDownloader.FilePath}{fileName}";
            }
        } catch {};

        Debug.LogWarning("Bad file url: " + url);
        return $"{FileDownloader.FilePath}temp";
    }
    
    private static async Task<string> GetFileRequest(string url, Action<float> onProgress = null, Action<string> onError = null)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            int fileSize = 0;
            req.downloadHandler = new DownloadHandlerFile(FileDownloader.GetFilePath(url));
            req.SendWebRequest();            

            if (req.isNetworkError || req.isHttpError)
            {
                if (onError != null) {
                    onError($"{req.error} : {req.downloadHandler.text}");
                }
            }

            while (!req.isDone)
            {
                if (fileSize == 0) {
                    Int32.TryParse(req.GetResponseHeader("Content-Length"), out fileSize);
                }
                if (onProgress != null) {
                    onProgress((fileSize != 0) ? (req.downloadedBytes / (float)fileSize) : req.downloadProgress);
                }
                await Task.Delay(100);
            }
            if (onProgress != null) {
                onProgress(1.0f);
            }
            Debug.Log(FileDownloader.GetFilePath(url));
            return FileDownloader.GetFilePath(url);
        }
    }
}