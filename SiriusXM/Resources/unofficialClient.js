window.unofficialClient  = {
    playPause: function() {
        document.getElementsByClassName('play-pause-btn')[0].click(); 
    },
    
    skipForward: function() {
        document.getElementsByClassName('skip-forward-btn')[0].click();
    },
    
    skipBack: function() {
        document.getElementsByClassName('skip-back-btn')[0].click();
    }
};