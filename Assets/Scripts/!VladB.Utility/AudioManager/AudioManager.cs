using System.Collections.Generic;
using UnityEngine;

namespace VladB.Utility
{
    public class AudioManager : MonoBehaviour
    {
        private static Transform _folder;

        private static Transform Folder
        {
            //TODO Не проверялось!
            get
            {
                if (_folder == null)
                {
                    GameObject go = GameObject.Find("AudioManagerFolder");
                    if (go == null)
                    {
                        go = new GameObject("AudioManagerFolder");
                    }

                    _folder = go.transform;
                }

                return _folder;
            }
        }

        private static Dictionary<string, AudioSource> _dictionary = new();

        private static AudioSource _tempSource;
        private static GameObject _tempObj;
        private static GameObject _tempPrefab;

        public static AudioSource GetAudioSource(string soundPath)
        {
            if (_dictionary.TryGetValue(soundPath, out _tempSource))
            {
                return _tempSource;
            }
            else
            {
                _tempPrefab = Resources.Load<GameObject>(soundPath);
                if (_tempPrefab == null)
                {
                    return null;
                }

                _tempObj = Instantiate(_tempPrefab, Folder);
                _tempSource = _tempObj.GetComponent<AudioSource>();
                _dictionary.Add(soundPath, _tempSource);
                return _tempSource;
            }
        }
    }
}