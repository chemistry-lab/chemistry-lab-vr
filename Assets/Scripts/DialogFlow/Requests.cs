using System;

namespace DialogFlow
{
    [Serializable]
    public class RequestBody
    {
        public OutputAudioConfig outputAudioConfig;
        public QueryInput queryInput;
        public string inputAudio;
    }

    [Serializable]
    public class QueryInput
    {
        public InputAudioConfig audioConfig;
    }

    [Serializable]
    public class InputAudioConfig
    {
        public string audioEncoding;
        public int sampleRateHertz;
        public string languageCode;
    }

    public class InputAudioEncoding
    {
        public const string Linear16 = "AUDIO_ENCODING_LINEAR_16";
        public const string Flac = "AUDIO_ENCODING_FLAC";
        public const string Mulaw = "AUDIO_ENCODING_MULAW";
        public const string Amr = "AUDIO_ENCODING_AMR";
        public const string AmrWb = "AUDIO_ENCODING_AMR_WB";
        public const string OggOpus = "AUDIO_ENCODING_OGG_OPUS";
        public const string SpeexWithHeaderByte = "AUDIO_ENCODING_SPEEX_WITH_HEADER_BYTE";
    }

    [Serializable]
    public class OutputAudioConfig
    {
        public string audioEncoding;
        public SynthesizeSpeechConfig synthesizeSpeechConfig;
    }

    [Serializable]
    public class SynthesizeSpeechConfig
    {
        public VoiceConfig voice;
    }

    [Serializable]
    public class VoiceConfig
    {
        public string ssmlGender;
    }

    public class SSMLGender
    {
        public const string Unspecified = "SSML_VOICE_GENDER_UNSPECIFIED";
        public const string Male = "SSML_VOICE_GENDER_MALE";
        public const string Female = "SSML_VOICE_GENDER_FEMALE";
        public const string Neutral = "SSML_VOICE_GENDER_NEUTRAL";
    }

    public class OutputAudioEncoding
    {
        public const string Linear16 = "OUTPUT_AUDIO_ENCODING_LINEAR_16";
        public const string Mp3 = "OUTPUT_AUDIO_ENCODING_MP3";
        public const string OggOpus = "OUTPUT_AUDIO_ENCODING_OGG_OPUS";
    }
}
