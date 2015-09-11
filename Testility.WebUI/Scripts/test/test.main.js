﻿var TEST_REGEXP_FINISH = /(spec)\.js$/i;
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
        testility: '/base/Scripts/app/testility.module',
        solutionController: '/base/Scripts/app/solution/controllers/solutioncontroller',
        //Mocks
        setupserviceMock: '/base/Scripts/test/mocks/solutionservice.mock',
        restangularMock: '/base/Scripts/test/mocks/restangular.mock',
        uiCodemirrorMock: '/base/Scripts/test/mocks/ui.codemirror.mock',
        uiBrowserMock: '/base/Scripts/test/mocks/ui.browser.mock',
        uiMessagingMock: '/base/Scripts/test/mocks/ui.messaging.mock',
        uiDialogboxMock: '/base/Scripts/test/mocks/ui.dialogbox.mock',
        uiSpinerMock: '/base/Scripts/test/mocks/ui.spiner.mock',
        validationMock: '/base/Scripts/test/mocks/validation.mock'
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
        },
        'solutionController': {
            deps: ['angular', 'angularMocks', 'testility']
        },
        //Mocks
        'setupserviceMock': {
            deps: ['angular', 'angularMocks', 'testility']
        },
        'restangularMock': {
            deps: ['angular', 'angularMocks']
        },
        'uiCodemirrorMock': {
            deps: ['angular', 'angularMocks']
        },
        'uiBrowserMock': {
            deps: ['angular', 'angularMocks']
        },
        'uiMessagingMock': {
            deps: ['angular', 'angularMocks']
        },
        'uiDialogboxMock': {
            deps: ['angular', 'angularMocks']
        },
        'uiSpinerMock': {
            deps: ['angular', 'angularMocks']
        },
        'validationMock': {
            deps: ['angular', 'angularMocks']
        }
    },

    priority: [
		'angular'
    ],

    deps: allTestFiles,

    callback: window.__karma__.start
});