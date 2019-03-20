/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');

function hello () {
	// place code for your default task here
	console.log('hello from gulp');
}

gulp.task('default', gulp.series(hello));
