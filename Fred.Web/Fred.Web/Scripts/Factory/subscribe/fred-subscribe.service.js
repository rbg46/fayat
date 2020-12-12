/*
 * Ce service sert a subscrire a un evenement. 
 * Se service est utile par exemple pour la communication entre components.
 * 
 * on souscrit a un evenement : 
 * fredSubscribeService.subscribe({ eventName: 'goBack', callback: actionReturnToCommandeList });
 * ici 'actionReturnToCommandeList' est l'action qui sera executée quand l'evenement 'goBack' quand sera soulevée.
 * 
 * on souleve un evenement  :
 *   fredSubscribeService.raiseEvent('goback',payload);
 * => payload => passage de parametres dans la function callback
 * 
 * on desouscrit a un evenement (dans un component dans le $onDestroy par exemple):
 * fredSubscribeService.unsubscribe({ eventName: 'goBack', callback: actionReturnToCommandeList });
 * 
 * on determine s'il a au moins une souscription :
 * fredSubscribeService.hasSubscriberFor('goBack');
 */
(function () {
  'use strict';

  angular
      .module('Fred')
      .service('fredSubscribeService', fredSubscribeService);

  function fredSubscribeService() {
    // Notion a connaitre : 
    // EventSubscriber => {eventName:'goback',subscribers:[{callback:function}]
    // eventSubscribers => tableau de EventSubscriber
    // subscriber => {eventName:'goback',callback:function}
    var eventSubscribers = [];

    var service = {
      subscribe: subscribe,
      unsubscribe: unsubscribe,
      raiseEvent: raiseEvent,
      hasSubscriberFor: hasSubscriberFor,
      getTooltip: getTooltip
    };

    return service;

    /*
     * Subscription a un evenement
     */
    //subscriber => {eventName:'goback',callback:function}
    function subscribe(subscriber) {
      var eventSubscriber = getEventSubscriberByEventName(subscriber.eventName);
      if (eventSubscriber === null) {
        addEventSubscriber(subscriber);
      }
      else {
        updateEventSubscriber(eventSubscriber, subscriber);
      }
    }

    /*
     * Retourne un EventSubscriber avec le nom de l'evenement
     */
    function getEventSubscriberByEventName(eventName) {
      var result = null;
      for (var i = 0; i < eventSubscribers.length; i++) {
        var eventNameOfEventSubscriber = eventSubscribers[i].eventName;
        if (eventNameOfEventSubscriber === eventName) {
          result = eventSubscribers[i];
        }
      }
      return result;
    }

    /*
    * Ajoute un EventSubscriber s'il n'en existe pas 
    */
    function addEventSubscriber(subscriber) {
      eventSubscribers.push({
        eventName: subscriber.eventName,
        tooltip: subscriber.tooltip,
        subscribers: [
          {
            callback: subscriber.callback
          }
        ]
      });
    }

    /*
    * Rajoute un callback a un evenement. S'il existe deja on ne le rajoute pas.
    */
    function updateEventSubscriber(eventSubscriber, subscriber) {
      if (canUpdateEventSubscriber(eventSubscriber, subscriber)) {
        eventSubscriber.subscribers.push({
          callback: subscriber.callback
        });
      }
    }

    /*
    * verifie si on peux ajouter un callback a un EventSubscriber.  
    */
    function canUpdateEventSubscriber(eventSubscriber, subscriber) {
      var result = true;
      for (var i = 0; i < eventSubscriber.subscribers.length; i++) {
        var internalSubscriber = eventSubscriber.subscribers[i];
        if (internalSubscriber.callback === subscriber.callback) {
          result = false;
        }
      }
      return result;
    }

    /*
     *  On desouscrit a un evenement.
     */
    //unsubscriber => {eventName:'goback',callback:function}
    function unsubscribe(unsubscriber) {
      var eventSubscriber = getEventSubscriberByEventName(unsubscriber.eventName);
      if (eventSubscriber !== null) {
        var callbacks = getCallbacksWithSameReference(eventSubscriber, unsubscriber.callback);
        removeCallBacksOfEventSubscriber(eventSubscriber, callbacks);
        removeEventSubscriberIfNecessary(eventSubscriber);
      }
    }

    /*
     * Retourne les callbacks ayant la meme reference que celle passée en parametre
     */
    function getCallbacksWithSameReference(eventSubscriber, callback) {
      var callbacks = [];
      for (var i = 0; i < eventSubscriber.subscribers.length; i++) {
        var internalSubscriber = eventSubscriber.subscribers[i];
        if (internalSubscriber.callback === callback) {
          callbacks.push(internalSubscriber);
        }
      }
      return callbacks;
    }

    /*
     * Supprime les callback de la liste de l'EventSubscriber.
     */
    function removeCallBacksOfEventSubscriber(eventSubscriber, callbacks) {
      for (var i = 0; i < callbacks.length; i++) {
        var index = eventSubscriber.subscribers.indexOf(callbacks[i]);
        if (index !== -1) {
          eventSubscriber.subscribers.splice(index, 1);
        }
      }
    }

    /*
     * Supprime l'EventSubscriber s'il n'y a plus de callbacks associés
     */
    function removeEventSubscriberIfNecessary(eventSubscriber) {
      if (eventSubscriber.subscribers.length === 0) {
        var indexEventSub = eventSubscribers.indexOf(eventSubscriber);
        if (indexEventSub !== -1) {
          eventSubscribers.splice(indexEventSub, 1);
        }
      }
    }

    /*
     *  on souleve un evenement 
     */
    function raiseEvent(eventName, payload) {
      var eventSubscriber = getEventSubscriberByEventName(eventName);
      if (eventSubscriber !== null) {
        executeCallBack(eventSubscriber, payload);
      }
    }

    /*
     * Execution des callbacks
     */
    function executeCallBack(eventSubscriber, payload) {
      for (var i = 0; i < eventSubscriber.subscribers.length; i++) {
        var callBack = eventSubscriber.subscribers[i].callback;
        callBack(payload);
      }
    }

    /*
     *  on determine s'il a au moins une souscription.
     */
    function hasSubscriberFor(eventName) {
      var result = false;
      var eventSubscriber = getEventSubscriberByEventName(eventName);
      if (eventSubscriber !== null) {
        result = true;
      }
      return result;
    }

    /**
     * Récupération du tooltip
     */
    function getTooltip(eventName) {
      var eventSubscriber = getEventSubscriberByEventName(eventName);
      return eventSubscriber.tooltip;
    }
  }
})();