using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;

public class SimpleVideoPlayer : MonoBehaviour
{
    static readonly string[] k_PlayFields = { "url" };

    async void Start()
    { 
        var metaData = await YoutubeDl.GetVideoMetaDataAsync<YoutubeVideoMetaData>("https://www.youtube.com/watch?v=1PuGuqpHQGo", YoutubeDlOptions.Default, k_PlayFields, CancellationToken.None);

        Debug.Log(metaData.Extension);
        foreach (var format in metaData.Formats)
        {
            Debug.Log(format);
        }
    }
}
