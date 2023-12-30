var gutil = require('gulp-util');
var proxy = require("http-proxy-middleware");

function SpaProxy(project, context) {
		var noProxyDirs = ['part', 'view', 'ctrl', 'css', 'js'];
		var mavenProjectArray = [{project: project, context: context || project, path: '.'}];
		var util = {
				assertURL: function(url){
						url = url.replace('//','/');
						url = url.replace('//','/');
						return url;
				}
		};
		this.noProxy = {
				add: function (dir) {
						noProxyDirs.push(dir);
				},
				set: function (dirs) {
						noProxyDirs = dirs || [];
				},
				get: function () {
						return noProxyDirs;
				}
		};

		this.mavenProject = {
				add: function (project, context, path) {
						mavenProjectArray.push({project: project, context: context, path: path});
				},
				set: function (array) {
						mavenProjectArray = array || [];
				},
				get: function () {
						return mavenProjectArray;
				}
		};

		this.createRoutes = function () {
				var routes = {};
				mavenProjectArray.forEach(function (maven) {
						noProxyDirs.forEach(function (directory) {
								var key = '/' + maven.context + '/' + directory + '/';
								key = util.assertURL(key);
								var value = maven.path + '/' + maven.project + '/src/main/webapp/' + directory + '/';
								value = util.assertURL(value);
								routes[key] = value;
						});
				});
				return routes;
		};

		this.createRules = function () {
				var rules = [];
				rules.push('!/browser-sync/**');
				mavenProjectArray.forEach(function (maven) {
						noProxyDirs.forEach(function (directory) {
								var contextPath = '!/' + maven.context + '/' + directory + '/**';
								contextPath = util.assertURL(contextPath);
								rules.push(contextPath);
						});
				});
				rules.push('**');
				return rules;
		};

		this.createWatchPath = function () {
				var watchDir = [];
				mavenProjectArray.forEach(function (maven) {
						noProxyDirs.forEach(function (directory) {
								var directoryPath = maven.path + '/' + maven.project + '/src/main/webapp/' + directory + '/**/*.*';
								directoryPath = util.assertURL(directoryPath);
								watchDir.push(directoryPath);
						});
				});
				return watchDir;
		};

		this.createServer = function (targetServer) {
				var _routes = this.createRoutes();
				var _rules = this.createRules();
				gutil.log('Routes:', _routes);
				var server = {
						baseDir: [],
						routes: _routes
				};
				server.middleware = proxy(_rules, {
						target: targetServer,
						changeOrigin: true,
						ws: true,
						onProxyRes: function (proxyRes, req, res) {
								gutil.log('Proxy for:', req.url);
						}
				});
				return server;
		};
}

module.exports = function (project, context) {
		gutil.log('SPA Proxy for: ' + project + ' url:' + context);
		return new SpaProxy(project, context);
};
