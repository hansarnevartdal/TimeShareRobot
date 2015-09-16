(function () {
    'use strict';

    angular
        .module('RemoteControlApp')
        .controller('ActionCenterController', ActionCenterController);

    ActionCenterController.$inject = ['$log', ];

    function ActionCenterController($log) {
        var model = this;

        model.robot = {
            locked: false,
            connnectionId: '',
            actions: [
                'KILL ALL HUMANS!!!',
                'BEND GIRDER!!!',
                'SELFDESTRUCT'
            ]
        };

        activate();

        function activate() {

        }
    }
})();