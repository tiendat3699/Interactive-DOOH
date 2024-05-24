using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace GadGame.SO
{
    [CreateAssetMenu]
    public class AudioConfig : Config
    {
        public AudioObject AudioObjectPrefab;
        
        [SerializeField]
        [FolderPath(RequireExistingPath = true, ParentFolder = "Assets")]
        private string _soundDefineFileSavePath;
        
        public Dictionary<string, AudioClip> Musics = new();
        public Dictionary<string, AudioClip> Sounds = new();

        #if UNITY_EDITOR
        
        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        private void GenerateAudioDefine()
        {
            var backgroundMusicBody = "";
            backgroundMusicBody = Musics.Aggregate(backgroundMusicBody, (current, music) => current + $"\t\t{music.Key},\n");
            var soundEffectsBody = "";
            soundEffectsBody = Sounds.Aggregate(soundEffectsBody, (current, music) => current + $"\t\t{music.Key},\n");


            var ns = _soundDefineFileSavePath.Replace('/', '.').Replace(".Scripts", "");
            
            var template = Utils.GetTemplate("AudioDefine", new Dictionary<string, string>
            {
                {"backgroundMusicBody", backgroundMusicBody},
                {"soundEffectsBody", soundEffectsBody},
                {"namespace", ns}
            });

            using (var sw = new StreamWriter(Path.Combine(Application.dataPath, _soundDefineFileSavePath,$"AudioDefine.cs")))
            {
                sw.Write (template);
            }

            AssetDatabase.Refresh (ImportAssetOptions.ForceUpdate);
        }
        #endif
    }
}