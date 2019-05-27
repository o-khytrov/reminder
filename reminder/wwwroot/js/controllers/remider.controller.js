
angular
    .module('app')
    .controller('reminderCtrl', reminderCtrl);

reminderCtrl.$inject = ['$scope', 'AuthenticationService', 'NotesService', '$interval'];
function reminderCtrl($scope, AuthenticationService, NotesService, $interval) {
    var c = 0;
    $scope.editNote = false;
   
   
    resetMinDates();
    $scope.note = {
        eventDate: getClosestDate(),
        remindDate: moment(getClosestDate()).subtract(10, 'minutes').toDate(),
        title: "",
        description: "",
        place: ""

    };
    function getClosestDate() {
        var closestDate = moment().add(1, 'hours').minutes(0).seconds(0).milliseconds(0).toDate();
        return closestDate;
    }
    function resetMinDates() {
        $scope.minDate = formatDate(moment());
        $scope.minRemindDate = formatDate(moment());
      
    }
    $scope.getAllNotes = function () {
        NotesService.GetAll()
            .then(function success(response) {
                $scope.notes = response.data;
                $scope.message = '';
                $scope.errorMessage = '';
            },
                function error(response) {
                    $scope.message = '';
                    $scope.errorMessage = 'Error getting notes!';
                });
    };
    $interval(function () {
        $scope.message = "This DIV is refreshed " + c + " time.";
        c++;
    }, 3000); // refresh view every 3 sec
    $scope.UpdateNote = function (note) {
        $scope.note = note;
        $scope.openForm();
    };

    $scope.SaveNote = function () {

        if ($scope.note.id > 0) {

            NotesService.Update($scope.note)
                .then(function success(response) {
                    $scope.getAllNotes();
                    $scope.message = '';
                    $scope.errorMessage = '';
                    $scope.editNote = false;
                    alertify.message('Event is updated.');


                },
                    function error(response) {
                        $scope.message = '';
                        $scope.errorMessage = 'Error updating note!';
                        alertify.error('Error updating note!');

                    });
        }
        else {
            NotesService.Create({ UserId: '1', title: $scope.note.title, description: $scope.note.description, eventDate: $scope.note.eventDate, remindDate: $scope.note.remindDate, place: $scope.note.place })
                .then(function success(response) {
                    $scope.getAllNotes();
                    $scope.message = '';
                    $scope.errorMessage = '';
                    $scope.editNote = false;
                    alertify.success('Event is scheduled.');


                },
                    function error(response) {
                        $scope.message = '';
                        $scope.errorMessage = 'Error creating notes!';
                        alertify.success('Error creating notes!');
                    });

        }

    };
    $scope.DeleteNote = function (id) {
        alertify.confirm("Do you want remove this event?",
            function () {
                NotesService.Delete(id)
                    .then(function success(response) {
                        $scope.getAllNotes();
                        $scope.message = '';
                        $scope.errorMessage = '';
                        alertify
                            .alert("Event is deleted.", function () {
                                alertify.message('OK');
                            });

                    },
                        function error(response) {
                            $scope.message = '';
                            $scope.errorMessage = 'Error removing note';
                            alertify
                                .alert("Error removing note!", function () {
                                    alertify.message('OK');
                                });
                        });
            },
            function () {

            });

    };
    this.$onInit = function () {
        $scope.getAllNotes();
    };

    logOut = function () {

        AuthenticationService.logout();
    };

    $scope.openForm = function () {
        resetMinDates();
        $scope.editNote = true;
       
    };
    $scope.CloseForm = function () {
        $scope.editNote = false;
    };

    $scope.logOut = logOut;
    $scope.currentDateTime = new Date();

    $scope.showNote = function (note) {
        if ($scope.editNote) {
            return false;
        }
        note.eventDate = moment.utc(note.eventDate).toDate();
        note.remindDate = moment.utc(note.remindDate).toDate();
        if (note.eventDate > moment().toDate() && note.remindDate < moment().toDate()) {
            return true;
        }
        return false;
    };

    $scope.onEventDateChange = function () {

        resetMinDates();
    };
    function formatDate(now) {
        return moment(now).format("YYYY-MM-DDTHH:00:00");

    }

}
