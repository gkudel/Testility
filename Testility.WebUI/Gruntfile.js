'use strict';

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
                configFile: 'karma.conf.js'
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
                devDependencies: true,
                src: './App_Start/BundleConfig.cs',
                ignorePath: /\.\.\//,
                fileTypes: {
                    cs: {
                        block: /(([\s\t]*)\/{2}\s*?bower:\s*?(\S*))(\n|\r|.)*?(\/{2}\s*endbower)/gi,
                        detect: {
                            js: /'(.*\.js)'/gi
                        },
                        replace: {
                            js: '\"~/{{filePath}}\",'
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
