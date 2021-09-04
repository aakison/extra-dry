'use strict';

function initCamera() {
    var video, canvas, snap, errorMsgElement, constraints, stream, context;
    return regeneratorRuntime.async(function initCamera$(context$1$0) {
        while (1) switch (context$1$0.prev = context$1$0.next) {
            case 0:
                video = document.getElementById('video');
                canvas = document.getElementById('canvas');
                snap = document.getElementById("snap");
                errorMsgElement = document.querySelector('span#errorMsg');
                constraints = {
                    audio: false,
                    video: {
                        width: 480, height: 480
                    }
                };
                context$1$0.prev = 5;
                context$1$0.next = 8;
                return regeneratorRuntime.awrap(navigator.mediaDevices.getUserMedia(constraints));

            case 8:
                stream = context$1$0.sent;

                handleSuccess(stream);
                context$1$0.next = 15;
                break;

            case 12:
                context$1$0.prev = 12;
                context$1$0.t0 = context$1$0['catch'](5);

                errorMsgElement.innerHTML = 'navigator.getUserMedia error:' + context$1$0.t0.toString();

            case 15:
                context = canvas.getContext('2d');

                snap.addEventListener("click", function () {
                    context.drawImage(video, 0, 0, 480, 480);
                });

            case 17:
            case 'end':
                return context$1$0.stop();
        }
    }, null, this, [[5, 12]]);
}

function handleSuccess(stream) {
    window.stream = stream;
    video.srcObject = stream;
}

