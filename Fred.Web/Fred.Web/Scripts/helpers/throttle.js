
/*
 * Cette fonction permet de faire un 'throttling'.
 * C-a-d on exectute la fonction que si elle n'a pas déja était appelé dans un intervale de temps.
 * Exemple : throttle(function () { onResize() }, 500, this);
 * La fonction onResize ne sera appelée que s'il elle n'a pas deja était appelée 500 ms avant.
 */

function throttle(fn, threshhold, scope) {
  threshhold || (threshhold = 250);
  var last;
  var deferTimer;
  return function () {
    var context = scope || this;

    var now = +new Date,
        args = arguments;
    if (last && now < last + threshhold) {
      // hold on to it
      clearTimeout(deferTimer);
      deferTimer = setTimeout(function () {
        last = now;
        fn.apply(context, args);
      }, threshhold);
    } else {
      last = now;
      fn.apply(context, args);
    }
  };
}


