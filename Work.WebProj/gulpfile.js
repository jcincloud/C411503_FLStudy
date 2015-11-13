
var gulp = require('gulp');
//var jshint = require('gulp-jshint');
var uglify = require('gulp-uglify');
//var rename = require('gulp-rename');
var concat = require('gulp-concat');
//var imagemin = require('gulp-imagemin');
var react = require('gulp-react');
var strip = require('gulp-strip-comments'); //移除註解
//後台共同引用的js打包處理
gulp.task('commHdl', function () {

    var jsfiles = [
        'Scripts/build/ts/dynScript/defData.js',
        'Scripts/app/moment/moment.min.js',
        'Scripts/app/moment/locale/zh-tw.js',
        'Scripts/jquery/jquery-2.1.3.min.js',
        'Scripts/react/react-with-addons.min.js',
        'Scripts/app/toastr.min.js',
        'Scripts/build/ts/commfunc.js',
        'Scripts/build/func/inc/c-Comm.js'
    ];

    return gulp.src(jsfiles)
        .pipe(strip({ safe: true }))
        .pipe(concat('comminc.js'))
        .pipe(gulp.dest('Scripts/build/'));
});
gulp.task('a-commHdl', function () {

    var jsfiles = [
        'Scripts/build/ts/dynScript/defData.js',
        'Scripts/jquery/jquery-2.1.3.min.js',
        'Scripts/angular/angular.js',
        'Scripts/angular/angular-animate.js',
        'Scripts/angular/angular-route.js',
        'Scripts/angular/i18n/angular-locale_zh-tw.js',
        'Scripts/angular/plugging/signalr-hub.js',
        'Scripts/angular/plugging/toaster.js',
        'Scripts/build/ts/commfunc.js',
    ];

    return gulp.src(jsfiles)
        .pipe(strip({ safe: true }))
        .pipe(concat('a-comminc.js'))
        .pipe(gulp.dest('Scripts/build/'));
});
//前台共同引用的js打包處理
gulp.task('commWHdl', function () {

    var jsfiles = [
        'Scripts/build/ts/dynScript/defData.js',
        'Scripts/app/moment/moment.min.js',
        'Scripts/app/moment/locale/zh-tw.js',
        'Scripts/jquery/jquery-2.1.3.min.js',
        'Scripts/react/react-with-addons.min.js',
        'Scripts/build/ts/commfunc.js',
        'Scripts/build/func/inc/c-Comm.js'
    ];

    return gulp.src(jsfiles)
        .pipe(strip({ safe: true }))
        .pipe(concat('commincW.js'))
        .pipe(gulp.dest('Scripts/build/'));
});
//react 處理
gulp.task('reactHdl', function () {
    var jsxfiles = ['Scripts/src/jsx/*.jsx'];

    return gulp.src(jsxfiles)
        .pipe(react())
        .pipe(uglify())
        .pipe(gulp.dest('Scripts/build/func/'));
});

gulp.task('reactIncHdl', function () {
    var jsxfiles = ['Scripts/src/jsx-inc/*.jsx'];

    return gulp.src(jsxfiles)
        .pipe(react())
        .pipe(uglify())
        .pipe(gulp.dest('Scripts/build/func/inc/'));
});

//typescript min化 處理
gulp.task('tsHdl', function () {
    var jsfiles = [
        'Scripts/ts-def/*.js',
        'Scripts/ts-def/d.ts/*.js',
        'Scripts/ts-def/dynScript/*.js',
        'Scripts/ts-def/widegt/*.js'
    ];

    return gulp.src(jsfiles, { base: 'Scripts/ts-def' })
        .pipe(uglify())
        .pipe(gulp.dest('Scripts/build/ts/'));
});
//default task
gulp.task('default', function () {
    gulp.run('tsHdl', 'reactHdl', 'reactIncHdl', 'commHdl', 'commWHdl');
    //監控react js 文件變化
    gulp.watch('Scripts/src/jsx/*.jsx', function () {
        gulp.run('reactHdl');
    });

    gulp.watch('Scripts/src/jsx-inc/*.jsx', function () {
        gulp.run('reactIncHdl');
        gulp.run('commHdl');
    });
});