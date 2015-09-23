(function () {
    'use strict';

    angular
        .module('RemoteControlApp')
        .controller('ActionCenterController', ActionCenterController);

    ActionCenterController.$inject = ['$log', 'robotService', '$scope', '$rootScope'];

    function ActionCenterController($log, robotService, $scope, $rootScope) {
        var model = this;

        model.state = {
            locked: false,
            messages: []
        };

        model.api = robotService.commands;

        activate();

        function activate() {
            // Set up eventlisteners
            registerEventHandler(robotService.events.robotLocked, robotLockedHandler);
            registerEventHandler(robotService.events.robotUnlocked, robotUnlockedHandler);
            registerEventHandler(robotService.events.robotMessage, robotMessageHandler);

            function registerEventHandler(event, handler) {
                var listener = $rootScope.$on(event, handler);
                $scope.$on('$destroy', listener);
            }

            // Eventhandlers
            function robotLockedHandler() {
                model.state.locked = true;
                $scope.$apply();
            }

            function robotUnlockedHandler() {
                model.state.locked = false;
                $scope.$apply();
            }

            function robotMessageHandler(event, message) {
                model.state.messages.unshift(message);
                $scope.$apply();
            }
        }
    }
})();