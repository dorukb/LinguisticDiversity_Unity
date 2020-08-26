using System.Runtime.InteropServices;

public static class MicrophoneWeb
{
    [DllImport("__Internal")]
    public static extern float[] StartRecording(string sessionId, string recordingName);

    [DllImport("__Internal")]
    public static extern void EndRecording();
}