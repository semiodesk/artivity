

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
                files: {
                    'css/style.css': 'css/style.scss'
                }
            }
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
                ],
            },
        },
        'create-windows-installer': {
            x64: {
                appDirectory: '../../build/dist/Artivity-win32-x64',
                outputDirectory: '../../build/dist/installer',
                exe: 'Artivity.exe',
            },
        }



    });


    grunt.loadNpmTasks('grunt-wiredep');
    grunt.loadNpmTasks('grunt-sass');
    grunt.loadNpmTasks('grunt-electron');
    grunt.loadNpmTasks('grunt-electron-installer');
    grunt.loadNpmTasks('grunt-msbuild');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-clean');


    // Default task
    grunt.registerTask('default', ['wiredep', 'sass']);

    grunt.registerTask('build', ['clean', 'electron', 'msbuild', 'copy', 'create-windows-installer']);

};