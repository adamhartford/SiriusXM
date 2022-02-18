window.unofficialClient  = {
    playPause: function() {
        this._private.clickButton('play-pause-btn');
    },
    
    skipForward: function() {
        this._private.clickButton('skip-forward-btn');
    },
    
    skipBack: function() {
        this._private.clickButton('skip-back-btn');
    },
    
    _private: {
        clickButton: function(classNames) {
            var btns = document.getElementsByClassName(classNames);
            for (var i in btns) btns[i].click();
        }
    }
};