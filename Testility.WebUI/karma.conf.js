module.exports = function(config) {
  config.set({
    basePath: 'Scripts',
    frameworks: ['jasmine', 'requirejs'],
    files: [
        { pattern: 'lib/**/*.js', included: false },
        { pattern: 'app/**/*.js', included: false },
        { pattern: 'test/**/*UnitTest.js', included: false },
        'test/test-main.js'
    ],
    exclude: ['**/*min.js'],
    preprocessors: {},
    reporters: ['progress'],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: false,
    browsers: ['PhantomJS'],
    singleRun: false
  })
}
