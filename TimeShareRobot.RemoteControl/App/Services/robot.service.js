(function () {
    angular
        .module('RemoteControlApp')
        .service('robotService', robotService);

    robotService.$inject = ['$log','$location','$rootScope'];

    function robotService($log, $location, $rootScope) {
        var service = this;
        service.connectedToRobot = undefined;

        service.commands = { };
            
        service.events = {
            robotLocked: "RobotService.Events.RobotLocked",
            robotUnlocked: "RobotService.Events.RobotUnlocked",
            robotMessage: "RobotService.Events.Message"
        }

        init();

        function init() {
            var connectionId = $location.search().connectionId;
            if (connectionId) {
                var proxy = $.connection.robotHub;

                // Set up client eventpublishers
                proxy.client.robotLocked = function (robotConnectionId) {
                    if (robotConnectionId == connectionId) {
                        $rootScope.$emit(service.events.robotLocked);
                    }
                };
                proxy.client.robotUnlocked = function (robotConnectionId) {
                    if (robotConnectionId == connectionId) {
                        $rootScope.$emit(service.events.robotUnlocked);
                    }
                };
                proxy.client.robotMessage = function (robotConnectionId, message) {
                    if (robotConnectionId == connectionId) {
                        $rootScope.$emit(service.events.robotMessage, message);
                    }
                };

                // Start connection
                $.connection.hub.url = "http://localhost:50278/signalr";
                $.connection.hub.start()
                    .done(function () {
                        $log.info('Now connected, connection ID=' + $.connection.hub.id);

                        // Define commands
                        service.commands.lockRobot = function () {
                            proxy.server.lockRobot(connectionId);
                        };

                        service.commands.unlockRobot = function () {
                            proxy.server.unlockRobot(connectionId);
                        };
                    })
                    .fail(function (err) {
                        $log.error('Could not connect to server.', err);
                    });
            } else {
                alert('Scan the QR-code on ur ROBOT');
                $log.error('No robot connection info.');
            }
        };
    }
})();