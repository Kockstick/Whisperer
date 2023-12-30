# spa-proxy
SPA Proxy FrondEnd

This module add proxy for MAVEN PROJECT

## Required:

1. Project Maven: Any Project with same structure:
```
 project-pom/
   project-ejb/    <!--  Other Project -->
   project-xxx/    <!--  Other Project -->
   project-asset/  <!--  Web Project -->
   project-web/    <!--  Web Project -->
   gulpfile.js     <!--  Gulp File   -->
```


## Used:

### 1. Add in devDependencies
```
"spa-proxy": "^1.0.8"
```
### 2. Create a gulpfile.js 
```
var gulp = require('gulp');
var bs = require('browser-sync').create();
var spa = require('spa-proxy')('maven-project-web', '/path-deploy');
```
Create Gulp Task
```
//Create a Browser Sync Server
gulp.task("server", function () {
		var proxyURL = 'http://127.0.0.1:8080/'; //JEE Server
		bs.init({
				startPath: '/',
				server: spa.createServer(proxyURL),
				online: false,
				browser: ["firefox"],
				port: 3000
		});
});
//Create a watch change
gulp.task("watch", function () {
		var _watch = spa.createWatchPath();
		console.log("WATCH FOR: ", _watch);
		gulp.watch(_watch, function () {
				bs.reload();
		});
});
//Create a default task
gulp.task("default", ["watch", "server"]);
```


### 3. Custom Configurarion
```
spa = require('spa-proxy')('#MAVEN-PROJECT-WEB#', '#CONTEXT-PATH#')  
```
#### Example: 

```
 example-pom/
   example-ejb/    <!--  Other Project -->
   example-xxx/    <!--  Other Project -->
   example-asset/  <!--  Web Project -->
   example-web/    <!--  Web Project -->
   gulpfile.js     <!--  Gulp File   -->
```
MAVEN POM: example-pom
MAVEN WEB: example-asset
MAVEN WEB: example-web 
```
spa = require('spa-proxy')('example-web', '/path/demo')
```
Include other Maven Project
```
spa.mavenProject.add('example-asset', '/path/to/asset'); // Project in same directory
spa.mavenProject.add('other-project', '/path/to/other', '/path/absolute'); // Project in other directory
```


### 4. Custom Directory

Default Directory to static
```
var spa = require('spa-proxy')('maven-project-web', '/path-deploy');
console.log(' No Proxy Dir: ', spa.noProxy.get()); //part, view, ctrl, css, js
spa.noProxy.add('asset');
console.log(' No Proxy Dir: ', spa.noProxy.get()); //part, view, ctrl, css, js, asset
spa.noProxy.set(['css', 'asset']);
console.log(' No Proxy Dir: ', spa.noProxy.get()); //css, asset
```

This structure is based by AngularJS and HTML Basic Concept
 * The 'ctrl' directory replace to 'controllers' of angularjs
 * The 'view' directory replace to 'views' of angularjs
 * The 'part' directory groups of common html
 * The 'css'  directory groups files 'css'
 * The 'js'   directory groups files 'js'

Include Maven Project 
```
var spa = require('spa-proxy')('maven-project-web', '/path-deploy');
console.log(' Maven Projects: ', spa.mavenProject.get()); 
//{project: 'maven-project-web', context: '/path-deploy', path: '.'}
spa.mavenProject.add('other-maven-web', '/other-path', '/root/path');
console.log(' Maven Projects: ', spa.mavenProject.get()); 
//{project: 'maven-project-web', context: '/path-deploy', path: '.'}
//{project: 'other-project-web', context: '/other-path', path: '/root/path'}
```


