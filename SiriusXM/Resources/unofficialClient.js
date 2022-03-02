window.unofficialClient  = {
    playPause: function() {
        this._.clickButton('play-pause-btn');
    },
    
    skipForward: function() {
        this._.clickButton('skip-forward-btn');
    },
    
    skipBack: function() {
        this._.clickButton('skip-back-btn');
    },
    
    getNowPlaying: function() {
        const state = {
            artistName: this._.getText('artist-name'),
            trackName: this._.getText('track-name')
        };
        
        console.log(JSON.stringify(state));
        return state;
    },

    postMessage: function (handlerName, msg) {
        var handler = window.webkit.messageHandlers[handlerName];
        handler.postMessage(JSON.stringify(msg));
    },
    
    _: {
        clickButton: function(classNames) {
            const btns = document.getElementsByClassName(classNames);
            for (let i in btns) btns[i].click();
        },
        
        getText: function(className) {
            const el = document.getElementsByClassName(className);
            return el.length ? el[0].textContent : null;
        },
        
        sendNowPlaying: function() {
            const state = unofficialClient.getNowPlaying();
            if (state.trackName) unofficialClient.postMessage('playerState', state);
        },
        
        init: function() {
            const el = document.getElementsByTagName('program-descriptive-text');
            if (el.length) {
                const observer = new MutationObserver( list => {
                    unofficialClient._.sendNowPlaying();
                });

                observer.observe(el[0], { characterData: true, attributes: false, childList: false, subtree: true });
                unofficialClient._.sendNowPlaying();
            }
        }
    }
};

(function() {
    unofficialClient._.init();
})();

//#sourceURL=unofficialClient.js