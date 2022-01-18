'use strict';

Object.defineProperty(exports, '__esModule', {
    value: true
});
var video = document.getElementById('video');
var canvas = document.getElementById('canvas');
var errorMsgElement = document.querySelector('span#errorMsg');
var stream;

var constraints = {
    audio: false,
    video: {
        width: 480, height: 480, facingMode: "user"
    }
};

function startCamera() {
    return regeneratorRuntime.async(function startCamera$(context$1$0) {
        while (1) switch (context$1$0.prev = context$1$0.next) {
            case 0:
                context$1$0.prev = 0;
                context$1$0.next = 3;
                return regeneratorRuntime.awrap(navigator.mediaDevices.getUserMedia(constraints));

            case 3:
                stream = context$1$0.sent;

                video.srcObject = stream;
                video.play();
                context$1$0.next = 11;
                break;

            case 8:
                context$1$0.prev = 8;
                context$1$0.t0 = context$1$0['catch'](0);

                errorMsgElement.innerHTML = 'navigator.getUserMedia error:' + context$1$0.t0.toString();

            case 11:
            case 'end':
                return context$1$0.stop();
        }
    }, null, this, [[0, 8]]);
}

function captureImage() {
    var context = canvas.getContext('2d');
    context.drawImage(video, 0, 0, 480, 480);
}

function stopCamera() {
    stream.getTracks().forEach(function (track) {
        return track.stop();
    });
}

exports.startCamera = startCamera;
exports.captureImage = captureImage;
exports.stopCamera = stopCamera;

