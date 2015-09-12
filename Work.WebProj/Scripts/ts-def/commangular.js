var services;
(function (services) {
    var workService = (function () {
        function workService() {
        }
        return workService;
    })();
    services.workService = workService;
})(services || (services = {}));
;
angular.module('commfun', ['toaster']).service('workService', ['$http', 'toaster', function ($http, toaster) {
    this.getId = function (tab) {
        return $http.put(gb_approot + apiPutActionId, { tab: tab });
    };
    this.showToaster = function (type, title, message) {
        if (type == 0 /* success */)
            toaster.pop('success', title, message);
        if (type == 1 /* error */)
            toaster.pop('error', title, message);
        if (type == 2 /* wait */)
            toaster.pop('wait', title, message);
        if (type == 3 /* warning */)
            toaster.pop('warning', title, message);
        if (type == 4 /* note */)
            toaster.pop('note', title, message);
    };
}]);
angular.module('commfun').directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (inputValue) {
                if (inputValue == undefined)
                    return '';
                var transformedInput = inputValue.replace(/[^0-9+.]/g, '');
                if (transformedInput != inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }
                return transformedInput;
            });
        }
    };
});
angular.module('commfun').directive('capitalize', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {
                if (angular.isUndefined(inputValue))
                    return;
                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                }
                return capitalized;
            };
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]);
        }
    };
});
angular.module('commfun').directive('logout', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs, modelCtrl) {
            element.bind('click', function (e) {
                document.location.href = gb_approot + 'Sys_Base/MNGLogin/Logout';
            });
        }
    };
});
angular.module('commfun').directive('setnowusercookie', [function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var getuserid = $.cookie('user_id');
            if (getuserid != undefined) {
                modelCtrl.$setViewValue(getuserid);
            }
            ;
            element.bind('change', function (e) {
                modelCtrl.$parsers.push(function (inputValue) {
                    if (angular.isUndefined(inputValue))
                        return;
                    $.cookie('user_id', inputValue, { path: '/' });
                    return inputValue;
                });
            });
        }
    };
}]);
angular.module('commfun').factory('gridpage', ['$http', function ($http) {
    var struc = {
        CountPage: function ($scope) {
            $scope.NowPage = $scope.NowPage == undefined ? 1 : $scope.NowPage;
            var s = { page: $scope.NowPage };
            if ($scope.sd != null) {
                for (var key in $scope.sd) {
                    s[key] = $scope.sd[key];
                }
            }
            if ($scope.NowPage >= 1) {
                $http.get(apiConnection, { params: s }).success(function (data, status, headers, config) {
                    $scope.Grid_Items = data.rows;
                    $scope.TotalPage = data.total;
                    $scope.NowPage = data.page;
                    $scope.RecordCount = data.records;
                    $scope.StartCount = data.startcount;
                    $scope.EndCount = data.endcount;
                    $scope.firstpage = 1;
                    $scope.prevpage = $scope.NowPage <= 1 ? 1 : data.page - 1;
                    $scope.lastpage = data.total;
                    $scope.nextpage = $scope.NowPage >= $scope.TotalPage ? $scope.TotalPage : data.page + 1;
                });
            }
        }
    };
    return struc;
}]);
angular.module('commfun').filter('codelang', function () {
    return function (input, data) {
        var r = input;
        if (data) {
            for (var i = 0; i < data.length; i++) {
                if (input == data[i].value) {
                    r = data[i].label;
                    break;
                }
            }
        }
        return r;
    };
});
angular.module('commfun').filter('coin', function () {
    return function (input, data) {
        var r = input;
        if (data) {
            for (var i = 0; i < data.length; i++) {
                if (input == data[i].code) {
                    r = data[i].sign;
                    break;
                }
            }
        }
        return r;
    };
}).filter('left', function () {
    return function (input, string_length) {
        if (input.length > string_length) {
            return input.substring(0, string_length) + '...';
        }
        else {
            return input;
        }
    };
});
