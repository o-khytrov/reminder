
angular
    .module('app')
    .controller('reminderCtrl', reminderCtrl);

reminderCtrl.$inject = ['$scope', 'AuthenticationService', 'NotesService', '$interval'];
function reminderCtrl($scope, AuthenticationService, NotesService, $interval) {
    var currentDate = new Date();
    var c = 0;
    $scope.editNote = false;
    $scope.minDate = formatDate(new Date());
    $scope.note = {
        eventDate: new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), currentDate.getHours()),
        remindDate: new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), currentDate.getHours()),
        title:"",
        description:"",
        place:""

    };
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
        $scope.note.eventDate= new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), currentDate.getHours()),
        $scope.note.remindDate= new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate(), currentDate.getHours()),

        $scope.openForm();
    }

    $scope.SaveNote = function () {
       
        if ($scope.note.id > 0) {
            NotesService.Update($scope.note)
                .then(function success(response) {
                    $scope.getAllNotes();
                    $scope.message = '';
                    $scope.errorMessage = '';
                    $scope.editNote = false;
                    alertify
                        .alert("Event is updated.", function () {
                            alertify.message('OK');
                        });

                },
                    function error(response) {
                        $scope.message = '';
                        $scope.errorMessage = 'Error updating note!';
                        alertify
                            .alert("Error updating note!", function () {
                                alertify.message('OK');
                            });
                    });
        }
        else {
            NotesService.Create({ UserId: '1', title: $scope.note.title, description: $scope.note.description, eventDate: $scope.note.eventDate, remindDate: $scope.note.remindDate, place: $scope.note.place })
                .then(function success(response) {
                    $scope.getAllNotes();
                    $scope.message = '';
                    $scope.errorMessage = '';
                    $scope.editNote = false;
                    alertify
                        .alert("Event is scheduled.", function () {
                            alertify.message('OK');
                        });

                },
                    function error(response) {
                        $scope.message = '';
                        $scope.errorMessage = 'Error creating notes!';
                        alertify
                            .alert("Error creating notes!", function () {
                                alertify.message('OK');
                            });
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

    $scope.AddData = function () {

    };
    logOut = function () {

        AuthenticationService.logout();
    };
    $scope.openForm = function () {
        $scope.editNote = true;
        $scope.minDate = formatDate(new Date());
    }
    $scope.CloseForm = function () {
        $scope.editNote = false;
    }
    $scope.logOut = logOut;
    $scope.currentDateTime = new Date();
    $scope.showNote = function (note) {

        var eventDate = Date.parse(note.eventDate + "Z");
        var remindDate = Date.parse(note.remindDate + "Z");
        if (eventDate > new Date() && remindDate < new Date()) {
            return true;
        }
        return false;
    }

    function formatDate(today) {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var hours = today.getHours();
        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' +dd  + 'T' + hours+':00:00';
        return today;
    }

}
