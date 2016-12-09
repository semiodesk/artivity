module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({
        //Read the package.json (optional)
        pkg: grunt.file.readJSON('package.json'),

        wiredep: {
            task: {
                src: [
                    'index.html'
                ]
            }
        },

        sass: {
            dist: {
                options: {
                    style: 'compressed',
                    sourceMap: false
                },
                files: {
                    'css/style.css': 'css/style.scss'
                }
            },
            dev: {
                options: {
                    style: 'expanded',
                    sourceMap: true
                },
                files: {
                    'css/style.css': 'css/style.scss'
                }
            }
        },

        watch: {
            options: {
                livereload: true
            },
            files: ['css/*.scss'],
            tasks: 'sass:dev'
        },

        clean: {
            options: {
                'force': true
            },
            build: ['../../build/Release'],
            dist: ['../../build/dist']

        },

        electron: {
            windowsBuild: {
                options: {
                    name: 'Artivity',
                    dir: '.',
                    out: '../../build/dist',
                    platform: 'win32',
                    icon: 'img/icon.ico',
                    arch: 'x64',
                    win32metadata: {
                        FileDescription: 'Artivity is a tool to track the creative process.',
                        OriginalFilename: 'Artivity.exe',
                        ProductName: 'Artivity',
                        InternalName: 'Artivity',
                        CompanyName: 'Semiodesk GmbH',
                        Copyright: 'Copyright © 2016 Semiodesk GmbH'
                    },

                }
            }
        },

        msbuild: {
            release: {
                src: ['../../Artivity.sln'],
                options: {
                    projectConfiguration: 'Release',
                    targets: ['Clean', 'Rebuild'],
                    version: 4.0,
                    maxCpuCount: 4,
                    buildParameters: {
                        WarningLevel: 2
                    },
                    nodeReuse: false,
                    verbosity: 'quiet'
                }
            }
        },

        copy: {
            main: {
                files: [
                    {
                        expand: true, // need expand: true with cwd
                        cwd: '../../build/Release',
                        src: '**',
                        dest: '../../build/dist/Artivity-win32-x64/apid/'
                    },
                    {
                        src: '../../build/dist/Artivity-win32-x64/resources/app/js/app.conf.release.js',
                        dest: '../../build/dist/Artivity-win32-x64/resources/app/js/app.conf.js',
                        force: true
                    },
                ],
            },
        }
    });

    grunt.loadNpmTasks('grunt-wiredep');
    grunt.loadNpmTasks('grunt-concurrent');
    grunt.loadNpmTasks('grunt-sass');
    grunt.loadNpmTasks('grunt-electron');
    grunt.loadNpmTasks('grunt-msbuild');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-clean');

    // Default task
    grunt.registerTask('default', ['concurrent:wiredep', 'concurrent:sass:dev']);
    grunt.registerTask('build', ['clean', 'sass:dist', 'electron', 'msbuild', 'copy']);
};