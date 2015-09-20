/// <binding Clean='karma:unit' />
/*'use strict';

module.exports = function(grunt) {
    grunt.loadNpmTasks('grunt-wiredep');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-karma');
 
    var appConfig = {
        app: require('./bower.json').appPath || 'app'
    };

    grunt.initConfig({
        karma: {
            unit: {
                configFile: 'karma.conf.js',
                autoWatch: true
            }
        },
        watch: {
            bower: {
                files: ['bower.json'],
                tasks: ['wiredep']
            }
        },
        wiredep: {
            test: {
                exclude: [
                    /jquery.validation/,
                    /Microsoft.jQuery.Unobtrusive.Validation/],
                dependencies: true,
                devDependencies: false,
                src: 'App_Start/BundleConfig.cs',
                ignorePath: '..',
                fileTypes: {
                    cs: {
                        block: /(([ \t]*)\/\/\s*bower:*(\S*)\s*)(\n|\r|.)*?(\/\/\s*endbower\s*)/gi,
                        detect: {
                            js: /<script.*src=['"](.+)['"]>/gi,
                            css: /<link.*href=['"](.+)['"]/gi
                        },
                        replace: {
                            js: '.Include("~{{filePath}}\")',
                            css: '.Include("~{{filePath}}\")'
                        }
                    }
                }
            }
        }
    });
	
    grunt.registerTask('build', [
        'wiredep'
    ]);
};
*/