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

                // Set up client eventlisteners
                proxy.client.robotLocked(function () {
                    $rootScope.$emit(service.events.robotLocked);
                });
                proxy.client.robotUnlocked(function () {
                    $rootScope.$emit(service.events.robotUnlocked);
                });
                proxy.client.robotMessage(function () {
                    $rootScope.$emit(service.events.robotMessage);
                });

                // Start connection
                $.connection.hub.start()
                    .done(function () {
                        $log.info('Now connected, connection ID=' + $.connection.hub.id);

                        // Define commands
                        service.commands.lockRobot = function () {
                            proxy.server.lockRobot();
                        };

                        service.commands.unlockRobot = function () {
                            proxy.server.unlockRobot();
                        };
                    })
                    .fail(function () {
                        $log.error('Could not connect to server.');
                    });
            } else {
                alert('Scan the QR-code on ur ROBOT');
                $log.error('No robot connection info.');
            }
        };
    }
})();