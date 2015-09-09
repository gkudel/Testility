var TEST_REGEXP = /(UnitTest)\.js$/i;
var allTestFiles = [];
Object.keys(window.__karma__.files).forEach(function (file) {    
    if (TEST_REGEXP.test(file)) {
        allTestFiles.push(file);
    }
});

require.config({
    baseUrl: '/base/',

    paths: {
        angular: '/base/lib/angular/angular',
    },

    deps: allTestFiles,

    callback: window.__karma__.start
});