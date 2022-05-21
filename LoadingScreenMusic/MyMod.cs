using System;
using MelonLoader;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using NAudio;
using NAudio.Wave;

namespace LoadingScreenMusic
{
    public class MyMod : MelonMod
    {
        static bool NoAudio = false;
        public override void OnApplicationStart()
        {
            if (!Directory.Exists("LoadingScreenMusic"))
            {
                Directory.CreateDirectory("LoadingScreenMusic");
            }
            ChangeTitle.ChangeIcon();
            ChangeTitle.SetNormal();
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex == 1)
            {
                MelonCoroutines.Start(LoadingScreen());
            }
            if (buildIndex == 2)
            {
                MelonCoroutines.Start(ChangeLoadingScreen());
            }
        }

        public override void OnUpdate()
        {
            ChangeTitle.SetNormal();
        }
        static System.Random random = new System.Random();
        static string RandomMusicFile()
        {
            string[] musicfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/LoadingScreenMusic");
            foreach (string musicfile in musicfiles)
            {
                if (!musicfile.EndsWith(".wav") && !musicfile.EndsWith(".ogg"))
                {
                    if (musicfile.EndsWith(".mp3"))
                    {

                    } else
                    if (musicfile.EndsWith(".aiff"))
                    {
                        
                    } else
                    {

                    }
                }
            }
            musicfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/LoadingScreenMusic");


            bool filesdeleted = false;
           
            //What idiot uses Goto? We are not in Assembler! Fucking Noob
            /*
            if (filesdeleted)
            {
                goto RandomMusicFile; 
            }
            */
            if (musicfiles.Length == 0)
            {
                // How to make ur app not crash :) 
                NoAudio = true; 
                //throw new Exception("No Audio file Found!");  Exception = Bad 
            } else
            {
                return musicfiles[0];
            }
         
            int index = random.Next(musicfiles.Length);
            return musicfiles[index - 1];
        }
        IEnumerator ChangeLoadingScreen()
        {
            if (NoAudio) { }
            else
            {
                string file = RandomMusicFile();
                UnityWebRequest www = UnityWebRequest.Get("file://" + file);
                www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }
                AudioClip audioClip = WebRequestWWW.InternalCreateAudioClipUsingDH(www.downloadHandler, www.url, false, false, AudioType.UNKNOWN);
                while (!www.isDone || audioClip.loadState == AudioDataLoadState.Loading)
                {
                    yield return null;
                }
                if (audioClip != null)
                {
                    if (loadingscreenAudio != null)
                    {
                        loadingscreenAudio.clip = audioClip;
                        loadingscreenAudio.Play();
                        ChangeTitle.CurrentMusik(file);
                    }
                }
            }
        }
        static AudioSource loadingscreenAudio;
        IEnumerator LoadingScreen()
        {
            GameObject authentication = GameObject.Find("LoadingBackground_TealGradient_Music/LoadingSound");
            GameObject loadingscreen = GameObject.Find("MenuContent/Popups/LoadingPopup/LoadingSound");
            if (authentication != null)
            {
                authentication.GetComponent<AudioSource>().Stop();
            }
            if (loadingscreen != null)
            {
                loadingscreenAudio = loadingscreen.GetComponent<AudioSource>();
                loadingscreenAudio.Stop();
            }
            string aud = RandomMusicFile();
            UnityWebRequest www = UnityWebRequest.Get("file://"+ aud);
            www.SendWebRequest();
            while (!www.isDone)
            {
                yield return null;
            }
            AudioClip audioClip = WebRequestWWW.InternalCreateAudioClipUsingDH(www.downloadHandler, www.url, false, false, AudioType.UNKNOWN);
            while (!www.isDone || audioClip.loadState == AudioDataLoadState.Loading)
            {
                yield return null;
            }
            if (audioClip != null)
            {
                if (authentication != null)
                {
                    authentication.GetComponent<AudioSource>().clip = audioClip;
                    authentication.GetComponent<AudioSource>().Play();
                }
                if (loadingscreenAudio != null)
                {
                    loadingscreenAudio.clip = audioClip;
                    loadingscreenAudio.Play();
                    ChangeTitle.CurrentMusik(aud);
                }
            }
        }

        static void AIFFConverter(string aiffFile)
        {
            string wav_name = aiffFile.Replace(".aiff", ".wav"); //Create a new name for the Output
            try
            {
                using (AiffFileReader reader = new AiffFileReader(aiffFile))
                {
                    using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                    {
                        WaveFileWriter.CreateWaveFile(wav_name, pcmStream);
                    }
                } // Dispose affirerader around the file from the memory so that the garbage collector does not have to take care of it
            } catch
            {
                MelonLogger.Error($"Error by Converting affi file {aiffFile}");
            } finally
            {
                File.Delete(aiffFile);
            }
        }

        static void MP3Converter(string mp3File)
        {
            string wav_name = mp3File.Replace(".mp3", ".wav"); //Create a new name for the Output
            try
            {
                using (AiffFileReader reader = new AiffFileReader(mp3File))
                {
                    using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                    {
                        WaveFileWriter.CreateWaveFile(wav_name, pcmStream);
                    }
                } // Dispose affirerader around the file from the memory so that the garbage collector does not have to take care of it
            }
            catch
            {
                MelonLogger.Error($"Error by Converting MP3 file {mp3File}");
            }
            finally
            {
                File.Delete(mp3File);
            }
        }

        static void AnyTypeConverter(string AnyFile)
        {
            string end;
            string wav_name;
            try
            {
                end = Path.GetExtension(AnyFile);
            }
            catch
            {
                return;
            } 
            //Create a new name for the Output
            wav_name = AnyFile.Replace(end, ".wav");
            try
            {
                using (MediaFoundationReader reader = new MediaFoundationReader(AnyFile)) 
                {
                    using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                    {
                        WaveFileWriter.CreateWaveFile(wav_name, pcmStream);
                    }
                } // Dispose affirerader around the file from the memory so that the garbage collector does not have to take care of it
            } catch
            {
                MelonLogger.Error($"Error by Converting {AnyFile}");  
            } finally
            {
                File.Delete(AnyFile);
            }
            
        }

    }
}





