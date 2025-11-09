using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Juegazo.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NVorbis;

namespace Juegazo
{
    public class AudioImoporter
    {
        private string sfxDirectory = string.Empty;
        public Dictionary<string, Dictionary<string, SoundEffectInstance>> SoundEffectsByFolderName = new();
        //this should not be here
        private List<Thread> threads = new();
        Dictionary<string, Block> blocks = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => { return a.GetTypes(); })
                .Where(t => t.IsSubclassOf(typeof(Block)) && !t.IsAbstract)
                .Select(t => (Block)Activator.CreateInstance(t))
                .ToDictionary(inst => inst.GetType().Name, inst => inst);
        public AudioImoporter(string SFXDirectory)
        {
            SFXDirectory = GetExecutingDir(SFXDirectory);
            if(!Directory.Exists(SFXDirectory))
            {
                throw new DirectoryNotFoundException($"directory: {SFXDirectory} does not exist, ur stupid");
            }
            sfxDirectory = SFXDirectory;
            GetAllSFX();
        }
        private void GetAllSFX()
        {
            foreach(string block in blocks.Keys)
            {
                var dirPath = Path.Combine(sfxDirectory, block);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                    Console.WriteLine($"created {block} directory");
                    continue;
                }
                Dictionary<string, SoundEffectInstance> soundEffectByName = new();

                foreach (var blockSFX in Directory.GetFiles(dirPath, "*.ogg"))
                {
                    LoadSFX(blockSFX, soundEffectByName);
                }
                SoundEffectsByFolderName.Add(Path.GetFileName(dirPath), soundEffectByName);
            }
        }

        private static void LoadSFX(string blockSFX, Dictionary<string, SoundEffectInstance> soundEffectByName)
        {
            SoundEffect sfx;
            using (var reader = File.OpenRead(blockSFX))
            {
                sfx = LoadSoundEffectFromOggStream(reader);
            }
            lock (soundEffectByName)
            {
                soundEffectByName.Add(Path.GetFileNameWithoutExtension(blockSFX), sfx.CreateInstance());
            }
        }

        private static string GetExecutingDir(string v)
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var dirInfo = new DirectoryInfo(baseDirectory);
                for (int i = 0; i < 3 && dirInfo.Parent != null; i++)
                {
                    dirInfo = dirInfo.Parent;
                }
                baseDirectory = dirInfo.FullName;
                return Path.Combine(baseDirectory, v);
            }
        private static SoundEffect LoadSoundEffectFromOggStream(Stream oggStream)
        {
            using var vorbis = new VorbisReader(oggStream, false);

            int sampleRate = vorbis.SampleRate;
            int channels = vorbis.Channels;
            List<byte> pcmData = new List<byte>();

            float[] floatBuffer = new float[4096];
            byte[] byteBuffer = new byte[floatBuffer.Length * 2]; // 16-bit PCM

            int samplesRead;
            while ((samplesRead = vorbis.ReadSamples(floatBuffer, 0, floatBuffer.Length)) > 0)
            {
                for (int i = 0; i < samplesRead; i++)
                {
                    short s = (short)(Math.Clamp(floatBuffer[i], -1f, 1f) * short.MaxValue);
                    byteBuffer[i * 2] = (byte)(s & 0xFF);
                    byteBuffer[i * 2 + 1] = (byte)((s >> 8) & 0xFF);
                }

                pcmData.AddRange(byteBuffer.AsSpan(0, samplesRead * 2).ToArray());
            }

            return new SoundEffect(pcmData.ToArray(), sampleRate, (AudioChannels)channels);
        }
        // this should not be here
        public void allSFXToBlocks(List<Block> blockss)
        {
            foreach(var b in blockss)
            {
                if(SoundEffectsByFolderName.TryGetValue(b.GetType().Name, out var thing))
                {
                    b.soundEffectsByName = thing;
                    b.loadedAudio = true;
                }
            }
        }
    }
}