module.exports = function (grunt) {
    // .NET solution file which contains the APID.
    var solutionFile = '../Artivity.sln';

    // Build directories.
    var buildReleaseDir = '../build/Release';
    var buildDistDir = '../build/dist';

    // The order of the following files is important:
    var jsFiles = [
        'app/app.conf.js',
        'app/app.route.js',
        'app/app.api.js',
        'app/app.translation.js',
        'app/host/*.js',
        'app/util/*.js',
        'app/classes/*.js',
        'app/services/*.js',
        'app/filters/*.js',
        'app/partials/**/*.js'
    ];

    // These files are NOT bundled.
    var devFiles = [
        'Gruntfile.js',
        'updateVersion.js',
        'getVersion.js',
        'version.txt',
        '.vscode'
    ];

    // All the above files are bundled into this file in the relase version.
    var appJsFile = 'app/app.src.js';

    function ignore(file) {
        var minimatch = require("minimatch")

        if (file.startsWith("/node_modules") || file.startsWith("/bower_components"))
            return false;

        var patterns = devFiles.concat(jsFiles);

        for (var i = 0, len = patterns.length; i < len; i++) {
            if (minimatch(file, "/" + patterns[i])) {
                grunt.verbose.writeln("Ignore '" + file + "' for packaging.");
                return true;
            }
        }
        return false;
    }


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
                tasks: 'tags:build'
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
            },
            release: {
                src: appJsFile,
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
                dest: appJsFile,
                src: jsFiles
            }
        },
        clean: {
            options: {
                'force': true
            },
            build: [buildReleaseDir],
            dist: [buildDistDir]

        },
        electron: {
            windowsBuild: {
                options: {
                    name: 'Artivity',
                    dir: '.',
                    out: buildDistDir,
                    platform: 'win32',
                    icon: 'img/icon.ico',
                    arch: 'x64',
                    'app-copyright': 'Copyright © 2016 Semiodesk GmbH',
                    win32metadata: {
                        FileDescription: 'Artivity is a tool to track the creative process.',
                        OriginalFilename: 'Artivity.exe',
                        ProductName: 'Artivity',
                        InternalName: 'Artivity',
                        CompanyName: 'Semiodesk GmbH',

                    },
                    ignore: ignore

                }
            },
            macosBuild: {
                options: {
                    name: 'Artivity',
                    dir: '.',
                    out: buildDistDir,
                    platform: 'darwin',
                    icon: 'img/Icons.icns',
                    arch: 'x64'
                }
            }
        },
        assemblyVersion: {
            artivity: {
                src: [
                    '../Apid/Properties/AssemblyInfo.cs',
                    '../Apid.Windows/Properties/AssemblyInfo.cs',
                    '../DataModel/Properties/AssemblyInfo.cs'
                ],
                version: '<%= pkg.version %>'
            }
        },
        msbuild: {
            release: {
                src: [solutionFile],
                options: {
                    projectConfiguration: 'Release',
                    targets: ['Clean', 'Rebuild'],
                    version: 4.0,
                    maxCpuCount: 4,
                    platform: "Any CPU",
                    buildParameters: {
                        WarningLevel: 2
                    },
                    nodeReuse: false,
                    verbosity: 'quiet'
                }
            },
            // xbuild cannot handle multipe targets at the same time, so we separate them in two tasks
            cleanMac: {
                src: [solutionFile],
                options: {
                    projectConfiguration: "Release OSX",
                    targets: ['Clean'],
                    version: 4.0,
                    maxCpuCount: 4,
                    platform: "Any CPU",
                    buildParameters: {
                        WarningLevel: 2
                    },
                    verbosity: 'quiet'
                }
            },
            releaseMac: {
                src: [solutionFile],
                options: {
                    projectConfiguration: "Release OSX",
                    targets: ['Rebuild'],
                    version: 4.0,
                    maxCpuCount: 4,
                    platform: "Any CPU",
                    buildParameters: {
                        WarningLevel: 2
                    },
                    verbosity: 'quiet'
                }
            }

        },
        nugetRestore: {
            artivity: [solutionFile]
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
            mainMac: {
                files: [{
                        expand: true, // need expand: true with cwd
                        cwd: buildReleaseDir + '/artivity-apid.app',
                        src: '**',
                        dest: buildDistDir + '/Artivity-darwin-x64/Artivity.app/Contents/Applications/artivity-apid.app'
                    },
                    {
                        src: buildDistDir + '/Artivity-darwin-x64/Artivity.app/Contents/Resources/app/js/app.conf.release.js',
                        dest: buildDistDir + '/Artivity-darwin-x64/Artivity.app/Contents/Resources/app/js/app.conf.js',
                        force: true
                    },
                ],
            },
            mainWin: {
                files: [{
                        expand: true, // need expand: true with cwd
                        cwd: buildReleaseDir,
                        src: '**',
                        dest: buildDistDir + '/Artivity-win32-x64/apid/'
                    },
                    {
                        src: buildDistDir + '/Artivity-win32-x64/resources/app/js/app.conf.release.js',
                        dest: buildDistDir + '/Artivity-win32-x64/resources/app/js/app.conf.js',
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
        grunt.file.write(appJsFile, '');
        grunt.file.write(appJsFile + '.map', '');
    });

    // Task to restore nuget package
    grunt.registerMultiTask('nugetRestore', function () {
        path = require('path');
        var done = this.async();
        grunt.util.async.forEachSeries(this.data, function (filePath, next) {
            filePath = path.normalize(filePath);
            filePath = path.resolve(filePath);

            var nugetPath = path.resolve(path.normalize('../Utils/NuGet/nuget.exe'));
            var spawn;
            if (process.platform === "win32") {
                spawn = {
                    cmd: nugetPath,
                    args: ['restore', filePath],
                    opts: {
                        stdio: 'inherit'
                    }, // print to the same stdout
                };
            } else {
                spawn = {
                    cmd: 'mono',
                    args: [nugetPath, 'restore', filePath],
                    opts: {
                        stdio: 'inherit'
                    }, // print to the same stdout
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

    grunt.registerMultiTask('assemblyVersion', function () {
        const path = require('path');

        var version = this.data['version'];
        var done = this.async();
        grunt.util.async.forEachSeries(this.data['src'], function (filePath, next) {
            var targetFile = path.resolve(filePath);
            var assemblyInfoContent = grunt.file.read(targetFile);
            const regex = /Assembly(\w*)Version\(("[0-9*\.]+")\)/g;

            var output = assemblyInfoContent.replace(regex, 'Assembly$1Version("' + version + '")');

            grunt.file.write(targetFile, output);
            next();
        }, function () {
            // Do something with tasks now that each
            // contains their respective error code
            done();
        });
    });

    grunt.registerTask("bower-install", ["bower-install-simple"]);

    // Create a new task to wrap the two targets for macOS
    grunt.registerTask("xbuild", ["msbuild:cleanMac", "msbuild:releaseMac"]);

    grunt.registerTask('default', [
        'wiredep',
        'concat:js:clear',
        'tags:build',
        'sass:dev'
    ]);

    grunt.registerTask('release', [
        'wiredep',
        'concat:js:clear',
        'concat:js',
        'tags:release',
        'sass:dist'
    ]);

    if (process.platform === "win32") {
        grunt.registerTask('build', [
            'clean',
            'bower-install',
            'release',
            'electron:windowsBuild',
            'nugetRestore',
            'assemblyVersion',
            'msbuild:release',
            'copy:mainWin'
        ]);
    } else if (process.platform === "darwin") {
        grunt.registerTask('build', [
            'clean',
            'bower-install',
            'release',
            'electron:macosBuild',
            'nugetRestore',
            'assemblyVersion',
            'xbuild',
            'copy:mainMac'
        ]);
    }

};