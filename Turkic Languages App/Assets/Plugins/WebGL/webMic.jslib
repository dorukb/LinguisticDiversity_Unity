// Creating functions for the Unity

var LibraryMyPlugin = {

$MyData: {
    recordedChunks: [],
    id: "default",
    recordingName: "recording",
    isRecording: false,
   },

StartRecording: function(sessionId, recordingName)
{
    MyData.id = Pointer_stringify(sessionId);
    MyData.recordingName = Pointer_stringify(recordingName);
    
    const handleSuccess = function(stream) 
    {
      console.log(sessionId);
      MyData.isRecording = true;
      MyData.recordedChunks= [];
      
      const options = {mimeType: 'audio/ogg'};
      this.mediaRecorder = new MediaRecorder(stream, options);

      this.mediaRecorder.addEventListener('dataavailable', function(e) {
          MyData.recordedChunks.push(e.data);
          unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'pushing audio data.');
      });

      this.mediaRecorder.addEventListener('stop', function() {
        MyData.isRecording = false;
          unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'stop event fired.');
          this.blob = new Blob(MyData.recordedChunks, { 'type': 'audio/ogg' });

            unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'posting to server side.');

            var reader = new FileReader();
                // this function is triggered once a call to readAsDataURL returns
                reader.onload = function(event){
                    const fd = new FormData();
                    fd.append('fname', MyData.recordingName);
                    fd.append('data', event.target.result);
                    fd.append('id', MyData.id);

                const xhr = new XMLHttpRequest();
                xhr.onload=function(e) {
                    if(this.readyState === 4) {
                        console.log("Server returned: ",e.target.responseText);
                    }
                };
                console.log(this.blob);
                console.log(MyData.id);
                xhr.open("POST","http://localhost/turkicLanguages/upload.php",true);
                xhr.send(fd);
                };      
                // trigger the read from the reader...
                reader.readAsDataURL(this.blob);
      });
      
      this.mediaRecorder.start();
      unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'started recording');
    };

    navigator.mediaDevices.getUserMedia({ audio: true, video: false })
          .then(handleSuccess);
},

EndRecording: function()
{
    if(this.mediaRecorder != null && MyData.isRecording){
      this.mediaRecorder.stop();
      unityInstance.SendMessage('JavaScriptHook', 'PrintMessage', 'EndRecording called.');
    }
},

};
autoAddDeps(LibraryMyPlugin, '$MyData');
mergeInto(LibraryManager.library, LibraryMyPlugin);