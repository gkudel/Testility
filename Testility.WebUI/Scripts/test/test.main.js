var TEST_REGEXP_FINISH = /(spec)\.js$/i;
var TEST_REGEXP_BEGIN = /^\/base\/Scripts\/test\//i;
var allTestFiles = [];
Object.keys(window.__karma__.files).forEach(function (file) {    
    if (TEST_REGEXP_FINISH.test(file) && TEST_REGEXP_BEGIN.test(file)) {
        allTestFiles.push(file);
    }
});

require.config({
    baseUrl: '/base/',

    paths: {
        angular: '/base/bower_components/angular/angular',
        angularMocks: '/base/bower_components/angular-mocks/angular-mocks',
        testility: '/base/Scripts/app/testility.module'
    },
    shim: {
        'angular': { 'exports': 'angular' },
        'angularMocks': {
            deps: ['angular'],
            'exports': 'angular.mock'
        },
        'testility': {
            deps: ['angular', 'angularMocks'],
            'exports': 'testility'
        }

    },

    priority: [
		'angular'
    ],

    deps: allTestFiles,

    callback: window.__karma__.start
});