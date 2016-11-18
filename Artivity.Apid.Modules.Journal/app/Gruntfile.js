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
        }

    });

 
    grunt.loadNpmTasks('grunt-wiredep');

    // Default task
    grunt.registerTask('default', ['wiredep']);
 
};