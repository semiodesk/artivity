

module.exports = function(grunt) {
 
    // Project configuration.
    grunt.initConfig({
 
        //Read the package.json (optional)
        pkg: grunt.file.readJSON('package.json'),
 
        // Metadata.
        //meta: {
        //    basePath: '../',
        //    srcPath: '../src/',
        //    deployPath: '../deploy/'
        //},
 
        wiredep: {
            task: {
                src: [
                    'index.html'
                ]
            }
        },

        sass: {
			dist: {
				files: {
					'css/style.css' : 'css/style.scss'
				}
			}
		},

        electron: {
        windowsBuild: {
            options: {
                name: 'journal',
                dir: '.',
                out: 'dist',
                platform: 'win32',
                arch: 'x64'
            }
        }
    }
    

    });

 
    grunt.loadNpmTasks('grunt-wiredep');
    grunt.loadNpmTasks('grunt-sass');
    grunt.loadNpmTasks('grunt-electron')

    // Default task
    grunt.registerTask('default', ['wiredep', 'sass']);

    grunt.registerTask('build', ['electron']);
 
};