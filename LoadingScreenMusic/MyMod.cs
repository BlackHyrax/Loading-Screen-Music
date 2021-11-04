using System;
using MelonLoader;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

namespace LoadingScreenMusic
{
    public class MyMod : MelonMod
    {
        private AudioClip audiofile;
        private AudioSource audiosource;
        private AudioSource audiosource2;

        public override void OnApplicationStart()
        {
            Directory.CreateDirectory("LoadingScreenMusic");
            MelonCoroutines.Start(WebRequest());
        }
        
        IEnumerator WebRequest()
        {
            // I used some code of Knah´s join notifier for the unitywebrequest.

            MelonLogger.Msg("Loading custom loading screen music");
            var uwr = UnityWebRequest.Get($"file://{Path.Combine(Environment.CurrentDirectory, "LoadingScreenMusic/Music.ogg")}");
            uwr.SendWebRequest();

            while (!uwr.isDone) yield return null;

            audiofile = WebRequestWWW.InternalCreateAudioClipUsingDH(uwr.downloadHandler, uwr.url, false, false, AudioType.UNKNOWN);
            audiofile.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            while (audiosource == null)
            {
                audiosource = GameObject.Find("LoadingBackground_TealGradient_Music/LoadingSound")?.GetComponent<AudioSource>();

                yield return null;
            }
            audiosource.clip = audiofile;
            audiosource.Stop();
            audiosource.Play();

            while (audiosource2 == null)
            {
                audiosource2 = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/LoadingSound")?.GetComponent<AudioSource>();

                yield return null;
            }
            audiosource2.clip = audiofile;
            audiosource2.Stop();
            audiosource2.Play();


        }



    }


}





