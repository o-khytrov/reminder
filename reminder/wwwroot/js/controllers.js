
angular
    .module('app')
    .controller('reminderCtrl', reminderCtrl)

reminderCtrl.$inject = ['$scope'];
function reminderCtrl($scope) {
    $scope.notes = [
        { title:"Make call",description: "Make call and order pizza", dateTime: new Date(2019, 5, 24, 12, 30, 0), place:"London" },
        { title: "Go to shop", description: "Go to shop and by some food", dateTime: new Date(2019, 5, 24, 15, 30, 0),place:"New York" }
    ];
    $scope.AddData = function () {

    }
}
