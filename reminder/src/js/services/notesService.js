(function () {
    'use strict';

    angular
        .module('app')
        .factory('NotesService', NotesService);

    NotesService.$inject = ['$http'];
    function NotesService($http) {
        var service = {};

        service.GetAll = GetAll;
        service.GetById = GetById;
        service.Create = Create;
        service.Update = Update;
        service.Delete = Delete;
        $http.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('auth_token');
        return service;

        function GetAll() {
            return $http.get('/api/notes').then(handleSuccess, handleError('Error getting all note'));
        }

        function GetById(id) {
            return $http.get('/api/notes' + id).then(handleSuccess, handleError('Error getting note by id'));
        }

        function Create(note) {
            return $http.post('/api/notes', note).then(handleSuccess, handleError('Error creating note'));
        }

        function Update(note) {
            return $http.put('/api/notes', note).then(handleSuccess, handleError('Error updating note'));
        }

        function Delete(id) {
            return $http.delete('/api/notes/' + id).then(handleSuccess, handleError('Error deleting note'));
        }

        // private functions

        function handleSuccess(res) {
            return res.data;
        }

        function handleError(error) {
            return function () {
                return { success: false, message: error };
            };
        }
    }

})();
