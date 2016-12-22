module.exports = function (grunt) {
    var jsFiles = [
        'js/host/*.js',
        'js/lib/*.js',
        'js/lib/classes/*.js',
        'js/lib/filters/*.js',
        'js/lib/services/*.js',
        'partials/directives/*.js',
        'partials/dialogs/*.js',
        'partials/*.js'
    ];

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
            js: {
                options: {
                    livereload: true
                },
                files: jsFiles,
                tasks: 'tags'
            },
            sass: {
                options: {
                    livereload: true
                },
                files: ['css/*.scss'],
                tasks: 'sass:dev'
            }
        },

        tags: {
            build: {
                src: jsFiles,
                dest: 'index.html'
            }
        },

        concat: {
            js: {
                options: {
                    banner: "(function () {\n\n'use strict';\n\n",
                    footer: "\n})();",
                    sourceMap: true
                },
                nonull: true,
                dest: 'js/app.src.js',
                src: [
                    'js/lib/*.js',
                    'js/host/*.js',
                    'js/lib/classes/*.js',
                    'js/lib/filters/*.js',
                    'js/lib/services/*.js',
                    'partials/directives/*.js',
                    'partials/dialogs/*.js',
                    'partials/*.js'
                ]
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

        nugetRestore: {
            artivity: ["../../Artivity.sln"]

        },

        "bower-install-simple": {
            options: {
                color: true,
            },
            "prod": {
                options: {
                    production: true
                }
            },
            "dev": {
                options: {
                    production: false
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
    grunt.loadNpmTasks('grunt-script-link-tags');
    grunt.loadNpmTasks('grunt-electron');
    grunt.loadNpmTasks('grunt-msbuild');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks("grunt-bower-install-simple");

    grunt.registerTask('concat:js:clear', 'Empties the concatenated app source files.', function () {
        // TODO: Read dest file from config 'concat:js:dest'.
        grunt.file.write('js/app.src.js', '');
        grunt.file.write('js/app.src.js.map', '');
    });

    // Task to restore nuget package
    grunt.registerMultiTask('nugetRestore', function () {
        path = require('path');
        var done = this.async();
        grunt.util.async.forEachSeries(this.data, function (filePath, next) {
            filePath = path.normalize(filePath);
            filePath = path.resolve(filePath);

            var nugetPath = path.resolve(path.normalize('../../Utils/NuGet/nuget.exe'));
            var spawn;
            if (process.platform === "win32") {
                spawn = {
                    cmd: nugetPath,
                    args: ['restore', filePath],
                    opts: { stdio: 'inherit' }, // print to the same stdout
                };
            } else {
                spawn = {
                    cmd: 'mono',
                    args: [nugetPath, 'restore', filePath],
                    opts: { stdio: 'inherit' }, // print to the same stdout
                }
            }

            grunt.util.spawn(spawn, function (err, result, code) {
                next();
            });
        }, function () {
            // Do something with tasks now that each
            // contains their respective error code
            done();
        });
    });

    grunt.registerTask("bower-install", ["bower-install-simple"]);

    grunt.registerTask('default', [
        'wiredep',
        'concat:js:clear',
        'tags',
        'sass:dev'
    ]);

    grunt.registerTask('build', [
        'clean',
        "bower-install",
        'wiredep',
        'concat:js',
        'sass:dist',
        'electron',
        'nugetRestore',
        'msbuild',
        'copy'
    ]);
};