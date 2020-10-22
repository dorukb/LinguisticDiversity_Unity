// Creating functions for the Unity

var LibraryMyPlugin = {

  $recording: {
      recordedChunks: [],
      id: "default",
      name: "recording",
      isRecording: false,
      mime: "",
      audioPlayback: null,
      postAddress: "https://coltekin.net/audio/upload.php",
    },

  StartRecording: function(sessionId, recordingName)
  {
      recording.id = Pointer_stringify(sessionId);
      recording.name = Pointer_stringify(recordingName);

      // 'webm' for chrome, 'ogg' for mozilla.
      var enc = ['ogg', 'webm'];
      var extension = "",
      mime = '';
      enc.forEach(function(e) 
        {
         !extension && (mime = "audio/" +  e  +";codecs=\"opus\"") && MediaRecorder.isTypeSupported(mime) && (extension = e)
        }
      );
      recording.mime = mime;
      console.log(mime);

      const handleSuccess = function(stream) 
      {
        recording.isRecording = true;
        recording.recordedChunks= [];
        
        const options = {mimeType: mime};
        this.mediaRecorder = new MediaRecorder(stream, options);

        this.mediaRecorder.addEventListener('dataavailable', function(e) {
            recording.recordedChunks.push(e.data);
            unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'pushing audio data.');
            unityInstance.SendMessage('JavaScriptHook', 'DataReady');
        });

        this.mediaRecorder.addEventListener('stop', function() {
          recording.isRecording = false;
            unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'stop event fired.');
           
        });
        
        this.mediaRecorder.start();
        unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'started recording');
      };

      navigator.mediaDevices.getUserMedia({ audio: true, video: false })
            .then(handleSuccess);
  },

  EndRecording: function()
  {
      if(this.mediaRecorder != null && recording.isRecording){
        this.mediaRecorder.stop();
        unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'EndRecording called.');
      }
  },

  PlayCurrentRecording: function()
  {
    if(recording.recordedChunks == []) return;
    console.log("playing the current recording.");

    var tmpBlob = new Blob(recording.recordedChunks, { 'type': recording.mime });
    var url = URL.createObjectURL(tmpBlob);
    
    if(recording.audioPlayback == null){
      recording.audioPlayback = new Audio(url);
      recording.audioPlayback.controls = false;
      document.body.appendChild(recording.audioPlayback);
      recording.audioPlayback.oncanplaythrough = function() 
      {
        recording.audioPlayback.play();
      };
    }
    else{
      recording.audioPlayback.src = url;
      recording.audioPlayback.load();
    }
    
  },

  SaveCurrentRecording: function()
  {
    if(recording.isRecording || recording.recordedChunks == []) {console.log("No recording to save."); return;}

    this.blob = new Blob(recording.recordedChunks, { 'type': recording.mime });
    unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'posting to server side.');
    var reader = new FileReader();
    
    // this function is triggered once a call to readAsDataURL returns
    reader.onload = function(event)
    {
      const fd = new FormData();
      fd.append('fname', recording.name);
      fd.append('data', event.target.result);
      fd.append('id', recording.id);

      const xhr = new XMLHttpRequest();
      xhr.onload=function(e) 
      {
        if(this.readyState === 4) 
        {
          console.log("Server returned: ",e.target.responseText);
        }
      };
      
      xhr.open("POST",recording.postAddress,true);
      xhr.send(fd);
    };      
    reader.readAsDataURL(this.blob);
  },

};
autoAddDeps(LibraryMyPlugin, '$recording');
mergeInto(LibraryManager.library, LibraryMyPlugin);