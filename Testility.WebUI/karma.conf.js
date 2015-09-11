module.exports = function(config) {
  config.set({
    basePath: '',
    frameworks: ['jasmine', 'requirejs'],
    files: [
        { pattern: 'bower_components/**/*.js', included: false },
        { pattern: 'Scripts/app/**/*.js', included: false },
        { pattern: 'Scripts/test/**/*spec.js', included: false },
        { pattern: 'Scripts/test/**/*mock.js', included: false },
        'Scripts/test/test.main.js'
    ],
    exclude: ['**/*min.js'],
    preprocessors: {},
    reporters: ['progress'],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    browsers: ['PhantomJS'],
    singleRun: false
  })
}
