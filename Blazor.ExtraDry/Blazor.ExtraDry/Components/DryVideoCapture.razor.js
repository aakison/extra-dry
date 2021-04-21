'use strict';

const video = document.getElementById('video');
const canvas = document.getElementById('canvas');
const errorMsgElement = document.querySelector('span#errorMsg');
var stream;

const constraints = {
    audio: false,
    video: {
        width: 480, height: 480, facingMode: "user"
    }
};

async function startCamera() {
    try {
        stream = await navigator.mediaDevices.getUserMedia(constraints);
        video.srcObject = stream;
        video.play();
    } catch (e) {
        errorMsgElement.innerHTML = `navigator.getUserMedia error:${e.toString()}`;
    }
}

function captureImage() {
    var context = canvas.getContext('2d');
    context.drawImage(video, 0, 0, 480, 480);
}

function stopCamera() {
    stream.getTracks().forEach(track => track.stop());
}

export { startCamera, captureImage, stopCamera };
