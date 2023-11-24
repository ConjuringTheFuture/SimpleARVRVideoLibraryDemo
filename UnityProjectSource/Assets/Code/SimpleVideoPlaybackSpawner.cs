using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************************************************************
 * Currently this is super simple and treats all thumbnails as 
 * if they are the same button. 
 * 
 * TODO: Add a way to launch and mange multiple videos at the 
 * same time and track which thumbnails from the browser were
 * selected.
 *************************************************************/
public class SimpleVideoPlaybackSpawner : MonoBehaviour
{
    public GameObject VideoPlayerPrefab;
    GameObject videoPlayerObject;
    public float videoPlaybackDuration;

    public void SpawnVideoPlayer()
    {
        if (videoPlayerObject == null)
        {
            videoPlayerObject = Instantiate(VideoPlayerPrefab,
            VideoPlayerPrefab.transform.position,
            VideoPlayerPrefab.transform.rotation);
            videoPlayerObject.SetActive(true);
        }
        StartCoroutine(DespawnVideoPlayerAfter(videoPlaybackDuration));
    }

    public IEnumerator DespawnVideoPlayerAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DespawnVideoPlayer();
    }

    public void DespawnVideoPlayer()
    {
        if (videoPlayerObject)
            Destroy(videoPlayerObject);
    }
    
    public void UserVideoSelectionMade(bool play)
    {
        if (play)
            SpawnVideoPlayer();
        else
            DespawnVideoPlayer();
    }
}
