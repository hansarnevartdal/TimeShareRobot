(function () {
    'use strict';

    angular
        .module('RemoteControlApp')
        .controller('AdminController', AdminController);

    AdminController.$inject = ['$log', 'robotService', '$scope', '$rootScope'];

    function AdminController($log, robotService, $scope, $rootScope) {
        var model = this;

        model.state = {
            locked: false,
        };

        model.api = robotService.commands;

        activate();

        function activate() {
            // Set up eventlisteners
            var robotLockedListener = $rootScope.$on(robotService.events.robotLocked, robotLockedHandler);
            var robotUnlockedListener = $rootScope.$on(robotService.events.robotUnlocked, robotUnlockedHandler);

            $scope.$on('$destroy', robotLockedListener);
            $scope.$on('$destroy', robotUnlockedListener);

            // Eventhandlers
            function robotLockedHandler() {
                model.locked = true;
            }

            function robotUnlockedHandler() {
                model.locked = false;
            }
        }
    }
})();