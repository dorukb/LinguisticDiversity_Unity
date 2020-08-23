using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
/// <summary>
/// Class with a JS Plugin functions for WebGL.
/// </summary>
public static class MicrophoneWeb
{
    // Importing "CallFunction"
    [DllImport("__Internal")]
    public static extern void CallFunction();
    
    // Importing "GetTextValue"
    [DllImport("__Internal")]
    public static extern string GetTextValue();

    // Importing "GetNumberValue"
    [DllImport("__Internal")]
    public static extern float[] StartRecording(string sessionId, string recordingName);

    [DllImport("__Internal")]
    public static extern void EndRecording();
}