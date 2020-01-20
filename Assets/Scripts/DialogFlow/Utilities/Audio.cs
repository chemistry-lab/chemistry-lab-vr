using System.Collections.Generic;
using System;

using UnityEngine;

namespace DialogFlow.Utilities
{
    public static class Audio
    {
        public static AudioClip ToAudioClip(byte[] file)
        {
            if (IsCompatible(file))
            {
                int subChunk1Size = BitConverter.ToInt32(file, 16);
                int audioFormat = BitConverter.ToInt16(file, 20);
                int numberOfChannels = BitConverter.ToInt16(file, 22);
                int sampleRate = BitConverter.ToInt32(file, 24);
                int bitsPerSample = BitConverter.ToInt16(file, 34);

                int dataIndex = 20 + subChunk1Size;
                for (int i = dataIndex; i < file.Length; i++)
                {
                    if (file[i] == 'd' && file[i + 1] == 'a' && file[i + 2] == 't' && file[i + 3] == 'a')
                    {
                        dataIndex = i + 4;
                        break;
                    }
                }

                int subChunk2Size = BitConverter.ToInt32(file, dataIndex);
                dataIndex += 4;
                int sampleSize = bitsPerSample / 8;
                int sampleCount = subChunk2Size / sampleSize;

                if (audioFormat == 1)
                {
                    float[] audioBuffer = new float[sampleCount];
                    for (int i = 0; i < sampleCount; i++)
                    {
                        int sampleIndex = dataIndex + i * sampleSize;
                        short intSample = BitConverter.ToInt16(file, sampleIndex);
                        float sample = intSample / 32768.0f;
                        audioBuffer[i] = sample;
                    }

                    AudioClip audioClip = AudioClip.Create(Guid.NewGuid().ToString(), sampleCount, numberOfChannels, sampleRate, false);
                    audioClip.SetData(audioBuffer, 0);

                    return audioClip;
                }
                return null;
            }
            return null;
        }

        public static byte[] FromAudioClip(AudioClip clip)
        {
            float[] samples = new float[clip.samples];
            clip.GetData(samples, 0);

            List<byte> file = new List<byte>();
            file.AddRange(new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' });
            file.AddRange(BitConverter.GetBytes(samples.Length * 2 + 44 - 8));
            file.AddRange(new byte[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E' });
            file.AddRange(new byte[] { (byte)'f', (byte)'m', (byte)'t', (byte)' ' });
            file.AddRange(BitConverter.GetBytes(16));
            file.AddRange(BitConverter.GetBytes((ushort)1));
            file.AddRange(BitConverter.GetBytes((ushort)clip.channels));
            file.AddRange(BitConverter.GetBytes(clip.frequency));
            file.AddRange(BitConverter.GetBytes(clip.frequency * clip.channels * 2));
            file.AddRange(BitConverter.GetBytes((ushort)(clip.channels * 2)));
            file.AddRange(BitConverter.GetBytes((ushort)16));
            file.AddRange(new byte[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' });
            file.AddRange(BitConverter.GetBytes(samples.Length * 2));

            for (int i = 0; i < samples.Length; i++)
            {
                short sample = (short)(samples[i] * 32768.0f);
                file.AddRange(BitConverter.GetBytes(sample));
            }

            return file.ToArray();
        }

        public static AudioClip EmptyClip()
        {
            return null;
        }

        private static bool IsCompatible(byte[] file)
        {
            byte[] data = new byte[4];
            Buffer.BlockCopy(file, 0, data, 0, data.Length);
            string chunkId = ByteArrayToString(data);
            Buffer.BlockCopy(file, 8, data, 0, data.Length);
            string format = ByteArrayToString(data);

            return (chunkId == "RIFF" && format == "WAVE");
        }

        private static string ByteArrayToString(byte[] content)
        {
            char[] chars = new char[content.Length];
            content.CopyTo(chars, 0);
            return new string(chars);
        }
    }
}
