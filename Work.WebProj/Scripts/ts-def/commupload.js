angular.module('commbox', ['angularFileUpload'])
    .directive('filehandleshadowbox', ['$http', '$q', '$upload', function ($http, $q, $upload) {
        return {
            restrict: 'E',
            templateUrl: gb_approot + 'ScriptsCtrl/tmp/fileuploadb.html',
            replace: true,
            scope: {
                sysid: '=?sysid',
                filekind: '@'
            },
            link: function (scope, element, attrs) {
            },
            controller: ['$scope', function ($scope) {
                    $scope.FileUpload = function ($files) {
                        var deferred = $q.defer();
                        var f_finish = [];
                        for (var i = 0; i < $files.length; i++) {
                            var file = $files[i];
                            f_finish.push({ name: file.name, finish: false });
                        }
                        for (var i = 0; i < $files.length; i++) {
                            var file = $files[i];
                            console.info(file.name);
                            var uploadfile = $upload.upload({
                                url: gb_allpath + 'aj_FUpload',
                                method: 'POST',
                                data: { id: $scope.sysid, FilesKind: $scope.filekind },
                                file: file
                            }).progress(function (evt) {
                                console.info('percent', 100.0 * evt.loaded / evt.total);
                            }).success(function (data, status, headers, config) {
                                if (!data.result) {
                                    alert(data.error);
                                }
                                console.info(data.FileName, 'Finish!');
                                for (var j = 0; j < f_finish.length; j++) {
                                    if (f_finish[j].name == data.FileName) {
                                        f_finish[j].finish = true;
                                        console.info('f_finish', j, data.FileName);
                                    }
                                }
                                var checkAll = true;
                                for (var j = 0; j < f_finish.length; j++) {
                                    checkAll = checkAll && f_finish[j].finish;
                                }
                                console.info('checkall', checkAll);
                                if (checkAll) {
                                    $scope.FileList();
                                    deferred.resolve(true);
                                }
                            });
                        }
                        return deferred.promise;
                    };
                    $scope.FileList = function () {
                        console.log('directive id', $scope.sysid);
                        $http.post(gb_allpath + 'aj_FList', { id: $scope.sysid, FileKind: $scope.filekind })
                            .success(function (data, status, headers, config) {
                            $scope.filedata = data.filesObject;
                            console.log('directive get file', $scope.filedata);
                        });
                    };
                    $scope.FileDelete = function (filename) {
                        $http.post(gb_allpath + 'aj_FDelete', { id: $scope.sysid, FileKind: $scope.filekind, FileName: filename })
                            .success(function (data, status, headers, config) {
                            if (!data.result) {
                                alert(data.error);
                            }
                            else {
                                $scope.FileList();
                            }
                        });
                    };
                    $scope.$watch('sysid', function (newValue, oldValue) {
                        if (newValue != undefined) {
                            $scope.FileList();
                        }
                    });
                    console.log('file info', $scope.fd, $scope.filekind);
                }]
        };
    }]);
